$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetCreditBuyAmount", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var StockName = [];
        //分钟级交易量
        var TradeAmount = [];
        //获取服务器日期
        //var Date = result[0].TradeDate;

        //单位
        var unit = "";
        if (result.length > 0) {
            //获取当日第十名交易量
            var minresult = result[0].MatchQty;
            for (var i = 0; i < result.length; i++) {
                if (minresult > result[i].MatchQty) {
                    minresult = Number(result[i].MatchQty);
                }
            }
            //实时曲线数据转换
            if (result[0].StockName != null && minresult / 1000000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    StockName.push(result[i].StockName);
                    TradeAmount.push(Number(result[i].MatchQty) / 1000000);
                }
                unit = "百万股";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    StockName.push(result[i].StockName);
                    TradeAmount.push(Number(result[i].MatchQty));
                }
                unit = "股";
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
                $('#CreditBuyAmountTop').highcharts({
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
                                        url: "/DashBoard/api/GetTradeDate/GetCreditBuyAmount",
                                        dataType: "json",
                                        success: function (data) {
                                            //实时更新图对应时间
                                            var rStockName = [];
                                            //分钟级交易量
                                            var rTradeAmount = [];
                                            if (data.length > 0) {
                                                //实时曲线数据转换
                                                if (data[0].StockName != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        rStockName[i] = data[i].StockName;
                                                        rTradeAmount[i] = data[i].MatchQty;
                                                    }
                                                }
                                                xAxis.setCategories(rStockName);
                                                series.setData(rTradeAmount);
                                            }
                                        }
                                    });

                                }, 60000);
                            }
                        }
                    },
                    title: {
                        text: '融券买入交易标的前10'
                    },
                    xAxis: {
                        categories: StockName
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
                            return '' + this.x + '<br>' +
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
