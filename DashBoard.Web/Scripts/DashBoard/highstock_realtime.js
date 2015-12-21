///获取首页实时曲线
///作者：刘静
///修改日期：2015-11-25


$.ajax(
{
    url: "/dashboard/api/GetTradeDate/GetRealTimeData", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //实时更新图对应时间
        var Realtime = [];
        //分钟级交易量
        var MinuteTrade = [];
        //数据
        var data = [];
        var d;
        var showtime = [];
        var h = [];
        //实时曲线数据转换
        if (result.length > 0) {
            if (result[0].Minute != null) {
                for (var i = 0; i < result.length; i++) {
                    var s = result[i].Day + ' ' + result[i].Minute;
                    h[i] = new Date(Date.parse(result[i].Day.replace(/-/g, "/")));
                    d = new Date(Date.parse(s.replace(/-/g, "/")));
                    Realtime.push(Number(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours() - 8, d.getMinutes())));
                    MinuteTrade.push(result[i].TradeAmount);
                    data.push([Realtime[i], MinuteTrade[i]]);
                }
            }
        }

        var am_startTime = new Date(d);
        am_startTime.setHours(9, 30, 0, 0);
        var am_startTimeUTC = Number(Date.UTC(am_startTime.getFullYear(), am_startTime.getMonth(), am_startTime.getDate(), am_startTime.getHours() - 8, am_startTime.getMinutes()));

        var am_midTime = new Date(d);
        am_midTime.setHours(10, 30, 0, 0);
        var am_midTimeUTC = Number(Date.UTC(am_midTime.getFullYear(), am_midTime.getMonth(), am_midTime.getDate(), am_midTime.getHours() - 8, am_midTime.getMinutes()));

        //股票交易早上最后的时间
        var am_lastTime = new Date(d);
        am_lastTime.setHours(11, 30, 0, 0);
        var am_lastTimeUTC = Number(Date.UTC(am_lastTime.getFullYear(), am_lastTime.getMonth(), am_lastTime.getDate(), am_lastTime.getHours() - 8, am_lastTime.getMinutes()));
        //股票交易下午最后的时间
        var pm_startTime = new Date(d);
        pm_startTime.setHours(13, 1, 0, 0);
        var pm_startTimeUTC = Number(Date.UTC(pm_startTime.getFullYear(), pm_startTime.getMonth(), pm_startTime.getDate(), pm_startTime.getHours() - 8, pm_startTime.getMinutes()));

        var pm_midTime = new Date(d);
        pm_midTime.setHours(14, 0, 0, 0);
        var pm_midTimeUTC = Number(Date.UTC(pm_midTime.getFullYear(), pm_midTime.getMonth(), pm_midTime.getDate(), pm_midTime.getHours() - 8, pm_midTime.getMinutes()));

        var pm_lastTime = new Date(d);
        pm_lastTime.setHours(15, 0, 0, 0);
        var pm_lastTimeUTC = Number(Date.UTC(pm_lastTime.getFullYear(), pm_lastTime.getMonth(), pm_lastTime.getDate(), pm_lastTime.getHours() - 8, pm_lastTime.getMinutes()));
        showtime.push(am_startTimeUTC, am_midTimeUTC, am_lastTimeUTC, pm_midTimeUTC, pm_lastTimeUTC);
        $(function () {
            $(document).ready(function () {  //页面加载
                Highcharts.setOptions({
                    global: {
                        useUTC: false
                    }
                });

                var chart;
                $('#realtimeTrade').highcharts('StockChart', {  //在#realtimeTrade处加载图表
                    chart: {
                        alignTicks: false,
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
                                            var rdata = [];
                                            var rd;
                                            //实时曲线数据转换
                                            if (data.length > 0) {
                                                if (data[0].Minute != null) {
                                                    for (var i = 0; i < data.length; i++) {
                                                        var s = data[i].Day + ' ' + data[i].Minute;
                                                        h[i] = new Date(Date.parse(data[i].Day.replace(/-/g, "/")));
                                                        rd = new Date(Date.parse(s.replace(/-/g, "/")));
                                                        rRealtime.push(Number(Date.UTC(rd.getFullYear(), rd.getMonth(), rd.getDate(), rd.getHours() - 8, rd.getMinutes())));
                                                        rMinuteTrade.push(data[i].TradeAmount);
                                                        rdata.push([Realtime[i], MinuteTrade[i]]);
                                                    }
                                                }
                                                series.setData(rdata);
                                            }
                                        }
                                    });

                                }, 1000 * 60);
                            }
                        }
                    },

                    rangeSelector: {
                        //selected: 1,
                        //buttons: [{
                        //    count: 1,
                        //    type: 'hour',
                        //    text: '1H'
                        //}, {
                        //    count: 3,
                        //    type: 'hour',
                        //    text: '3H'
                        //}, {
                        //    type: 'all',
                        //    text: 'All'
                        //}]
                        enabled: false
                    },
                    yAxis: [{
                         opposite: false,//是否把它显示到另一边（右边）
                        }],
                    title: {
                        text: '成交量'
                    },
                    credits: {
                        enabled: false
                    },
                    xAxis: {
                        showFirstLabel: true,
                        showLastLabel: true,
                        scrollbar: { enabled: true },
                        labels: {
                            // staggerLines:5
                            style: {         //字体样式
                                font: 'normal 10px Verdana, sans-serif'
                            },
                            formatter: function () {
                                var returnTime = Highcharts.dateFormat('%H:%M ', this.value);
                                if (returnTime == "11:30 ") {
                                    return "11:30/13:00";
                                }
                                return returnTime;
                            },
                        },
                        tickPositioner: function () {
                            var positions = [am_startTimeUTC, am_midTimeUTC, am_lastTimeUTC, pm_midTimeUTC, pm_lastTimeUTC];
                            return positions;
                        },
                        gridLineWidth: 1,
                    },
                    series: [{
                        type: 'column',
                        name: '交易量',
                        data: data,
                        dataGrouping: {
                            units: [[
                                'minute', // unit name
                                [1] // allowed multiples
                            ], [
                                'hour',
                                [1, 2, 3, 4, 6]
                            ]]
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
