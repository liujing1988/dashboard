

//刷新频率
var refresh = 60;

function GetBuyData(da) {
    $.ajax({
        url: "/DashBoard/api/GetTradeDate/GetCreditBuyAmount", //表示提交给的action 
        type: "post",   //提交方法 
        data: da, //传递参数
        datatype: "json",//数据类型
        success: function (result) {
            //股票名称
            var StockName = [];
            //交易量
            var TradeAmount = [];
            //单位
            var unit = "";
            if (result.length > 0) {
                //刷新频率
                refresh = result[0].RefreshRate;
                //获取当日第十名交易量
                var minresult = result[0].MatchQty;
                for (var i = 0; i < result.length; i++) {
                    if (Number(minresult) > Number(result[i].MatchQty)) {
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
                                    data: da,
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
                            }, refresh * 1000);
                        }
                    }
                },
                title: {
                    text: '信用账户净买入担保标的前10'
                },
                xAxis: {
                    categories: StockName
                },
                credits: {
                    enabled: false
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
        }
    });
}
$(function () {
    $(document).ready(function () {
        var da = {
            "Day": "1"
        };
        GetBuyData(da);
        $("button.creditbuy").click(function () {
            var type = $(this).html();

            if (type == "最近一月") {
                var da = {
                    "Day": "30"
                };
            } else if (type == "最近五日") {
                var da = {
                    "Day": "5"
                };
            }
            else {
                var da = {
                    "Day": "1"
                };
            }
            GetBuyData(da);
        });
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });
    });

});
