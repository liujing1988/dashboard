
var table;
$(document).ready(function ($) {
    $(function () {
        table = $('#myDataTable').DataTable({
            "bServerSide": true,
            "sAjaxSource": "Home/AjaxHandler",
            
            "fnServerParams": function ( aoData ) {
                aoData.push({ "name": "extra_search", "value": $('#reportrange span').html() });
            },
            "bProcessing": true,
            "aoColumns": [
                            {
                                "sName": "客户ID",
                                "bSearchable": true,
                                "bSortable": true,
                                "fnRender": function (oObj) {
                                    return '<a href=\"Details/' +
                                    oObj.aData[0] + '\">View</a>';
                                }
                            },
                            { "sName": "交易日期" },
                            { "sName": "股票代码" },
                            { "sName": "交易量" },
                            { "sName": "交易价格" },
                            { "sName": "交易方向" }
            ],
            "language": {
                "sProcessing": "处理中...",
                "sLengthMenu": "显示 _MENU_ 项结果",
                "sZeroRecords": "没有匹配结果",
                "sInfo": "显示第 _START_ 至 _END_ 项结果，共 _TOTAL_ 项",
                "sInfoEmpty": "显示第 0 至 0 项结果，共 0 项",
                "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
                "sInfoPostFix": "",
                "sSearch": "搜索:",
                "sUrl": "",
                "sEmptyTable": "表中数据为空",
                "sLoadingRecords": "载入中...",
                "sInfoThousands": ",",
                "oPaginate": {
                    "sFirst": "首页",
                    "sPrevious": "上页",
                    "sNext": "下页",
                    "sLast": "末页"
                },
                "oAria": {
                    "sSortAscending": ": 以升序排列此列",
                    "sSortDescending": ": 以降序排列此列"
                }
            },
            "dom":
                    "<'row'<'col-xs-7'l<'#mytoolbox'>><'col-xs-5'f>r>" +
                    "t" +
                    "<'row'<'col-xs-6'i><'col-xs-6'p>>",
            initComplete: initComplete

        });
    });
})

/**
    * 表格加载渲染完毕后执行的方法
    * @param data
    */
function initComplete(data) {

    var dataPlugin =
            '<div id="reportrange" class="pull-left dateRange" style="margin-left: 10px"> ' +
            '日期：<i class="glyphicon glyphicon-calendar fa fa-calendar"></i> ' +
            '<span id="searchDateRange"></span>  ' +
            '<b class="caret"></b></div> ';
    $('#mytoolbox').append(dataPlugin);
    //时间插件
    $('#reportrange span').html(moment().subtract('months', 1).format('YYYY-MM-DD HH:mm:ss') + ' - ' + moment().format('YYYY-MM-DD HH:mm:ss'));
    
    $('#reportrange').daterangepicker(
            {
                
                //startDate: moment().startOf('day'),
                //endDate: moment(),
                //minDate: '01/01/2012',    //最小时间
                maxDate: moment(), //最大时间
                dateLimit: {
                    days: 365
                }, //起止时间的最大间隔
                showDropdowns: true,
                showWeekNumbers: false, //是否显示第几周
                timePicker: true, //是否显示小时和分钟
                timePickerIncrement: 60, //时间的增量，单位为分钟
                timePicker12Hour: false, //是否使用12小时制来显示时间
                ranges: {
                    '最近1小时': [moment().subtract('hours',1), moment()],
                    '今日': [moment().startOf('day'), moment()],
                    '昨日': [moment().subtract('days', 1).startOf('day'), moment().subtract('days', 1).endOf('day')],
                    '最近7日': [moment().subtract('days', 6), moment()],
                    '最近30日': [moment().subtract('days', 29), moment()]
                },
                opens: 'right', //日期选择框的弹出位置
                buttonClasses: ['btn btn-default'],
                applyClass: 'btn-small btn-primary blue',
                cancelClass: 'btn-small',
                format: 'YYYY-MM-DD HH:mm:ss', //控件中from和to 显示的日期格式
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

                $('#reportrange span').html(start.format('YYYY-MM-DD HH:mm:ss') + ' - ' + end.format('YYYY-MM-DD HH:mm:ss'));
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
    $("#reportrange").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        table.ajax.reload();
        //获取dt请求参数
        var args = table.ajax.params();
        //alert(args.extra_search);
        //console.log("额外传到后台的参数值extra_search为：" + args.extra_search);
    });
}