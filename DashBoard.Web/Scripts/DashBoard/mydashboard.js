
$.ajax(
    {
        url: "/Api/values/GetTradeData", //表示提交给的action 
        type: "GET",   //提交方法 
        //data: $.toJSON(comment),
        datatype: "json",//数据类型
        success: function (result) { //返回的结果自动放在resut里面了
            //月份
            var Month = [];
            //月成交量
            var MonthAccont = [];
            //交易类型
            var stratetp = [];
            //各交易类型对应交易量
            var nustratetp = [];
            //交易类型占比
            var percent = [];
            //各交易类型的总交易量
            var numorder = 0;
            //饼图对应数据
            var pie = new Array;
            //实时更新图对应时间
            var Realtime = [];
            //分钟级交易量
            var MinuteTrade = [];
            //月曲线图数据转换
            if (result[0].Month != null) {
                for (var i = 0; i < result.length; i++) {
                    Month.push(result[i].Month);
                    MonthAccont.push(result[i].MatchAmount);
                }
            }

            //饼图数据转换
            if (result[0].strategytype != null) {
                for (var i = 0; i < result.length; i++) {
                    stratetp.push(result[i].StrategyType);
                    nustratetp.push(result[i].NumStrategyType);
                    numorder += result[i].NumStrategyType;
                }
                for (var j = 0; j < stratetp.length; j++) {
                    percent.push(nustratetp[j] / numorder * 100);
                }
                for (var h = 0; h < stratetp.length; h++) {
                    pie[h] = [stratetp[h].StrategyType, nustratetp[h].NumStrategyType];
                }
            }
            //实时曲线数据转换
            if (result[0].Minute != null) {
                for (var i = 0; i < result.length; i++) {
                    Realtime.push(result[i].Minute);
                    MinuteTrade.push(result[i].TradeAmount);
                }
            }
            
                //添加元素
                $(document).ready(function () {

                    //随机数据动态图
                    Highcharts.setOptions({
                        global: {
                            useUTC: false
                        }
                    });
                    var chart;
                    $('#refresh').highcharts({
                        chart: {
                            type: 'spline',
                            animation: Highcharts.svg, // don't animate in old IE               
                            marginRight: 10,
                            events: {
                                load: function () {

                                    // set up the updating of the chart each second             
                                    var series = this.series[0];
                                    setInterval(function () {
                                        var x = new Date(Realtime[Realtime.length - 1]).getTime();// current time
                                            y = MinuteTrade[MinuteTrade.length - 1];
                                        series.addPoint([x, y], true, true);
                                    }, 1000);
                                }
                            }
                        },
                        title: {
                            text: '交易总量情况'
                        },
                        xAxis: {
                            type: 'datetime',
                            tickPixelInterval: 150
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
                                return '<b>' + this.series.name + '</b><br/>' +
                                Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) + '<br/>' +
                                Highcharts.numberFormat(this.y, 2);
                            }
                        },
                        legend: {
                            enabled: false
                        },
                        exporting: {
                            enabled: false
                        },
                        series: [{
                            name: 'Random data',
                            data: (function () {
                                // generate an array of random data                             
                                var data = [],
                                    i;

                                for (i = 0; i <= Realtime.length; i++) {
                                    data.push({
                                        x: new Date(Realtime[i]).getTime(),
                                        y: MinuteTrade[i]
                                    });
                                }
                                return data;
                            })()
                        }]
                    });

                    //线图
                var strategytype = new Highcharts.Chart({
                    chart: {
                        renderTo: 'line'
                    },
                    title: {
                        text: '月交易量统计',
                        x: -20 //center
                    },
                    xAxis: {
                        name:'月份',
                        categories: Month
                    },
                    legend: {
                        layout: 'vertical',
                        align: 'right',
                        verticalAlign: 'middle',
                        borderWidth: 0
                    },

                    series: [{
                        name:'交易量',
                        data: MonthAccont
                    }]
                });

                //数据导出
                $('#getcsv').click(function () {
                    alert(strategytype.getCSV());
                });

                $("#download").click(function () {
                    Highcharts.post('http://export.hcharts.cn/cvs.php', {
                        csv: strategytype.getCSV()
                    });
                })

                //饼图
                $('#pie').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    title: {
                        text: '模块使用情况统计'
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true
                            },
                            showInLegend: true
                        }
                    },
                    series: [{
                        type: 'pie',
                        name: '使用百分比',
                        data: [pie[0],pie[1],pie[2],pie[3],pie[4]]
                    }]
                });
            });

            //柱状图
            $('#column').highcharts({
                chart: {
                    type: 'column'
                },
                title: {
                    text: '客户增长情况'
                },
                xAxis: {
                    categories: ['Apples', 'Oranges', 'Pears', 'Grapes', 'Bananas']
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '客户增长量'
                    }
                },
                tooltip: {
                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                    shared: true
                },
                plotOptions: {
                    column: {
                        stacking: 'percent'
                    }
                },
                series: [{
                    name: 'John',
                    data: [5, 3, 4, 7, 2],
                    dataLabels: {
                        enabled: true,
                        rotation: 0,
                        color: '#FFFFFF',
                        valign: 'middle',
                        align: 'center',
                        x: 4,
                        y: 10,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif',
                            textShadow: '0 0 3px black'
                        }
                    }
                }, {
                    name: 'Jane',
                    data: [2, 2, 3, 2, 1],
                    dataLabels: {
                        enabled: true,
                        rotation: 0,
                        color: '#FFFFFF',
                        valign: 'middle',
                        align: 'center',
                        x: 4,
                        y: 10,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif',
                            textShadow: '0 0 3px black'
                        }
                    }
                }, {
                    name: 'Joe',
                    data: [3, 4, 4, 2, 5],
                    dataLabels: {
                        enabled: true,
                        rotation: 0,
                        color: '#FFFFFF',
                        valign:'middle',
                        align: 'center',
                        x: 4,
                        y: 10,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif',
                            textShadow: '0 0 3px black'
                        }
                    }
                }]
            });

            //1,获取上面id为cloneTr的tr元素  
            var tr = $("#cloneTr");

            $.each(result, function (index, item) {
                //克隆tr，每次遍历都可以产生新的tr                              
                var clonedTr = tr.clone();
                var _index = index;

                //循环遍历cloneTr的每一个td元素，并赋值  
                clonedTr.children("td").each(function (inner_index) {

                    //根据索引为每一个td赋值  
                    switch (inner_index) {
                        case (0):
                            $(this).html(_index + 1);
                            break;
                        case (1):
                            $(this).html(item.caller);
                            break;
                        case (2):
                            $(this).html(item.imsi);
                            break;
                        case (3):
                            $(this).html(item.imei);
                            break;
                        case (4):
                            $(this).html(item.osid);
                            break;

                    }//end switch                          
                });//end children.each  

                //把克隆好的tr追加原来的tr后面  
                clonedTr.insertAfter(tr);
            });//end $each  
            $("#cloneTr").hide();//隐藏id=clone的tr，因为该tr中的td没有数据，不隐藏起来会在生成的table第一行显示一个空行  
            $("#generatedTable").show();
        }//end success

    });