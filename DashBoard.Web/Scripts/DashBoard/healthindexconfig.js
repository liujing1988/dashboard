$(document).ready(function () {
    $("#saveconfig").click(function ()
    {
        alert($("#MaxSecondTrade").val());
        if ($("#MaxDayTrade").val() == "" && $("#MaxMinuteTrade").val() == "" && $("#MaxSecondTrade").val() == "")
        {
            alert("交易量阈值不能为空");
        }

        $("#HealthIndexConfig").submit();

    })
})