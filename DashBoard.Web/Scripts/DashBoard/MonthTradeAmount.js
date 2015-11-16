$(document).ready(function ($) {
    
    GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val());
    
    $("#unit").change(function () {
        GetMonth($('#getMonth_Trade span').html().substr(0, 7), $('#getMonth_Trade span').html().substr(10, 7), $("#unit").val());
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
        GetMonth(starmonth, endmonh, $("#unit").val());
    });
});
function GetMonth(begindate, enddate, munit) {
    if (begindate == "" && enddate == "")
    {
        var myDate = new Date();
        var year = myDate.getFullYear() - 1;
        var month = myDate.getMonth() + 1;
        begindate = year + "-" + month;
        enddate = myDate.getFullYear() + "-" + month;
    }
    if (begindate == "")
    {
        alert("请输入起始时间");
    }
    else if (enddate == "")
    {
        alert("请输入截止时间");
    }
    var da = {
        "begindate": begindate,
        "enddate": enddate
    }

    $.ajax(
      {
          url: "/DashBoard/api/GetTradeDate/GetMatchAmount", //表示提交给的action 
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
              if (result[0].Month != null) {
                  if(munit == "003")
                  {
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
                          text: '月交易量统计'
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
                          min:0,
                          title: {
                              text: '交易量'
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
                                          DynamicCreateSubChart(objectData, width, height, x, y);
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
                          }
                      },

                      legend: {
                          layout: 'vertical',
                          align: 'right',
                          verticalAlign: 'middle',
                          borderWidth: 0
                      },

                      series: [{
                          name: '交易量',
                          data: MonthAccont
                      }]
                  });
              })
          }//end success

      });
}


///动态创建副图表
/// oject:对象内部可以封装很多数据 通用模式
/// widht:生成的副图表宽度
/// height:生成的副图表高度
/// left:图表距左侧窗体的距离值
/// top:图表距上侧窗体的距离值
///===============
function DynamicCreateSubChart(object, width, height, left, top) {
    

    var da = {
        "begindate": object.title
    }
    $.ajax(
     {
         url: "/DashBoard/api/GetTradeDate/GetDateAmount", //表示提交给的action 
         type: "post",   //提交方法 
         data: da,
         datatype: "json",//数据类型
         success: function (result) {
             //日期
             var Date = [];
             //日成交量
             var DateAccont = [];
             var minresult = result[0].DateAmount;
             var unit = "";
             //日曲线图数据转换
             if (result[0].Date != null) {
                 for (var i = 0; i < result.length; i++) {
                     if (minresult > result[i].DateAmount) {
                         minresult = result[i].DateAmount;
                     }
                 }
                 if (minresult / 10000000 >= 1) {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount / 100000000);
                         unit = "亿元";
                     }
                 }
                 else if (minresult / 10000 >= 1) {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount / 10000);
                         unit = "万元";
                     }
                 }
                 else {
                     for (var i = 0; i < result.length; i++) {
                         Date.push(result[i].Date);
                         DateAccont.push(result[i].DateAmount);
                         unit = "元";
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
                     text: '月交易量统计'
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
                         text: '交易量'
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
                     name:'交易量',
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