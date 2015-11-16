$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetDayAmount", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var CustId = [];
        //分钟级交易量
        var TradeAmount = [];
        //获取服务器日期
        var Date = result[0].TradeDate;
        //单位
        var unit = "";
        //获取当日第五名交易量
        var minresult = result[0].TradeAmount;
        for (var i = 0; i < result.length; i++) {
            if (minresult > result[i].TradeAmount) {
                minresult = result[i].TradeAmount;
            }
        }
        //实时曲线数据转换
        if (result[0].CustId != null && minresult / 1000000 >= 1) {
            for (var i = 0; i < result.length; i++) {
                CustId.push(result[i].CustId);
                TradeAmount.push(result[i].TradeAmount / 1000000);
            }
            unit = "百万元";
        }
        else {
            for (var i = 0; i < result.length; i++) {
                CustId.push(result[i].CustId);
                TradeAmount.push(result[i].TradeAmount);
            }
            unit = "元";
        }
        
        $(function () {
            $(document).ready(function () {
                Highcharts.setOptions({
                    global: {
                        useUTC: false
                    }
                });

                var chart;
                $('#TradeAmountTop').highcharts({
                    chart: {
                        type: 'bar',
                        animation: Highcharts.svg, // don't animate in old IE               
                        marginRight: 10,
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series = this.series[0];
                                var xAxis = this.xAxis[0];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetTradeDate/GetDayAmount",
                                        dataType: "json",
                                        success: function (data) {
                                            //实时更新图对应时间
                                            var rCustId = [];
                                            //分钟级交易量
                                            var rTradeAmount = [];
                                            //实时曲线数据转换
                                            if (data[0].CustId != null) {
                                                for (var i = 0; i < data.length; i++) {
                                                    rCustId[i] = data[i].CustId;
                                                    rTradeAmount[i] = data[i].TradeAmount;
                                                }
                                            }
                                            xAxis.setCategories(rCustId);
                                            series.setData(rTradeAmount);
                                        }
                                    });

                                }, 60000);
                            }
                        }
                    },
                    title: {
                        text: '今日客户交易量排名<br>(' + Date +')'
                    },
                    xAxis: {
                        categories:CustId
                    },
                    yAxis: {
                        title: {
                            text: '交易量'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    tooltip: {
                        formatter: function () {
                            return '客户ID' + this.x + '<br>' +
                               this.series.name + ': ' + Highcharts.numberFormat(this.y, 0, '.') + unit;
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    series: [{
                        name: '交易量',
                        data: TradeAmount,
                        color: '#f7a35c',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            align: 'right',
                            style: {
                                fontSize: '13px',
                                fontFamily: 'Verdana, sans-serif',
                                textShadow: '0 0 0 black'
                            },
                            formatter: function () {
                                return this.y + unit
                            }
                        }
                    }],
                    lang: {
                    noData: "Nichts zu anzeigen"
                },
                noData: {
                    style: {
                            fontWeight: 'bold',
                            fontSize: '15px',
                            color: '#303030'
                    }
                }
                });
            });

        });
    }
});
