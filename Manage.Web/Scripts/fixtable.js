$(function () {
    if (parseInt($("#pagecount").val()) != 0) {
        pageClickedEventStoppable();
    }
    sidebarToggle();
});

function sidebarToggle() {
    var w = $(window).width() - $("#sidebar").width();
    var h = $(window).height() - $("#navbar").height() - $("#breadcrumbs").height() - $("#searchDiv").height() - $("#btnBar").height() - $("#btnBar2").height() - $("#pager").height();
    w = w - 30;
    h = h - 50;

    $("#grid").height(h);
    $("#grid").width(w);

    $('#fixTable').fixedHeaderTable({ cloneHeadToFoot: true, altClass: 'odd', width: "" + w + "" });
}

function resetPage(){
    var checkValue = $("#pageSizeSel").val();
    $("#rows").val(checkValue);
    $("#page").val(1);
}

//重置所有条件包含隐藏域
function clearForm(objstr) {
    if ($) {
        $(':text,select,textarea,:checkbox,:hidden', '#' + objstr).val("").attr("data-init", "");
    }
}
//重置不含隐藏域所有条件
function clearFormNoHidden(objstr) {
    if ($) {
        $(':text,select,textarea,:checkbox', '#' + objstr).val("").attr("data-init", "");
    }
}

function selChange() {
    var checkValue = $("#pageSizeSel").val();
    $("#rows").val(checkValue);
    $("#page").val(1);
    doSearch();
}

function pageClickedEventStoppable() {
    var options = {
        bootstrapMajorVersion: 3,
        numberOfPages: 5,
        currentPage: $("#page").val(),
        totalPages: $("#pagecount").val(),
        itemTexts: function (type, page, current) {
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "尾页";
                case "page":
                    return page;
            }
        },
        onPageClicked: function (e, originalEvent, type, page) {
            e.stopImmediatePropagation();
            $("#page").val(page);
            $("#searchForm").submit();
        }
    }

    $('#paginator').bootstrapPaginator(options);
}

function resetSearch() {
    $("#page").val(1);
}