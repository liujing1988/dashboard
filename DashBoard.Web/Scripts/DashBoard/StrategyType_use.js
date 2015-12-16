$(document).ready(function ($) {
    var myDate = new Date();
    var month = myDate.getMonth() + 1;
    GetType(myDate.getFullYear() + "-" + myDate.getMonth() + "-" + myDate.getDate(),
        myDate.getFullYear() + "-" + month + "-" + myDate.getDate());
    GetOpen(myDate.getFullYear() + "-" + myDate.getMonth() + "-" + myDate.getDate(),
        myDate.getFullYear() + "-" + month + "-" + myDate.getDate());


    //时间插件
    $('#getDate_Type span').html(moment().subtract('months', 1).format('YYYY-MM-DD') + ' - ' + moment().format('YYYY-MM-DD'));

    $('#getDate_Type').daterangepicker(
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
                    '今日': [moment().startOf('day'), moment()],
                    '昨日': [moment().subtract('days', 1).startOf('day'), moment().subtract('days', 1).endOf('day')],
                    '最近7日': [moment().subtract('days', 6), moment()],
                    '最近30日': [moment().subtract('days', 29), moment()],
                    '最近半年': [moment().subtract('month', 5), moment()],
                    '最近一年': [moment().subtract('year', 1), moment()]
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

                $('#getDate_Type span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));

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
    $("#getDate_Type").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        var starmonth = $('#getDate_Type span').html().substr(0, 10);
        var endmonh = $('#getDate_Type span').html().substr(13, 10);
        GetType(starmonth, endmonh);
        GetOpen(starmonth, endmonh);
    });
});

function GetType(begindate, enddate) {
    var da = {
        "begindate": begindate,
        "enddate": enddate
    }
    $.ajax(
        {
            url: "/DashBoard/api/GetCustomer/GetStrategyType", //表示提交给的action 
            type: "post",   //提交方法 
            data: da,
            datatype: "json",//数据类型
            success: function (result) { //返回的结果自动放在resut里面了
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

                var pieArray = new Array();
                if (result == "") {
                    alert("找不到符合条件的数据！");
                }
                else {
                    //饼图数据转换
                    if (result[0].StrategyType != null) {
                        for (var i = 0; i < result.length; i++) {
                            stratetp.push(result[i].StrategyType);
                            nustratetp.push(result[i].NumStrategyType);
                            //numorder += result[i].NumStrategyType;
                        }
                        //for (var j = 0; j < stratetp.length; j++) {
                        //    percent.push(nustratetp[j] / numorder * 100);
                        //}
                        for (var h = 0; h < stratetp.length; h++) {
                            //pie[h] = [stratetp[h], percent[h]];
                            pie[h] = [stratetp[h], nustratetp[h]];
                        }

                        for (var k = 0; k < pie.length; k++)
                            pieArray.push(pie[k]);
                    }
                    //添加元素
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
                            pointFormat: '{series.name}: <b>{point.y}股</b>'
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
                            name: '成交量',
                            data: pieArray
                        }]
                    });
                }
            },//end success
            error: function (err) { throw err }
        });
}

function GetOpen(begindate, enddate) {
    var da = {
        "begindate": begindate,
        "enddate": enddate
    }
    $.ajax(
        {
            url: "/DashBoard/api/GetCustomer/GetStrategyOpen", //表示提交给的action 
            type: "post",   //提交方法 
            data: da,
            datatype: "json",//数据类型
            success: function (result) { //返回的结果自动放在resut里面了
                //交易类型
                var stratetp = [];
                //各交易类型对应开仓次数
                var nustratetp = [];


                var pieArray = new Array();
                if (result == "") {
                    alert("找不到符合条件的数据！");
                }
                else {
                    //数据转换
                    if (result[0].StrategyType != null) {
                        for (var i = 0; i < result.length; i++) {
                            stratetp.push(result[i].StrategyType);
                            nustratetp.push(result[i].NumStrategyType);
                        }
                    }

                    //添加元素
                    //柱状图
                    $('#NumOpen').highcharts({
                        chart: {
                            type: 'column',
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false
                        },
                        title: {
                            text: '模块开仓次数统计'
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.y}次</b>'
                        },
                        xAxis: {
                            categories: stratetp
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '开仓次数'
                            }
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: [{
                            name: '开仓次数',
                            data: nustratetp,
                            colorByPoint: true,
                            dataLabels: {
                                enabled: true,
                                rotation: -90,
                                color: '#FFFFFF',
                                align: 'right',
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
                }
            },//end success
            error: function (err) { throw err }
        });
}