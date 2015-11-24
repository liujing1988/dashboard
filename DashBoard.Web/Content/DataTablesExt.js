//对行单击添加监听事件
$('tbody').on('click', 'tr', function () {
    var tr = $(this).closest('tr');
    var checkbox = tr.find('td:first-child input:checkbox')[0];
    checkbox.checked = !checkbox.checked;
});

//获Ggridview中所有的复选框 sName: Gridview 的ID  
function getCheckbox(sName) {
    var aryCheckbox = new Array();
    var tb = document.getElementById(sName);
    if (tb == null)
        return;
    var objs = tb.getElementsByClassName("cbx");
    for (var i = 0; i < objs.length; i++) {
        if (objs[i].type == 'checkbox')
            aryCheckbox.push(objs[i]);
    }
    return aryCheckbox;
}

//监听每一行的复选框,控制全选、反选按钮  
function controlSelectAll(i) {
    var tblName, cbkAll; //Gridview ID ，全选框ID  
    var tblName = "table_local";
    var cbkAll = "chkAllColl";
    var id = "#cbx" + i;
    if ($(id)[0].checked == true) {
        $(id).closest('tr').addClass('selected');
    } else {
        $(id).closest('tr').removeClass('selected');
    }
    var chks = getCheckbox(tblName);
    var count = 0;
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked == true) {
            count++;
        }
    }
    if (count < chks.length) {
        document.getElementById(cbkAll).checked = false;
    }
    else {
        document.getElementById(cbkAll).checked = true;
    }
}