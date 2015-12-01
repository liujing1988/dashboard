$(document).ready(function ($) {

    GetRealTime($('#getReal_Trade span').html().substr(0, 10), $('#getReal_Trade span').html().substr(13, 10));
    //时间插件
    $('#getReal_Trade span').html(moment().subtract('day', 1).format('YYYY-MM-DD') + ' - ' + moment().format('YYYY-MM-DD'));

    $('#getReal_Trade').daterangepicker(
            {

                //startDate: moment().startOf('day'),
                //endDate: moment(),
                //minDate: '01/01/2012',    //最小时间
                maxDate: moment(), //最大时间
                //dateLimit: {
                //    days: 365
                //}, //起止时间的最大间隔
                showDropdowns: true,
                showWeekNumbers: false, //是否显示第几周
                timePicker: true, //是否显示小时和分钟
                timePickerIncrement: 60, //时间的增量，单位为分钟
                timePicker12Hour: false, //是否使用12小时制来显示时间
                ranges: {
                    '今日': [moment().subtract('day', 0), moment()],
                    '昨日': [moment().subtract('day', 1), moment()],
                    '最近7日': [moment().subtract('day', 7), moment()],
                    '最近一个月': [moment().subtract('month', 1), moment()]
                },
                opens: 'right', //日期选择框的弹出位置
                buttonClasses: ['btn btn-default'],
                applyClass: 'btn-small btn-primary blue',
                cancelClass: 'btn-small',
                format: 'YYYY-MM-DD', //控件中from和to 显示的日期格式
                separator: ' to ',
                locale: {
                    applyLabel: '确定',
                    cancelLabel: '取消',
                    fromLabel: '起始时间',
                    toLabel: '结束时间',
                    customRangeLabel: '自定义',
                    daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                    monthNames: ['一月', '二月', '三月', '四月', '五月', '六月',
                        '七月', '八月', '九月', '十月', '十一月', '十二月'],
                    firstDay: 1
                }
            }, function (start, end, label) {//格式化日期显示框

                $('#getReal_Trade span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
            });

    //设置日期菜单被选项  --开始--
    var dateOption;
    if ("${riqi}" == 'day') {
        dateOption = "今日";
    } else if ("${riqi}" == 'yday') {
        dateOption = "昨日";
    } else if ("${riqi}" == 'week') {
        dateOption = "最近7日";
    } else if ("${riqi}" == 'month') {
        dateOption = "最近30日";
    } else if ("${riqi}" == 'year') {
        dateOption = "最近一年";
    } else {
        dateOption = "自定义";
    }
    $(".daterangepicker").find("li").each(function () {
        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
        }
        if (dateOption == $(this).html()) {
            $(this).addClass("active");
        }
    });
    //设置日期菜单被选项  --结束--


    //选择时间后触发重新加载的方法
    $("#getReal_Trade").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        var starmonth = $('#getReal_Trade span').html().substr(0, 10);
        var endmonh = $('#getReal_Trade span').html().substr(13, 10);
        GetRealTime(starmonth, endmonh);
    });
});
function GetRealTime(begindate, enddate) {
    if (begindate == "" && enddate == "") {
        var myDate = new Date();
        var month = myDate.getMonth() + 1;
        var date = myDate.getDate() - 1;
        begindate = myDate.getFullYear() + "-" + month + "-" + myDate.getDate();
        enddate = myDate.getFullYear() + "-" + month +"-" + myDate.getDate();
    }
    if (begindate == "") {
        alert("请输入起始时间");
    }
    else if (enddate == "") {
        alert("请输入截止时间");
    }

    var da = {
        "beginDate": begindate,
        "endDate": enddate
    }
    $.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetRealData", //表示提交给的action 
    type: "post",   //提交方法 
    data:da,
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
                    if (result[i].Minute == "13:00") {
                        Realtime.push(result[i].Day + ' ' + "11:30/13:00");
                    }
                    Realtime.push(result[i].Day + ' '+ result[i].Minute);
                    MinuteTrade.push(result[i].TradeAmount);
                }
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
                $('#tradeData').highcharts({
                    chart: {
                        type: 'column',
                        animation: Highcharts.svg, // don't animate in old IE               
                        marginRight: 10,
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series = this.series[0];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetTradeDate/GetRealData",
                                        data:da,
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
                                                rMinuteTrade[i] = data[i].TradeAmount;
                                            }
                                        }
                                    });

                                }, 1000 * 60);
                            }
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

}

