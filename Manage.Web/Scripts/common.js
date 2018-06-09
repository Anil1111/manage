/*********************************************************

* 作者：
* 创建日期：2014/11/13

**********************************************************/

//ajax
$.ajaxjson = function (url, dataMap, fnSuccess) {
    $.ajax({
        type: "post",
        dataType: "json",
        url: url,
        async: false,
        data: dataMap,
        beforeSend: function () { },
        complete: function () { },
        success: fnSuccess,
        error: function (msg) {
            alert(msg);
        }
    });
}

/*******************************************************************************
 * function : isNull author : flotage create : 2012.07.31 des : 判断字符是否为空（空格也算空）
 * parm
 ******************************************************************************/
function isNull(str) {
    if (undefined == str || null == str || $.trim(str) == ""
        || "undefined" == str || "null" == str || "(null)" == str) {
        return true;
    }
    return false;
}

function checkReturn(d) {
    var state = d.state;
    var msg = d.message;

    if (state == 4) {
        bDialog.alert("没有操作权限，请联系系统管理员", null, { messageType: 'error' });
        return false;
    } else if (state == 2) {
        window.location.href = "/Login/Index";
        return false;
    }
}