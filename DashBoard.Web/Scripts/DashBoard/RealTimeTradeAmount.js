///获取首页实时曲线
///作者：刘静
///修改日期：2015-11-25
$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetRealTimeData", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //实时更新图对应时间
        var Realtime = [];
        //分钟级交易量
        var MinuteTrade = [];
        //实时曲线数据转换
        if (result.length > 0) {
            if (result[0].Minute != null) {
                for (var i = 0; i < result.length; i++) {
                    if (result[i].Minute == "13:00")
                    {
                        Realtime.push("11:30/13:00");
                    }
                    else
                    {
                        Realtime.push(result[i].Minute);
                    }
                    MinuteTrade.push(result[i].TradeAmount);
                }
            }
        }
$(function () {
    $(document).ready(function () {  //页面加载
        Highcharts.setOptions({
            global: {
                useUTC: false
            }
        });

        var chart;
        $('#realtimeTrade').highcharts({  //在#realtimeTrade处加载图表
            chart: {
                type: 'column',  //图表类型为柱状图
                animation: Highcharts.svg, // don't animate in old IE               
                marginRight: 10
            },
                events: {
                    load: function () {

                        // set up the updating of the chart each second             
                        var series = this.series[0];
                        setInterval(function () {
                            $.ajax({
                                type: "post",
                                url: "/DashBoard/api/GetTradeDate/GetRealTimeData",
                                dataType: "json",
                                success: function (data) {
                                    //实时更新图对应时间
                                    var rRealtime = [];
                                    //分钟级交易量
                                    var rMinuteTrade = [];
                                    //实时曲线数据转换
                                    if (data.length > 0) {
                                        if (data[0].Minute != null) {
                                            for (var i = 0; i < data.length; i++) {
                                                rMinuteTrade[i] = data[i].TradeAmount;
                                            }
                                        }
                                        series.setData(rMinuteTrade);
                                    }
                                }
                            });
                                        
                        }, 1000 * 60);
                    }
                
            },
            title: {
                text: '实时交易量'
            },
            xAxis: {
                tickInterval: 120,
                categories: Realtime
            },
            scrollbar: {
                enabled: true
            },
            credits: {
                enabled: false
            },
            yAxis: {
                title: {
                    text: '交易量'
                },
                plotLines: [{
                    value: 3,
                    width: 2,
                    color: '#808080'
                }]
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.series.name + '</b><br/>' +
                    this.x + '<br/>' +
                    Highcharts.numberFormat(this.y, 2);
                },
                crosshairs: [{
                    width: 1,
                    color: 'red'
                }, {
                    width: 1,
                    color: 'red'
                }]
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: '交易量',
                data: MinuteTrade
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
