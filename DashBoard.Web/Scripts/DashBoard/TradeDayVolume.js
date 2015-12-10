$.ajax(
{
    url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume", //表示提交给的action 
    type: "post",   //提交方法 
    datatype: "json",//数据类型
    success: function (result) {

        //委托笔数
        var NumOrder;
        if (result.NumOrder > 0) {
            NumOrder = result.NumOrder;
        }
        else {
            NumOrder = 0;
        }

        //最大委托笔数
        var MaxNumOrder = NumOrder;

        //成交笔数
        var NumVolum;
        if (result.NumVolum > 0) {
            NumVolum = result.NumVolum;
        }
        else {
            NumVolum = 0;
        }
        //撤单笔数
        var NumRevoke;
        if (result.NumRevoke > 0) {
            NumRevoke = result.NumRevoke;
        }
        else {
            NumRevoke = 0;
        }
        //成交金额
        var VolumAmt;
        if (result.VolumAmt > 0) {
            VolumAmt = result.VolumAmt;
        }
        else {
            VolumAmt = 0;
        }
        //逆回购金额
        var RRPAmt;
        if (result.RRPAmt != null && parseFloat(result.RRPAmt) > 0) {
            RRPAmt = parseFloat(result.RRPAmt);
        }
        else {
            RRPAmt = 0;
        }

        //总成交金额
        var TradeAmt = VolumAmt - RRPAmt;

        //买入金额
        var BuyAmt;
        if (result.BuyAmt != null && parseFloat(result.BuyAmt) > 0) {
            BuyAmt = parseFloat(result.BuyAmt);
        }
        else {
            BuyAmt = 0;
        }
        //卖出金额
        var SalesAmt;
        if (result.SalesAmt != null && parseFloat(result.SalesAmt) > 0) {
            SalesAmt = parseFloat(result.SalesAmt);
        }
        else {
            SalesAmt = 0;
        }
        //净买入金额
        var NETBuyAmt = BuyAmt - SalesAmt;
        $(function () {
            $(document).ready(function () {
                Highcharts.setOptions({
                    global: {
                        useUTC: false
                    }
                });

                $('#TradeDayVolume').highcharts({
                    chart: {
                        spacingBottom: 0,
                        height: 400,
                        type: 'column',
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series = this.series[0];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                        dataType: "json",
                                        success: function (data) {
                                            //委托笔数
                                            var rNumOrder;
                                            if (data.NumOrder > 0) {
                                                rNumOrder = data.NumOrder;
                                            }
                                            else {
                                                rNumOrder = 0;
                                            }

                                            //最大委托笔数
                                            var rMaxNumOrder = NumOrder;

                                            //成交笔数
                                            var rNumVolum;
                                            if (data.NumVolum > 0) {
                                                rNumVolum = data.NumVolum;
                                            }
                                            else {
                                                rNumVolum = 0;
                                            }
                                            //撤单笔数
                                            var rNumRevoke;
                                            if (data.NumRevoke > 0) {
                                                rNumRevoke = data.NumRevoke;
                                            }
                                            else {
                                                rNumRevoke = 0;
                                            }
                                            //成交金额
                                            var rVolumAmt;
                                            if (data.VolumAmt > 0) {
                                                rVolumAmt = data.VolumAmt;
                                            }
                                            else {
                                                rVolumAmt = 0;
                                            }
                                            //逆回购金额
                                            var rRRPAmt;
                                            if (data.RRPAmt != null && parseFloat(data.RRPAmt) > 0) {
                                                rRRPAmt = parseFloat(data.RRPAmt);
                                            }
                                            else {
                                                rRRPAmt = 0;
                                            }
                                            
                                            //买入金额
                                            var rBuyAmt;
                                            if (data.BuyAmt != null && parseFloat(data.BuyAmt) > 0) {
                                                rBuyAmt = parseFloat(data.BuyAmt);
                                            }
                                            else {
                                                rBuyAmt = 0;
                                            }
                                            //卖出金额
                                            var rSalesAmt;
                                            if (data.SalesAmt != null && parseFloat(data.SalesAmt) > 0) {
                                                rSalesAmt = parseFloat(data.SalesAmt);
                                            }
                                            else {
                                                rSalesAmt = 0;
                                            }
                                            //净买入金额
                                            var rNETBuyAmt = BuyAmt - SalesAmt;
                                            
                                            var rarray = [rNumOrder,rNumVolum,rNumRevoke];
                                            
                                            series.setData(rarray);
                                        }
                                    });

                                }, 1000 * 60);
                            }
                        }
                    },
                    title: {
                        useHTML: true,
                        text: '今日交易操作情况'
                    },
                    
                    xAxis: {
                        categories: ['总委托笔数', '总成交笔数', '总撤单笔数']
                    },
                    yAxis: {
                        title: {
                            text: null
                        }
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}笔</b>',
                        shared: true
                    },
                    series: [{
                        name: '笔数',
                        data: [NumOrder,NumVolum,NumRevoke]
                    }],
                    exporting: {
                        enabled: false
                    },
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


                $('#TradeDayVolumeAmt').highcharts({
                    chart: {
                        spacingBottom: 0,
                        height: 400,
                        type: 'column',
                        events: {
                            load: function () {

                                // set up the updating of the chart each second             
                                var series = this.series[0];
                                setInterval(function () {
                                    $.ajax({
                                        type: "post",
                                        url: "/DashBoard/api/GetTradeDate/GetTradeDayVolume",
                                        dataType: "json",
                                        success: function (data) {
                                            //委托笔数
                                            var rNumOrder;
                                            if (data.NumOrder > 0) {
                                                rNumOrder = data.NumOrder;
                                            }
                                            else {
                                                rNumOrder = 0;
                                            }

                                            //最大委托笔数
                                            var rMaxNumOrder = NumOrder;

                                            //成交笔数
                                            var rNumVolum;
                                            if (data.NumVolum > 0) {
                                                rNumVolum = data.NumVolum;
                                            }
                                            else {
                                                rNumVolum = 0;
                                            }
                                            //撤单笔数
                                            var rNumRevoke;
                                            if (data.NumRevoke > 0) {
                                                rNumRevoke = data.NumRevoke;
                                            }
                                            else {
                                                rNumRevoke = 0;
                                            }
                                            //成交金额
                                            var rVolumAmt;
                                            if (data.VolumAmt > 0) {
                                                rVolumAmt = data.VolumAmt;
                                            }
                                            else {
                                                rVolumAmt = 0;
                                            }
                                            //逆回购金额
                                            var rRRPAmt;
                                            if (data.RRPAmt != null && parseFloat(data.RRPAmt) > 0) {
                                                rRRPAmt = parseFloat(data.RRPAmt);
                                            }
                                            else {
                                                rRRPAmt = 0;
                                            }
                                            //总成交金额
                                            var rTradeAmt = rVolumAmt - rRRPAmt;
                                            //买入金额
                                            var rBuyAmt;
                                            if (data.BuyAmt != null && parseFloat(data.BuyAmt) > 0) {
                                                rBuyAmt = parseFloat(data.BuyAmt);
                                            }
                                            else {
                                                rBuyAmt = 0;
                                            }
                                            //卖出金额
                                            var rSalesAmt;
                                            if (data.SalesAmt != null && parseFloat(data.SalesAmt) > 0) {
                                                rSalesAmt = parseFloat(data.SalesAmt);
                                            }
                                            else {
                                                rSalesAmt = 0;
                                            }
                                            //净买入金额
                                            var rNETBuyAmt = (rBuyAmt - rSalesAmt).toFixed(2);

                                            var rarray = [rTradeAmt, rNETBuyAmt];

                                            series.setData(rarray);
                                        }
                                    });

                                }, 1000 * 60);
                            }
                        }
                    },
                    title: {
                        text: '今日交易金额情况'
                    },
                    xAxis: {
                        categories: ['总成交金额', '净买入总额']
                    },
                    yAxis: {
                        title: {
                            text: null
                        }
                    },
                    plotOptions: {
                        column: {
                            stacking: 'normal'
                        }
                    },
                    legend: {
                        enabled: false
                    },
                    tooltip: {
                        pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}元</b>',
                        shared: true
                    },
                    series: [{
                        name: '金额',
                        data: [TradeAmt, NETBuyAmt]
                    }],
                    exporting: {
                        enabled: false
                    },
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
