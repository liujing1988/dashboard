$(document).ready(function ($) {

    GetStrategy($('#getMonth_Strategy span').html().substr(0, 7), $('#getMonth_Strategy span').html().substr(10, 7));

    //时间插件
    $('#getMonth_Strategy span').html(moment().subtract('years', 1).format('YYYY-MM') + ' - ' + moment().format('YYYY-MM'));

    $('#getMonth_Strategy').daterangepicker(
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

                $('#getMonth_Strategy span').html(start.format('YYYY-MM') + ' - ' + end.format('YYYY-MM'));
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
    $("#getMonth_Strategy").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        var starmonth = $('#getMonth_Strategy span').html().substr(0, 7);
        var endmonh = $('#getMonth_Strategy span').html().substr(10, 7);
        GetStrategy(starmonth, endmonh);
    });
});
function GetStrategy(begindate, enddate) {
    if (begindate == "" && enddate == "") {
        var myDate = new Date();
        var year = myDate.getFullYear() - 1;
        var month = myDate.getMonth() + 1;
        begindate = year + "-" + month;
        enddate = myDate.getFullYear() + "-" + month;
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
          url: "/dashboard/api/GetTradeDate/GetCustomerCreatedStrategy", //表示提交给的action 
          type: "post",   //提交方法 
          data: da,
          datatype: "json",//数据类型
          success: function (result) { //返回的结果自动放在resut里面了
              //月份
              var Month = [];
              //自动上传交易量
              var AutoQty = [];
              //手动上载交易量
              var ManualQty = [];
              //月曲线图数据转换
              if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    Month.push(result[i].Month);
                    AutoQty.push(result[i].AutoQty);
                    ManualQty.push(result[i].ManualQty);
                  }
              }
              //添加元素
              $(document).ready(function () {
                  //线图
                  var strategytype = new Highcharts.Chart({
                      chart: {
                          renderTo: 'CustomerCreateStrategy',
                          type: 'column'
                      },
                      title: {
                          text: '用户自建策略交易量统计'
                      },
                      xAxis: {
                          name: '月份',
                          categories: Month,
                          labels: {
                              align: 'center',
                              style: {
                                  fontSize: '13px',
                                  fontFamily: 'Verdana, sans-serif'
                              }
                          }
                      },
                      yAxis: {
                          min: 0,
                          title: {
                              text: '交易量'
                          },
                      },
                      tooltip: {
                          formatter: function () {
                              return '' + this.series.name + '' +
                                 this.x + ': ' + this.y + '股';
                          },
                          //pointFormat: '{series.name}: <b>{point.y:.1f} millions</b>',
                      },
                      plotOptions: {
                          column: {
                              stacking: 'normal',
                              dataLabels: {
                                  enabled: true,
                                  color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                              }
                          }
                      },
                          column: {
                              borderWidth: 0,
                              dataLabels: {
                                  enabled: true,
                                  format: '{point.y:.0f}股'
                              }
                      },
                      credits: {
                          enabled: false
                      },
                      legend: {
                          layout: 'vertical',
                          align: 'right',
                          verticalAlign: 'middle',
                          borderWidth: 0
                      },

                      series: [{
                          name: '自动上传',
                          data: AutoQty
                      },
                      {
                          name:'手动上载',
                          data:ManualQty
                      }],
                      lang: {
                          noData: "无数据!"
                      },
                      noData: {
                          style: {
                              fontWeight: 'bold',
                              fontSize: '15px',
                              color: '#303030'
                          }
                      }
                  });

              })
          }//end success
      });
    CustomerCreateStrategyTopMatchQty(begindate, enddate);
}

//用户自建策略总交易量Top5
function CustomerCreateStrategyTopMatchQty(begindate, enddate) {
    var da = {
        "beginDate": begindate,
        "endDate": enddate
    }
    $.ajax({
        url: '/dashboard/api/GetTradeDate/GetCustomerCreateStrategyTopMatchQty',
        data: da,
        type: 'post',
        success: function (result) {
            //1,获取上面id为cloneTr的tr元素  
            var tr = $("#cloneTr");
            $("#CustomerCreateStrategyTopMatchQty tr:gt(0):not(:eq(0))").remove();
            var data = [];
            for (var i = result.length -1 ; i >= 0; i--)
            {
                data.push(result[i]);
            }
            $.each(data, function (index, item) {
                //克隆tr，每次遍历都可以产生新的tr                              
                var clonedTr = tr.clone().attr('id', 'clone');
                var _index = index;
                //循环遍历cloneTr的每一个td元素，并赋值  
                clonedTr.children("td").each(function (inner_index) {

                    //根据索引为每一个td赋值  
                    switch (inner_index) {
                        case (0):
                            $(this).html(item.CustId);
                            break;
                        case (1):
                            $(this).html(item.MatchQty);
                            break;
                    }//end switch  
                });//end children.each  
                
                //把克隆好的tr追加原来的tr后面  
                clonedTr.insertAfter(tr);
            });//end $each 
            $("#cloneTr").hide();//隐藏id=clone的tr，因为该tr中的td没有数据，不隐藏起来会在生成的table第一行显示一个空行
            $("#CustomerCreateStrategyTopMatchQty").show();
            $("tr#clone").show();
        }
    })
}