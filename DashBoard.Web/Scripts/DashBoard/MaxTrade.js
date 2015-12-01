$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {
        //当日委托笔数
        var NumOrder;
        if (result.NumOrder > 0) {
            NumOrder = result.NumOrder;
        }
        else {
            NumOrder = 0;
        }

        //日委托笔数阈值
        ThDayNumOrder = result.ThDayNumOrder;

        //分钟委托笔数
        var MiNumOrder;
        if (result.MiNumOrder > 0) {
            MiNumOrder = result.MiNumOrder;
        }
        else {
            MiNumOrder = 0;
        }

        //分委托笔数阈值
        ThMiNumOrder = result.ThMiNumOrder;

        //秒委托笔数
        var SeNumOrder;
        if (result.SeNumOrder > 0) {
            SeNumOrder = result.SeNumOrder;
        }
        else {
            SeNumOrder = 0;
        }

        //秒委托笔数阈值
        ThSeNumOrder = result.ThSeNumOrder;

        //当日最大委托笔数
        var MaxNumOrder = NumOrder;

        //成交笔数
        var NumVolum;
        if (result.NumVolum > 0) {
            NumVolum = result.NumVolum;
        }
        else {
            NumVolum = 0;
        }
        //撤单笔数
        var NumRevoke;
        if (result.NumRevoke > 0) {
            NumRevoke = result.NumRevoke;
        }
        else {
            NumRevoke = 0;
        }
        
        var PR;
        if(NumOrder > 0){
            PR = (NumRevoke / NumOrder * 100);
        }
        else {
            PR = 0;
        }

        //添加图表
        $('#PRevoke').highcharts({
            chart: {
                type: 'column',
                events: {
                    load: function () {

                        // set up the updating of the chart each second             
                        var series = this.series[0];
                        var series1 = this.series[1];
                        var series2 = this.series[2];
                        var series3 = this.series[3];
                        setInterval(function () {
                            $.ajax({
                                type: "post",
                                url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                dataType: "json",
                                success: function (data) {
                                    //委托笔数
                                    var rNumOrder;
                                    if (data.NumOrder > 0) {
                                        rNumOrder = data.NumOrder;
                                    }
                                    else {
                                        rNumOrder = 0;
                                    }

                                    //最大委托笔数
                                    var rMaxNumOrder = NumOrder;

                                    //成交笔数
                                    var rNumVolum;
                                    if (data.NumVolum > 0) {
                                        rNumVolum = data.NumVolum;
                                    }
                                    else {
                                        rNumVolum = 0;
                                    }
                                    //撤单笔数
                                    var rNumRevoke;
                                    if (data.NumRevoke > 0) {
                                        rNumRevoke = data.NumRevoke;
                                    }
                                    else {
                                        rNumRevoke = 0;
                                    }
                                    var rPR;
                                    if(rNumOrder > 0){
                                        rPR = rNumRevoke / rNumOrder * 100;
                                    }
                                    else {
                                        rPR = 0;
                                    }
                                    series.update(100);
                                    series1.update(rPR);
                                    series2.update(rNumOrder);
                                    series3.update(rNumRevoke);
                                }
                            });

                        }, 1000 * 60);
                    }
                }
            },
            title: {
                useHTML: true,
                text: '撤单/委托比<br><font size= 5 color = green>' + (NumRevoke / NumOrder * 100).toFixed(2) + '%</font>',
                
            },
            xAxis: {
                categories: ['撤单/委托']
            },
            yAxis: [{
                min : 0,
                title: {
                    text: '比值'
                    }
                },{ title:{
                    text: '笔数'
                    },
                opposite:true
            }],
            plotOptions: {
                column: {
                    grouping: false,
                    shadow: false,
                    borderWidth: 0
                }
            },
            legend: {
                shadow: false
            },
            tooltip: {
                shared: true
            },
            series: [{
                name: '委托占比（100%）',
                color: 'rgba(165,170,217,1)',
                data: [100],
                tooltip: {
                    valueSuffix: ' %'
                },
                pointPadding: 0.3,
                pointPlacement: -0.2
            }, {
                name: '撤单占比',
                color: 'rgba(126,86,134,.9)',
                data: [PR],
                tooltip: {
                    valueSuffix: ' %'
                },
                pointPadding: 0.4,
                pointPlacement: -0.2
            }, {
                name: '委托笔数',
                color: 'rgba(248,161,63,1)',
                data: [NumOrder],
                pointPadding: 0.3,
                pointPlacement: 0.2,
                yAxis: 1
            }, {
                name: '撤单笔数',
                color: 'rgba(186,60,61,.9)',
                data: [NumRevoke],
                pointPadding: 0.4,
                pointPlacement: 0.2,
                yAxis: 1
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


        //单位时间最大委托数
        var gaugeOptions = {

            chart: {
                type: 'solidgauge'
            },
            title: null,
            pane: {
                center: ['50%', '85%'],
                size: '140%',
                startAngle: -90,
                endAngle: 90,
                background: {
                    backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || '#EEE',
                    innerRadius: '60%',
                    outerRadius: '100%',
                    shape: 'arc'
                }
            },
            tooltip: {
                enabled: false
            },

            // the value axis
            yAxis: {
                stops: [
                    [0.1, '#55BF3B'], // green
                    [0.5, '#DDDF0D'], // yellow
                    [0.9, '#DF5353'] // red
                ],
                lineWidth: 0,
                minorTickInterval: null,
                tickPixelInterval: 400,
                tickWidth: 0,
                title: {
                    y: -70
                },
                labels: {
                    y: 16
                }
            },
            plotOptions: {
                solidgauge: {
                    dataLabels: {
                        y: 5,
                        borderWidth: 0,
                        useHTML: true
                    }
                }
            }
        };

        $('#MaxDayOrder').highcharts(Highcharts.merge(gaugeOptions, {
            chart: {
                events: {
                    load: function () {

                        // set up the updating of the chart each second             
                        var points = this.series[0].points[0];
                        setInterval(function () {
                            $.ajax({
                                type: "post",
                                url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                dataType: "json",
                                success: function (data) {
                                    //委托笔数
                                    var rNumOrder;
                                    if (data.NumOrder > 0) {
                                        rNumOrder = data.NumOrder;
                                    }
                                    else {
                                        rNumOrder = 0;
                                    }

                                    //最大委托笔数
                                    var rMaxNumOrder = NumOrder;
                                    points.update(rMaxNumOrder);
                                }
                            });

                        }, 1000 * 60);
                    }
                }
            },
            title: {
                text: '今日委托笔数'
            },
            yAxis: {
                min: 0,
                max: ThDayNumOrder,
                title: {
                    text: null
                }
            },
            series: [{
                name: '委托数',
                data: [MaxNumOrder],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}</span><br/>' +
                           '<span style="font-size:12px;color:silver">笔</span></div>'
                },
                tooltip: {
                    valueSuffix: ' 笔'
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
            
        }));

        $('#MaxMiOrder').highcharts(Highcharts.merge(gaugeOptions, {
            chart: {
                events: {
                    load: function () {

                        // set up the updating of the chart each second             
                        var points = this.series[0].points[0];
                        setInterval(function () {
                            $.ajax({
                                type: "post",
                                url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                dataType: "json",
                                success: function (data) {
                                    //委托笔数
                                    var rMiNumOrder;
                                    if (data.MiNumOrder > 0) {
                                        rMiNumOrder = data.MiNumOrder;
                                    }
                                    else {
                                        rMiNumOrder = 0;
                                    }

                                    //最大委托笔数
                                    points.update(rMiNumOrder);
                                }
                            });

                        }, 1000 * 60);
                    }
                }
            },
            title: {
                text: '今日每分钟最大委托笔数'
            },
            yAxis: {
                min: 0,
                max: ThMiNumOrder,
                title: {
                    text: null
                }
            },
            series: [{
                name: '委托数',
                data: [MiNumOrder],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}</span><br/>' +
                           '<span style="font-size:12px;color:silver">笔</span></div>'
                },
                tooltip: {
                    valueSuffix: ' 笔'
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
        }));

        $('#MaxSeOrder').highcharts(Highcharts.merge(gaugeOptions, {
            chart: {
                events: {
                    load: function () {

                        // set up the updating of the chart each second             
                        var points = this.series[0].points[0];
                        setInterval(function () {
                            $.ajax({
                                type: "post",
                                url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                dataType: "json",
                                success: function (data) {
                                    //委托笔数
                                    var rSeNumOrder;
                                    if (data.SeNumOrder > 0) {
                                        rSeNumOrder = data.SeNumOrder;
                                    }
                                    else {
                                        rSeNumOrder = 0;
                                    }

                                    //最大委托笔数

                                    points.update(rSeNumOrder);
                                }
                            });

                        }, 1000);
                    }
                }
            },
            title: {
                text: '今日每秒最大委托笔数'
            },
            yAxis: {
                min: 0,
                max: ThSeNumOrder,
                title: {
                    text: null
                }
            },
            series: [{
                name: '委托数',
                data: [SeNumOrder],
                dataLabels: {
                    format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                        ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}</span><br/>' +
                           '<span style="font-size:12px;color:silver">笔</span></div>'
                },
                tooltip: {
                    valueSuffix: ' 笔'
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
        }));
    }

});

