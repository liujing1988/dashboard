var table = null; //定义全局变量表格
var searchtitle = null; //搜索列名
var searchtext = null;  //搜索内容

var tablemain = null; //主表格
var searchmain = null; //搜索列名
var textmain = null;  //搜索内容
$(document).ready(function ($) {

    GetRevoke($('#getRevoke_Customer span').html().substr(0, 10), $('#getRevoke_Customer span').html().substr(13, 10));
    //时间插件
    $('#getRevoke_Customer span').html(moment().subtract('day', 1).format('YYYY-MM-DD') + ' - ' + moment().format('YYYY-MM-DD'));

    $('#getRevoke_Customer').daterangepicker(
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

                $('#getRevoke_Customer span').html(start.format('YYYY-MM-DD') + ' - ' + end.format('YYYY-MM-DD'));
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
    $("#getRevoke_Customer").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        tablemain.ajax.reload();
        if (table != null) {
            table.ajax.reload();
        }
    });
});

function GetRevokeDetails(custid, begindate, enddate) {
    //重写表格的tfoot，将其变成输入框
    $('#revokeTable tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" data=' + title + '  placeholder="搜索 ' + title + '" />');
    });
    //加载客户交易表的内容，并将其汉化
    $(function () {
        TableTools.DEFAULTS.aButtons = [{
            "sExtends": "csv",
            "sCharSet": "utf8",
            "bBomInc": true
        }, "xls"];
        searchtitle = '客户ID,委托日期';
        searchtext = custid + ',' + begindate;
        table = $('#revokeTable').DataTable({
            "bServerSide": true,  //是否链接服务器端
            "sAjaxSource": "/dashboard/Customer/RevokeDetails",  //json访问，调取数据

            "sDom": 'T<"clear">lfrtip',

            "oTableTools": {
                "sSwfPath": "../../Content/swf/copy_csv_xls_pdf.swf"
            },
            "bFilter": false, //搜索框 

            "bStateSave": true, //缓存 

            //传递条件参数至后台
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "extra_search", "value": $('#getRevoke_Customer span').html() },  //时间参数
                    { "name": "columnIndex", "value": searchtitle }, //搜索列
                    { "name": "searchColumns", "value": searchtext }); //搜索内容
            },
            "bProcessing": true,
            "aoColumns": [  //初始化列名
                            {
                                "sName": "custid", //第一列列名
                                "bSearchable": false, //是否启动搜索
                                "bSortable": false,  //是否启用排序
                                "fnRender": function (oObj) {  //数据填充
                                    return '<a href=\"Details/' +
                                    oObj.aData[0] + '\">View</a>';
                                }
                            },
                            { "sName": "orderdate" },
                            { "sName": "opertime" },
                            { "sName": "stockcode" },
                            { "sName": "orderqty" },
                            { "sName": "bsflag" },
                            { "sName": "cancelflag" }
            ],
            "language": { //汉化
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
            "dom":  //调整控件布局，添加#mytoolbox时间选择控件
                    "<'row'<'col-xs-7'l<'#mytoolbox'>><'col-xs-5'>r>" +
                    "t" +
                    "<'row'<'col-xs-6'i><'col-xs-6'p>>"

        });

        //多条件搜索
        table.columns().every(function () {  //列初始化
            var that = this;
            $('input', this.footer()).on('blur', function () {  //单元格失去焦点时触发该函数
                if (that.search() !== this.value || this.value == '') {
                    var stitle = $(this).attr("data");  //获取列名
                    var stext = $.fn.dataTable.util.escapeRegex($(this).val());  //获取搜索内容
                    var re = new RegExp(stitle);  //判断搜索列名是否有重复的初始化
                    if (searchtitle == null) {
                        searchtitle = '客户ID,委托日期';
                        searchtext = custid + ',' + begindate;
                    }
                    else if (searchtitle.search(re) != -1) {  //判断搜索列名是否重复，如有重复，更新相应的搜索内容
                        var st = searchtitle.split(',');
                        var sx = searchtext.split(',');
                        for (var i = 0; i < st.length; i++) {
                            if (st[i] == stitle) {
                                sx[i] = stext;
                            }
                            if (i == 0) {
                                searchtitle = st[i];
                                searchtext = sx[i];
                            }
                            else {
                                searchtitle += ',' + st[i];
                                searchtext += ',' + sx[i];
                            }
                        }
                    }
                    else {
                        searchtitle += ',' + stitle;
                        searchtext += ',' + stext;
                    }
                    table.ajax.reload();  //重新加载数据
                    that
                        .search(this.value)
                        .draw();
                }
            });
        });
    });//enddatatable
}


function GetRevoke(begindate, enddate) {
    TableTools.DEFAULTS.aButtons = [{
        "sExtends": "csv",
        "sCharSet": "utf8",
        "bBomInc": true
    }, "xls"];
    //重写表格的tfoot，将其变成输入框
    $('#revokeMain tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" data=' + title + '  placeholder="搜索 ' + title + '" />');
    });
    //加载客户交易表的内容，并将其汉化
    $(function () {
        tablemain = $('#revokeMain').DataTable({
            //"bJQueryUI": false, 
            //'bPaginate': false, //是否分页 
            //"bRetrieve": false, //是否允许从新生成表格  
            //"bInfo": false, //显示表格的相关信息 
            //"bDestroy": true, 
            "bFilter": false, //搜索框 
            //"bLengthChange": false, //动态指定分页后每页显示的记录数 
            //"bSort": true, //排序 
            //"bStateSave": true, //缓存 
            ////            "sAjaxSource": "ajaxProduct.action", 


            "bServerSide": true,  //是否链接服务器端
            "sAjaxSource": "/dashboard/Customer/RevokeMain",  //json访问，调取数据

            "sDom": 'T<"clear">lfrtip',

            "oTableTools": {
                "sSwfPath": "../../Content/swf/copy_csv_xls_pdf.swf"
            },

            //传递条件参数至后台
            "fnServerParams": function (aoData) {
                aoData.push({ "name": "extra_search", "value": $('#getRevoke_Customer span').html() },  //时间参数
                    { "name": "columnIndex", "value": searchmain }, //搜索列
                    { "name": "searchColumns", "value": textmain }); //搜索内容
            },
            "bProcessing": true, //当处理大量数据时，显示进度，进度条等 
            "aoColumns": [  //初始化列名
                            {
                                "sName": "custid", //第一列列名
                                "bSearchable": true, //是否启动搜索
                                "bSortable": true,  //是否启用排序
                                "fnRender": function (oObj) {  //数据填充
                                    return '<a href=\"Details/' +
                                    oObj.aData[0] + '\">View</a>';
                                }
                            },
                            { "sName": "orderdate" },
                            { "sName": "percentrevoke" },
                            { "sName": "numrevoke" },
                            { "sName": "numorder" },
                            { "sName": "maxminuteorder" },
                            { "sName": "maxsecondorder" },
                            { "sName": "netbuyamt" }
            ],
            "language": { //汉化
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
            "dom":  //调整控件布局，添加#mytoolbox时间选择控件
                    "<'row'<'col-xs-7'l<'#mytoolbox'>><'col-xs-5'>r>" +
                    "t" +
                    "<'row'<'col-xs-6'i><'col-xs-6'p>>"

        });

        //多条件搜索
        tablemain.columns().every(function () {  //列初始化
            var that = this;
            $('input', this.footer()).on('blur', function () {  //单元格失去焦点时触发该函数
                
                if (that.search() !== this.value || this.value == '') {
                    var stitle = $(this).attr("data");  //获取列名
                    var stext = $.fn.dataTable.util.escapeRegex($(this).val());  //获取搜索内容

                    var re = new RegExp(stitle);  //判断搜索列名是否有重复的初始化
                    if (searchmain == null) {
                        searchmain = stitle;
                        textmain = stext;
                    }
                    else if (searchmain.search(re) != -1) {  //判断搜索列名是否重复，如有重复，更新相应的搜索内容
                        var st = searchmain.split(',');
                        var sx = textmain.split(',');
                        for (var i = 0; i < st.length; i++) {
                            if (st[i] == stitle) {
                                sx[i] = stext;
                            }
                            if (i == 0) {
                                searchmain = st[i];
                                textmain = sx[i];
                            }
                            else {
                                searchmain += ',' + st[i];
                                textmain += ',' + sx[i];
                            }
                        }
                    }
                    else {
                        searchmain += ',' + stitle;
                        textmain += ',' + stext;
                    }
                    tablemain.ajax.reload();  //重新加载数据
                    that
                        .search(this.value)
                        .draw();
                }
            });
        });

        $('#revokeMain tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                tablemain.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
                if (table != null) {
                    searchtitle = '客户ID,委托日期';
                    searchtext = $(this).find("td:eq(0)").text() + ',' + $(this).find("td:eq(1)").text();
                    table.ajax.reload();  //重新加载数据
                }
                else {
                    GetRevokeDetails($(this).find("td:eq(0)").text(), $(this).find("td:eq(1)").text(), $(this).find("td:eq(1)").text());
                }
            }
        });
    });//enddatatable
}
