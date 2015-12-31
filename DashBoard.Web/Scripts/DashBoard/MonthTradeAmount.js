$(document).ready(function ($) {

    GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());

    $("#unit").change(function () {
        GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());
    });
    $("#strategyname").on('blur', function () {
        GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());
    });
    $("#stratkindname").on('blur', function () {
        GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());
    });
    $("#seriesno").on('blur', function () {
        GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());
    });
    //时间插件
    $('#getMonth_Trade span').html(moment().subtract('years', 1).format('YYYY-MM') + ' - ' + moment().format('YYYY-MM'));

    $('#getMonth_Trade').daterangepicker(
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

                $('#getMonth_Trade span').html(start.format('YYYY-MM') + ' - ' + end.format('YYYY-MM'));
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
    $("#getMonth_Trade").on('apply.daterangepicker', function () {
        //当选择时间后，触发dt的重新加载数据的方法
        var starmonth = $('#getMonth_Trade span').html().substr(0, 7);
        var endmonh = $('#getMonth_Trade span').html().substr(10, 7);
        GetMonth(starmonth, endmonh, $("#unit").val(), $("#strategyname").val(), $("#stratkindname").val(), $("#seriesno").val());
    });
});
function GetMonth(begindate, enddate, munit, strategyname, stratkindname, seriesno) {
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
        "endDate": enddate,
        "strategyName": strategyname,
        "stratInfo": stratkindname,
        "seriesNo": seriesno
    }

    $.ajax(
      {
          url: "/dashboard/api/GetTradeDate/GetMatchAmount", //表示提交给的action 
          type: "post",   //提交方法 
          data: da,
          datatype: "json",//数据类型
          success: function (result) { //返回的结果自动放在resut里面了
              //月份
              var Month = [];
              //月成交量
              var MonthAccont = [];
              //月曲线图数据转换
              var unit = "";
              if (result.length > 0) {
                  if (munit == "003") {
                      for (var i = 0; i < result.length; i++) {
                          Month.push(result[i].Month);
                          MonthAccont.push(result[i].MatchAmount / 100000000);
                          unit = "亿元";
                      }
                  }
                  else if (munit == "002") {
                      for (var i = 0; i < result.length; i++) {
                          Month.push(result[i].Month);
                          MonthAccont.push(result[i].MatchAmount / 10000);
                          unit = "万元";
                      }
                  }
                  else {
                      for (var i = 0; i < result.length; i++) {
                          Month.push(result[i].Month);
                          MonthAccont.push(result[i].MatchAmount);
                          unit = "元";
                      }
                  }
              }
              //添加元素
              $(document).ready(function () {
                  //线图
                  var strategytype = new Highcharts.Chart({
                      chart: {
                          renderTo: 'MonthTrade',
                          type: 'column'
                      },
                      title: {
                          text: '月交易金额统计'
                      },
                      xAxis: {
                          name: '月份',
                          categories: Month,
                          labels: {
                              rotation: -45,
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
                              text: '交易金额'
                          },
                      },
                      tooltip: {
                          formatter: function () {
                              return '' + this.series.name + '' +
                                 this.x + ': ' + Highcharts.numberFormat(this.y, 2, '.') + unit;
                          },
                          //pointFormat: '{series.name}: <b>{point.y:.1f} millions</b>',
                      },
                      plotOptions: {
                          series: {
                              point: {
                                  events: {
                                      //鼠标移动至某个柱子上时显示副图表
                                      mouseOver: function () {
                                          var x, y, width = 500, height = 500;
                                          //获取当前鼠标的绝对位置值
                                          x = event.clientX;
                                          y = event.clientY;
                                          var objectData = new Object();
                                          objectData.title = this.category; //将当前柱子的X轴刻度值作为副图表的标题
                                          //调用函数弹出副图表
                                          DynamicCreateSubChart(objectData, width, height, x, y, $("#strategyname").val(), $("#stratinfo").val(), $("#seriesno").val(), unit);
                                      }
                                  }
                              },
                              events: {
                                  //鼠标移开柱子时的操作
                                  mouseOut: function () {
                                      //隐藏图表且回收资源
                                      HiddenSubChart();
                                  }
                              }
                          },
                          column: {
                              borderWidth: 0,
                              dataLabels: {
                                  enabled: true,
                                  format: '{point.y:.1f}'+unit
                              }
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
                          name: '交易金额',
                          data: MonthAccont
                      }]
                  });

              })
          }//end success
      });
    GetOrderSend(begindate, enddate, strategyname, stratkindname, seriesno);
    GetStrategyTopMatchQty(begindate, enddate, strategyname, stratkindname, seriesno);
}


///动态创建副图表
/// oject:对象内部可以封装很多数据 通用模式
/// widht:生成的副图表宽度
/// height:生成的副图表高度
/// left:图表距左侧窗体的距离值
/// top:图表距上侧窗体的距离值
///===============
function DynamicCreateSubChart(object, width, height, left, top, strategyname, stratkindname, seriesno, unit) {


    var da = {
        "beginDate": object.title,
        "strategyName": strategyname,
        "strategyKindName": stratkindname,
        "seriesNo": seriesno
    }
    $.ajax(
     {
         url: "/dashboard/api/GetTradeDate/GetDateAmount", //表示提交给的action 
         type: "post",   //提交方法 
         data: da,
         datatype: "json",//数据类型
         success: function (result) {
             //日期
             var Date = [];
             //日成交量
             var DateAccont = [];
             //日曲线图数据转换
             if (result[0].Date != null) {
                 if (unit == "亿元") {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount / 100000000);
                     }
                 }
                 else if (unit == "万元") {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount / 10000);
                     }
                 }
                 else {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount);
                     }
                 }
             }
             //动态设置副图表容器的相关属性并显示出来
             $("#divChart").css({ "left": left, "top": top, "width": width, "height": height, "display": "block" });
             ///动态给div渲染图表
             //这里可以再次进行封装 根据object内的参数动态渲染图表
             new Highcharts.Chart({
                 chart: {
                     renderTo: "divChart"
                 },
                 title: {
                     text: object.title + "的详细数据"
                 },
                 title: {
                     text: '月交易金额统计'
                 },
                 xAxis: {
                     name: '月份',
                     categories: Date,
                     labels: {
                         rotation: -45,
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
                         text: '交易金额'
                     },
                 },
                 tooltip: {
                     formatter: function () {
                         return '' + this.series.name + '' +
                            this.x + ': ' + Highcharts.numberFormat(this.y, 2, '.') + unit;
                     }
                 },
                 credits: {
                     enabled: false
                 },
                 legend: {
                     enabled: false
                 },
                 series: [{
                     name: '交易金额',
                     data: DateAccont
                 }]
             });
         }//end success
     });
}
///隐藏副图表
function HiddenSubChart() {
    $("#divChart").html("");
    $("#divChart").css({ "display": "none" });
}


//开仓次数走势图
function GetOrderSend(begindate, enddate, strategyname, stratkindname, seriesno) {
    var da = {
        "beginDate": begindate,
        "endDate": enddate,
        "strategyName": strategyname,
        "strategyKindName": stratkindname,
        "seriesNo": seriesno
    }

    $.ajax({
        url: '/dashboard/api/GetTradeDate/GetOrderSendNum',
        type: "post",   //提交方法 
        data: da,
        success: function (result) {
            //月份
            var month = [];
            //开仓次数
            var numsender = [];
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    month.push(result[i].Month);
                    numsender.push(result[i].NumSend);
                }
            }
            var numordersender = new Highcharts.Chart({
                chart: {
                    renderTo: 'NumOrder',
                    type: 'column'
                },
                title: {
                    text: '开仓次数'
                },
                credits: {
                    enabled: false
                },
                xAxis: {
                    categories: month
                },
                yAxis: {
                    title: {
                        text: '开仓次数'
                    }
                },
                plotOptions: {
                    column: {
                        borderWidth: 0,
                        dataLabels: {
                            enabled: true,
                            format: '{point.y:.0f}次'
                        }
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '' + this.series.name + '' +
                           this.x + ': ' + this.y;
                    }
                },
                series: [{
                    name: '开仓次数',
                    data: numsender
                }]
            })
        }
    });
}

//策略总交易量Top5
function GetStrategyTopMatchQty(begindate, enddate, strategyname, stratkindname, seriesno) {
    var da = {
        "beginDate": begindate,
        "endDate": enddate,
        "strategyName": strategyname,
        "strategyKindName": stratkindname,
        "seriesNo": seriesno
    }
    $.ajax({
        url: '/dashboard/api/GetTradeDate/GetTopMatchQty',
        data: da,
        type: 'post',
        success: function (result) {
            //1,获取上面id为cloneTr的tr元素  
            var tr = $("#cloneTr");
            $("#generatedTable tr:gt(0):not(:eq(0))").remove();
            $.each(result, function (index, item) {
                //克隆tr，每次遍历都可以产生新的tr                              
                var clonedTr = tr.clone().attr('id','clone');
                var _index = index;
                //循环遍历cloneTr的每一个td元素，并赋值  
                clonedTr.children("td").each(function (inner_index) {
                    
                    //根据索引为每一个td赋值  
                    switch (inner_index) {
                        case (0):
                            $(this).html(item.CustId);
                            break;
                        case (1):
                            $(this).html(item.StrategyName);
                            break;
                        case (2):
                            $(this).html(item.MatchQty);
                            break;
                    }//end switch  
                });//end children.each  

                //把克隆好的tr追加原来的tr后面  
                clonedTr.insertAfter(tr);
            });//end $each 
            $("#cloneTr").hide();//隐藏id=clone的tr，因为该tr中的td没有数据，不隐藏起来会在生成的table第一行显示一个空行  
            $("#generatedTable").show();
            $("tr#clone").show();
        }
    })
}