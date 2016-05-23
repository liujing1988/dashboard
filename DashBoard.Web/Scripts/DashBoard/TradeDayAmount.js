
//当日交易金额TOP10

$.ajax(
{
    url: "/dashboard/api/GetTradeDate/GetDayAmount", //表示提交给的action 
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

            //获取当日第一名交易量
            var maxresult = result[0].TradeAmount;
            for (var i = 0; i < result.length; i++) {
                if (maxresult < result[i].TradeAmount) {
                    maxresult = result[i].TradeAmount;
                }
            }
            //实时曲线数据转换
            if (result[0].CustId != null && maxresult / 1000000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    CustId.push(result[i].CustId);
                    TradeAmount.push(result[i].TradeAmount / 1000000);
                    if (result[i].CreditTrade > 0) {
                        CreditTrade.push(result[i].CreditTrade / 1000000);
                    } else {
                        CreditTrade.push(0);
                    }
                    NormalTrade.push((result[i].TradeAmount - result[i].CreditTrade) / 1000000);
                }
                unit = "百万元";
            }
            else if (result[0].CustId != null && maxresult / 10000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    CustId.push(result[i].CustId);
                    TradeAmount.push(result[i].TradeAmount / 10000);
                    if (result[i].CreditTrade > 0) {
                        CreditTrade.push(result[i].CreditTrade / 10000);
                    } else {
                        CreditTrade.push(0);
                    }
                    NormalTrade.push((result[i].TradeAmount - result[i].CreditTrade) / 10000);
                }
                unit = "万元";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    CustId.push(result[i].CustId);
                    TradeAmount.push(result[i].TradeAmount);
                    if (result[i].CreditTrade > 0) {
                        CreditTrade.push(result[i].CreditTrade);
                    } else {
                        CreditTrade.push(0);
                    }
                    NormalTrade.push(result[i].TradeAmount - result[i].CreditTrade);
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
                        high: '400px',
                        type: 'bar',
                        animation: Highcharts.svg, // don't animate in old IE               
                        marginRight: 10
                    },
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
                                                if (data[0].CustId != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        rCustId[i] = data[i].CustId;
                                                        rTradeAmount[i] = data[i].TradeAmount;
                                                        if (data[i].CreditTrade > 0) {
                                                            rCreditTrade.push(data[i].CreditTrade);
                                                        } else {
                                                            rCreditTrade.push(0);
                                                        }
                                                        rNormalTrade.push(data[i].TradeAmount - data[i].CreditTrade);
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
                    },
                    title: {
                        text: '今日客户交易金额排名'
                    },
                    xAxis: {
                        categories: CustId
                    },
                    credits: {
                        enabled: false
                    },
                    yAxis: {
                        title: {
                            text: '交易金额'
                        }
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
                            stacking: 'normal'
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
                            align: 'left',
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
                        noData: "无数据！"
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
