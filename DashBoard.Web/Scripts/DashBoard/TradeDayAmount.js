$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetDayAmount", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var CustId = [];
        //总交易量
        var TradeAmount = [];
        //信用交易量
        var CreditTrade = [];
        //普通交易量
        var NormalTrade = [];
        //单位
        var unit = "";
        if (result.length > 0) {
            ////获取服务器日期
            //var Date = result[0].tradeDate;

            //获取当日第五名交易量
            var minresult = result[0].tradeAmount;
            for (var i = 0; i < result.length; i++) {
                if (minresult > result[i].tradeAmount) {
                    minresult = result[i].tradeAmount;
                }
            }
            //实时曲线数据转换
            if (result[0].custId != null && minresult / 1000000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    CustId.push(result[i].custId);
                    TradeAmount.push(result[i].tradeAmount / 1000000);
                    if (result[i].creditTrade > 0) {
                        CreditTrade.push(result[i].creditTrade / 1000000);
                    } else {
                        CreditTrade.push(0);
                    }
                    NormalTrade.push((result[i].tradeAmount - result[i].creditTrade) / 1000000);
                }
                unit = "百万元";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    CustId.push(result[i].custId);
                    TradeAmount.push(result[i].tradeAmount);
                    if (result[i].creditTrade > 0) {
                        CreditTrade.push(result[i].creditTrade);
                    } else {
                        CreditTrade.push(0);
                    }
                    NormalTrade.push(result[i].tradeAmount - result[i].creditTrade);
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
                $('#TradeAmountTop').highcharts({
                    chart: {
                        type: 'bar',
                        animation: Highcharts.svg, // don't animate in old IE               
                        marginRight: 10,
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series0 = this.series[0];
                                var series1 = this.series[1];
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
                                            //信用交易量
                                            var rCreditTrade = [];
                                            //普通交易量
                                            var rNormalTrade = [];
                                            if (data.lenth > 0) {
                                                //实时曲线数据转换
                                                if (data[0].custId != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        rCustId[i] = data[i].custId;
                                                        rTradeAmount[i] = data[i].tradeAmount;
                                                        if (data[i].creditTrade > 0) {
                                                            rCreditTrade.push(data[i].creditTrade);
                                                        } else {
                                                            rCreditTrade.push(0);
                                                        }
                                                        rNormalTrade.push(data[i].tradeAmount - data[i].creditTrade);
                                                    }
                                                }
                                                xAxis.setCategories(rCustId);
                                                series0.setData(rCreditTrade);
                                                series1.setData(rNormalTrade);
                                            }
                                        }
                                    });

                                }, 60000);
                            }
                        }
                    },
                    title: {
                        text: '今日客户交易金额排名'
                    },
                    xAxis: {
                        categories: CustId
                    },
                    yAxis: {
                        title: {
                            text: '交易金额'
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
                               this.series.name + ': ' + Highcharts.numberFormat(this.y, 2, '.') + unit;
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    plotOptions: {
                        series: {
                            stacking: 'percent'
                        }
                    },
                    series: [{
                        name: '信用交易',
                        data: CreditTrade,
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
                                return this.series.name + '<br>' + Highcharts.numberFormat(this.y, 2, '.') + unit
                            }
                        }
                    }, {
                        name: '普通交易',
                        data: NormalTrade,
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
                                return this.series.name + '<br>' + Highcharts.numberFormat(this.y, 2, '.') + unit
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
