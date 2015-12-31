$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetStrategyTradeAmt", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var StrategyName = [];
        //交易金额
        var TradeAmount = [];
        //单位
        var unit = "";
        if (result.length > 0) {
            //获取服务器日期
            var Date = result[0].TradeDate;
            //获取当日第三名交易量
            var minresult = result[0].StrategyTradeAmt;
            for (var i = 0; i < result.length; i++) {
                if (minresult > result[i].StrategyTradeAmt) {
                    minresult = Number(result[i].StrategyTradeAmt);
                }
            }
            //实时曲线数据转换
            if (result[0].StrategyType != null && minresult / 1000000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    StrategyName.push(result[i].StrategyType);
                    TradeAmount.push(Number(result[i].StrategyTradeAmt) / 1000000);
                }
                unit = "百万元";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    StrategyName.push(result[i].StrategyType);
                    TradeAmount.push(Number(result[i].StrategyTradeAmt));
                }
                unit = "元";
            }
        }
        $(function () {
            $(document).ready(function () {
                Highcharts.setOptions({
                    global: {
                        useUTC: false
                    }
                });

                var chart;
                $('#StrategyTradeAmt').highcharts({
                    chart: {
                        high: '400px',
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
                                        url: "/DashBoard/api/GetTradeDate/GetStrategyTradeAmt",
                                        dataType: "json",
                                        success: function (data) {
                                            //实时更新图对应时间
                                            var rStrategyType = [];
                                            //分钟级交易量
                                            var rTradeAmount = [];
                                            if (data.length > 0) {
                                                //实时曲线数据转换
                                                if (data[0].StrategyType != null) {
                                                    //获取当日第三名交易量
                                                    var rminresult = data[0].StrategyTradeAmt;
                                                    for (var i = 0; i < result.length; i++) {
                                                        if (rminresult > result[i].StrategyTradeAmt) {
                                                            rminresult = Number(result[i].StrategyTradeAmt);
                                                        }
                                                    }
                                                    
                                                    if (minresult / 1000000 >= 1) {
                                                        for (var i = 0; i < data.length; i++) {
                                                            rStrategyType[i] = data[i].StrategyType;
                                                            rTradeAmount[i] = data[i].StrategyTradeAmt / 1000000;
                                                        }
                                                        unit = "百万元";
                                                    }
                                                    else {
                                                        for (var i = 0; i < data.length; i++) {
                                                            rStrategyType[i] = data[i].StrategyType;
                                                            rTradeAmount[i] = data[i].StrategyTradeAmt;
                                                        }
                                                        unit = "元";
                                                    }
                                                }
                                                xAxis.setCategories(rStrategyType);
                                                series.update({
                                                    data: rTradeAmount,
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
                                                            return Highcharts.numberFormat(this.y, 2, '.') + unit
                                                        }
                                                    }
                                                });
                                            }
                                        }
                                    });

                                }, 1000 * 60);
                            }
                        }
                    },
                    title: {
                        text: '本月策略开仓'
                    },
                    xAxis: {
                        categories: StrategyName
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
                    credits: {
                        enabled: false
                    },
                    tooltip: {
                        formatter: function () {
                            return '' + this.x + '<br>' +
                               this.series.name + ': ' + Highcharts.numberFormat(this.y, 2, '.') + unit;
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
                                return Highcharts.numberFormat(this.y, 2, '.') + unit
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
