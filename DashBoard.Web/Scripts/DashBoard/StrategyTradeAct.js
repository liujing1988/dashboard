$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetStrategyTradeAct", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var StrategyName = [];
        //分钟级交易量
        var TradeNum = [];
        //单位
        var unit = "";
        if (result.length > 0) {
            //获取服务器日期
            var Date = result[0].TradeDate;
            //获取当日第五名交易数
            var minresult = result[0].NumStrategyType;
            for (var i = 0; i < result.length; i++) {
                if (minresult > result[i].NumStrategyType) {
                    minresult = Number(result[i].NumStrategyType);
                }
            }
            //实时曲线数据转换
            if (result[0].StrategyType != null && minresult / 10000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    StrategyName.push(result[i].StrategyType);
                    TradeNum.push(Number(result[i].NumStrategyType) / 10000);
                }
                unit = "万人次";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    StrategyName.push(result[i].StrategyType);
                    TradeNum.push(Number(result[i].NumStrategyType));
                }
                unit = "人次";
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
                $('#StrategyTradeAct').highcharts({
                    chart: {
                        high: '400px',
                        type: 'bar',
                        animation: Highcharts.svg, // don't animate in old IE               
                        marginRight: 10
                    },
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series = this.series[0];
                                var xAxis = this.xAxis[0];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetTradeDate/GetStrategyTradeAct",
                                        dataType: "json",
                                        success: function (data) {
                                            //实时更新图对应时间
                                            var rStrategyType = [];
                                            //分钟级交易量
                                            var rTradeNum = [];
                                            if (data.length > 0) {
                                                //实时曲线数据转换
                                                if (data[0].StrategyType != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        rStrategyType[i] = data[i].StrategyType;
                                                        rTradeNum[i] = data[i].NumStrategyType;
                                                    }
                                                }
                                                xAxis.setCategories(rStrategyType);
                                                series.setData(rTradeNum);
                                            }
                                        }
                                    });

                                }, 60000);
                            
                        }
                    },
                    title: {
                        text: '本月功能热度排名'
                    },
                    xAxis: {
                        categories: StrategyName
                    },
                    yAxis: {
                        title: {
                            text: '访问人次'
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
                    credits: {
                        enabled: false
                    },
                    exporting: {
                        enabled: false
                    },
                    series: [{
                        name: '访问人次',
                        data: TradeNum,
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            align: 'right',
                            style: {
                                fontSize: '13px',
                                textShadow: '0 0 0 black'
                            },
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 0, '.') + unit
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
