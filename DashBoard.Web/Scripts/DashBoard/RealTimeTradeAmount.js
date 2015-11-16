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
        if (result[0].Minute != null) {
            for (var i = 0; i < result.length; i++) {
                Realtime.push(result[i].Minute);
                MinuteTrade.push(result[i].TradeAmount);
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
        $('#realtimeTrade').highcharts({
            chart: {
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE               
                marginRight: 10,
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
                                    if (data[0].Minute != null) {
                                        for (var i = 0; i < data.length; i++) {
                                            rRealtime[i] = data[i].Minute;
                                            rMinuteTrade[i] = data[i].TradeAmount;
                                        }
                                    }
                                    if (data[data.length-1].Minute > result[result.length-1].Minute) {
                                        
                                        for (j = 0 ; j < data.length ; j++) {
                                            if (data[j].Minute > result[result.length - 1].Minute) {
                                                var dt = rRealtime[j];
                                                dt = dt.replace(/-/g, "/");
                                                var x = new Date(dt).getTime(), // current time         
                                                        y = rMinuteTrade[j];
                                                series.addPoint([x, y], true, true);
                                            }
                                        }
                                        result = data;
                                    }
                                }
                            });
                                        
                        }, 60000);
                    }
                }
            },
            title: {
                text: '实时交易量'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150
            },
            scrollbar: {
                enabled: true
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
                    Highcharts.dateFormat('%Y-%m-%d %H:%M', this.x) + '<br/>' +
                    Highcharts.numberFormat(this.y, 2);
                },
                crosshairs: true,
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: '交易量',
                data: (function () {
                    // generate an array of random data 
                    var data = [],
                        i;
                    for (i = 0; i < Realtime.length - 1; i++) {
                        var dt = Realtime[i];
                        dt = dt.replace(/-/g, "/");
                        data.push({
                            x: new Date(dt).getTime(),
                            y: MinuteTrade[i]
                        });
                    }
                    return data;
                })()
            }]
        });
    });

});
    }
});
