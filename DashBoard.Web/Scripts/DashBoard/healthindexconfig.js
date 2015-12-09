$(document).ready(function () {
    $("#saveconfig").click(function ()
    {
        if ($("#MaxDayTrade").val() == "" && $("#MaxMinuteTrade").val() == "" && $("#MaxSecondTrade").val() == "")
        {
            alert("交易量阈值不能为空");
        }

        $("#HealthIndexConfig").submit();

    })
})