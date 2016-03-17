$.ajax(
{
    url: "/DashBoard/api/GetCustomer/GetCustomerOnline", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {

        //在线用户数
        var NumCustomer;
        if (result.NumCustomerOnline > 0) {
            NumCustomer = result.NumCustomerOnline;
        }
        else {
            NumCustomer = 0;
        }

        //最大在线用户数
        var MaxNumCustomer = NumCustomer;

        //当前在线/今日最大在线
        var PerOnline = NumCustomer * 100 / MaxNumCustomer;

        //信用用户数
        var CreditCustomer;
        if (result.CreditCustomer > 0) {
            CreditCustomer = result.CreditCustomer;
        }
        else {
            CreditCustomer = 0;
        }
        //普通用户数
        var NormalCustomer = NumCustomer - CreditCustomer;
        //服务器日期
        var ServerDate = result.ServerDate;
        var pieArray = [];
        if (CreditCustomer > 0 && NormalCustomer > 0) {
            var array = [['两融用户', CreditCustomer / NumCustomer * 100], ['普通用户', NormalCustomer / NumCustomer * 100]];
            for (var k = 0; k < array.length; k++) {
                pieArray.push(array[k]);
            }
        }
        
        $(function () {
            $(document).ready(function () {
                Highcharts.setOptions({
                    global: {
                        useUTC: false
                    }
                });

                $('#CreditCustomerOnline').highcharts({
                        events: {
                            load: function () {

                                // set up the updating of the chart each second   
                                var series0 = this.series[0];
                                var series1 = this.series[1];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetCustomer/GetCustomerOnline",
                                        dataType: "json",
                                        success: function (data) {
                                            //在线用户数
                                            var rNumCustomer;
                                            if (data.NumCustomerOnline > 0) {
                                                rNumCustomer = data.NumCustomerOnline;
                                            }
                                            else {
                                                rNumCustomer = 0;
                                            }

                                            //最大在线用户数
                                            var rMaxNumCustomer = MaxNumCustomer;
                                            if (MaxNumCustomer < rNumCustomer);
                                            {
                                                rMaxNumCustomer = rNumCustomer;
                                            }

                                            //在线用户数占比
                                            rPerOnline = rNumCustomer * 100 / rMaxNumCustomer;

                                            //信用用户数
                                            var rCreditCustomer;
                                            if (result.CreditCustomer > 0) {
                                                rCreditCustomer = data.CreditCustomer;
                                            }
                                            else {
                                                rCreditCustomer = 0;
                                            }
                                            //普通用户数
                                            var rNormalCustomer = rNumCustomer - rCreditCustomer;
                                            //服务器日期
                                            var rServerDate = data.ServerDate;
                                            var rarray = [['两融用户', rCreditCustomer / rNumCustomer * 100], ['普通用户', rNormalCustomer / rNumCustomer * 100]];
                                            var rpieArray = [];
                                            for (var k = 0; k < rarray.length; k++) {
                                                rpieArray.push(rarray[k]);
                                            }
                                            series0.setData(rPerOnline);
                                            series1.setData(rpieArray);
                                        }
                                    });

                                }, 1000 * 60);
                        }
                    },
                    title: {
                        useHTML: true,
                        text: ServerDate + '<br>在线用户数<br><font size= 5 color = green>' + NumCustomer + '</font><br>' +
                            '其中两融用户数<br><font size= 5 color=green>' + CreditCustomer + '</font>',
                        align: 'left'
                    },
                    xAxis: {
                        categories: ['在线用户']
                    },
                    yAxis: {
                        labels: {
                            enabled: false
                        },
                        title: {
                            enabled: false
                        }
                    },
                    credits: {
                        enabled: false
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        pie: {
                            dataLabels: {
                                enabled: true,
                                distance: -30,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {
                                    fontWeight: 'bold',
                                    color: 'black',
                                    textShadow: '0px 0px 0px black'
                                }
                            }
                        },
                        column: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                                format: '{point.y:.1f}%'
                            }
                        }
                    },
                    tooltip: {
                        enabled:false
                    },
                    series: [{
                        name: '在线用户',
                        type: 'column',
                        data: [PerOnline],
                        pointPadding: 0.4,
                        pointPlacement: -0.3
                    }, {
                        name: '两融用户占比',
                        type: 'pie',
                        data: pieArray,
                        center: [180, 50],
                        size: 100,
                        showInLegend: false,

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
                })
                //var chart;
                //$('#CreditCustomerOnline').highcharts({
                //    chart: {
                //        height: 200,
                //        events: {
                //            load: function () {

                //                // set up the updating of the chart each second             
                //                var series = this.series[0];
                //                setInterval(function () {
                //                    $.ajax({
                //                        type: "post",
                //                        url: "/DashBoard/api/GetCustomer/GetCustomerOnline",
                //                        dataType: "json",
                //                        success: function (data) {
                //                            //在线用户数
                //                            var rNumCustomer;
                //                            if (data.NumCustomerOnline > 0) {
                //                                rNumCustomer = data.NumCustomerOnline;
                //                            }
                //                            else {
                //                                rNumCustomer = 0;
                //                            }

                //                            //最大在线用户数
                //                            var rMaxNumCustomer = MaxNumCustomer;
                //                            if (MaxNumCustomer < rNumCustomer);
                //                            {
                //                                rMaxNumCustomer = rNumCustomer;
                //                            }

                //                            //信用用户数
                //                            var rCreditCustomer;
                //                            if (result.CreditCustomer > 0) {
                //                                rCreditCustomer = data.CreditCustomer;
                //                            }
                //                            else {
                //                                rCreditCustomer = 0;
                //                            }
                //                            //普通用户数
                //                            var rNormalCustomer = rNumCustomer - rCreditCustomer;
                //                            //服务器日期
                //                            var rServerDate = data.ServerDate;
                //                            var rarray = [['两融用户', rCreditCustomer / rNumCustomer * 100], ['普通用户', rNormalCustomer / rNumCustomer * 100]];
                //                            var rpieArray = [];
                //                            for (var k = 0; k < rarray.length; k++) {
                //                                rpieArray.push(rarray[k]);
                //                            }
                //                            series.setData(rpieArray);
                //                        }
                //                    });

                //                }, 1000 * 60);
                //            }
                //        }
                //    },
                //    title: {
                //        useHTML:true,
                //        text: '其中两融用户<br><font size= 5 color=green>' + CreditCustomer + '</font>',
                //        align:'left'
                //    },
                //    credits: {
                //        enabled: false
                //    },
                //    plotOptions: {
                //        pie: {
                //            allowPointSelect: true,
                //            cursor: 'pointer',
                //            dataLabels: {
                //                enabled: true,
                //                color: '#000000',
                //                connectorColor: '#000000',
                //                format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                //            }
                //        }
                //    },
                //    series: [{
                //        name: '两融用户占比',
                //        type:'pie',
                //        data: pieArray,

                //    }],
                //    legend: {
                //        enabled: false
                //    },
                //    exporting: {
                //        enabled: false
                //    },
                //    lang: {
                //        noData: "Nichts zu anzeigen"
                //    },
                //    tooltip: {
                //        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                //    },
                //    noData: {
                //        style: {
                //            fontWeight: 'bold',
                //            fontSize: '15px',
                //            color: '#303030'
                //        }
                //    }
                //});



                //$('#CustomerOnline').highcharts({
                //    chart: {
                //        spacingBottom:0,
                //        height: 200,
                //        type:'column',
                //        events: {
                //            load: function () {

                //                // set up the updating of the chart each second             
                //                var series = this.series[0];
                //                setInterval(function () {
                //                    $.ajax({
                //                        type: "post",
                //                        url: "/DashBoard/api/GetCustomer/GetCustomerOnline",
                //                        dataType: "json",
                //                        success: function (data) {
                //                            //在线用户数
                //                            var rNumCustomer;
                //                            if (data.NumCustomerOnline > 0) {
                //                                rNumCustomer = data.NumCustomerOnline;
                //                            }
                //                            else {
                //                                rNumCustomer = 0;
                //                            }

                //                            //最大在线用户数
                //                            var rMaxNumCustomer = MaxNumCustomer;
                //                            if (MaxNumCustomer < rNumCustomer);
                //                            {
                //                                rMaxNumCustomer = rNumCustomer;
                //                            }

                //                            //信用用户数
                //                            var rCreditCustomer;
                //                            if (result.CreditCustomer > 0) {
                //                                rCreditCustomer = data.CreditCustomer;
                //                            }
                //                            else {
                //                                rCreditCustomer = 0;
                //                            }
                //                            //普通用户数
                //                            var rNormalCustomer = rNumCustomer - rCreditCustomer;
                //                            //服务器日期
                //                            var rServerDate = data.ServerDate;
                //                            var rarray = [{ name: '在线用户', data: [rNumCustomer] }];
                //                            var rpieArray = [];
                //                            for (var k = 0; k < rarray.length; k++) {
                //                                rpieArray.push(rarray[k]);
                //                            }
                //                            series.setData(rpieArray);
                //                        }
                //                    });

                //                }, 1000 * 60);
                //            }
                //        }
                //    },
                //    title: {
                //        useHTML:true,
                //        text: ServerDate + '<br>在线用户数<br><font size= 5 color = green>' + NumCustomer + '</font>',
                //        align: 'left'
                //    },
                //    xAxis: {
                //        categories: ['在线用户']
                //    },
                //    yAxis: {
                //        title: {
                //            text: null
                //        }
                //    },
                //    credits: {
                //        enabled: false
                //    },
                //    plotOptions: {
                //        column: {
                //            stacking: 'normal'
                //        },
                //        dataLabels: {
                //            enabled: true,
                //            color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                //        }
                //    },
                //    legend: {
                //        enabled: false
                //    },
                //    tooltip: {
                //        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                //        shared: true
                //    },
                //    series: [{
                //        name: '不在线用户数',
                //        data: [(MaxNumCustomer - NumCustomer)]
                //    }, {
                //        name: '在线用户',
                //        data: [NumCustomer]
                //    }],
                //    exporting: {
                //        enabled: false
                //    },
                //    lang: {
                //        noData: "Nichts zu anzeigen"
                //    },
                //    noData: {
                //        style: {
                //            fontWeight: 'bold',
                //            fontSize: '15px',
                //            color: '#303030'
                //        }
                //    }
                //});
            });

        });
    }
});
