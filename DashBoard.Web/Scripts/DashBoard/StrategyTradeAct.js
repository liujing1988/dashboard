$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetStrategyTradeAct", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //用户ID
        var ModuleName = [];
        //访问次数
        var VisitNum = [];
        //交易金额
        var TradeAmt = [];

        //单位
        var unit = "";
        if (result.length > 0) {
            //获取服务器日期
            var Date = result[0].TradeDate;
            //获取本月第五名访问次数
            var minresult = result[0].NumModules;
            for (var i = 0; i < result.length; i++) {
                if (minresult > result[i].NumModules) {
                    minresult = Number(result[i].NumModules);
                }
            }
            //实时曲线数据转换
            if (result[0].ModuleName != null && minresult / 10000 >= 1) {
                for (var i = 0; i < result.length; i++) {
                    ModuleName.push(result[i].ModuleName);
                    VisitNum.push(result[i].ModuleName + "<br/>(" + Number(result[i].NumModules) / 10000 + "万人次)");
                    TradeAmt.push(result[i].ModuleTradeAmt / 10000);
                }
                unit = "万人次";
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    ModuleName.push(result[i].ModuleName);
                    VisitNum.push(result[i].ModuleName + "<br/>(" + Number(result[i].NumModules) +"人次)");
                    TradeAmt.push(result[i].ModuleTradeAmt / 10000);
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
                                            var rModuleName = [];
                                            //分钟级交易量
                                            var rVisitNum = [];
                                            if (data.length > 0) {
                                                //实时曲线数据转换
                                                if (data[0].ModuleName != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        rModuleName[i] = data[i].ModuleName;
                                                        rVisitNum[i] = data[i].NumModules;
                                                    }
                                                }
                                                xAxis.setCategories(rModuleName);
                                                series.setData(rVisitNum);
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
                        allowDecimals: false, //不允许刻度有小数
                        categories:VisitNum
                        //categories: ModuleName
                    },
                    yAxis: {
                        title: {
                            text: '交易金额(万元)'
                        },
                        plotLines: [{
                            value: 0,
                            width: 1,
                            color: '#808080'
                        }]
                    },
                    tooltip: {
                        formatter: function () {
                            return '访问人次 ' + this.x + '<br>' +
                               this.series.name + ': ' + Highcharts.numberFormat(this.y, 2, '.') + "万元";
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
                        name: '交易金额(万元)',
                        data: TradeAmt,
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            align: 'right',
                            style: {
                                fontSize: '13px',
                                textShadow: '0 0 0 black'
                            },
                            formatter: function () {
                                return Highcharts.numberFormat(this.y, 2, '.') + "万元"
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
