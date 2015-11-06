﻿$(document).ready(function ($) {
    var myDate = new Date();
    var month = myDate.getMonth() + 1;
    var year = myDate.getFullYear() - 1;
    GetCustomer(year + "-" + month, myDate.getFullYear() + "-" + month);
    //时间插件
    $('#getMonth_Customer span').html(moment().subtract('years', 1).format('YYYY-MM') + ' - ' + moment().format('YYYY-MM'));
    $('#getMonth_Customer').daterangepicker(
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
                    '最近半年': [moment().subtract('month', 5), moment()],
                    '最近一年': [moment().subtract('year', 1), moment()]
                },
                opens: 'right', //日期选择框的弹出位置
                buttonClasses: ['btn btn-default'],
                applyClass: 'btn-small btn-primary blue',
                cancelClass: 'btn-small',
                format: 'YYYY-MM', //控件中from和to 显示的日期格式
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

                $('#getMonth_Customer span').html(start.format('YYYY-MM') + ' - ' + end.format('YYYY-MM'));
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
    $("#getMonth_Customer").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        var starmonth = $('#getMonth_Customer span').html().substr(0, 7);
        var endmonh = $('#getMonth_Customer span').html().substr(10, 7);
        GetCustomer(starmonth, endmonh);
    });
});
function GetCustomer(BeginMonth, EndMonth) {
    var da = {
        "beginmonth": BeginMonth,
        "endmonth":EndMonth
    }
    $.ajax(
        {
            url: "/api/GetCustomer/GetCustomerAccount", //表示提交给的action 
            type: "post",   //提交方法 
            data: da,
            datatype: "json",//数据类型
            success: function (result) { //返回的结果自动放在resut里面了
                var AliveMonth = [];
                var AliveCustomerNum = [];
                var CreatCustomerNum = [];
                for (var i = 0; i < result.length; i++)
                {
                    AliveMonth[i] = result[i].AllCustomerMonth;
                    AliveCustomerNum[i] = result[i].AliveCustomer;
                    CreatCustomerNum[i] = result[i].CreatCustomer;
                }
                //柱状图
                $('#column').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: '客户增长情况'
                    },
                    xAxis: {
                        categories: AliveMonth
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: '客户增长量'
                        }
                    },
                    tooltip: {
                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b><br/>',
                        shared: true
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal'
                        }
                    },
                    series: [{
                        name: '活跃用户数',
                        data: AliveCustomerNum,
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
                        },
                        stack:'male'
                    }, {
                        name: '新增用户数',
                        data: CreatCustomerNum,
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
                        },
                        stack: 'fmale'
                    }
                    ]
                });
            },//end success
        });
}
