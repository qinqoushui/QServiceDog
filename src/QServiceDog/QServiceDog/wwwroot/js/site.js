// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function toAbsURL(url) {
    var a = document.createElement('a');
    a.href = url;
    return a.href;
}

function reQuery(grid, onlyChange) {
    if (!grid)
        grid = $("#gridContainer");
    if (onlyChange)
        grid.dxDataGrid("instance").refresh(true).done(function () {
            grid.dxDataGrid("instance").pageIndex(0);
        });
    else
        grid.dxDataGrid("instance").refresh().done(function () {
            grid.dxDataGrid("instance").pageIndex(0);
        });
    //var store = grid.dxDataGrid("getDataSource").store();
    //store.load().done(function (result) {
    //});
}

function onGridToolbarPreparing(e, options) {
    var dataGrid = e.component;
    for (var i = 0; i < options.length; i++) {
        var option = options[i];
        e.toolbarOptions.items.unshift({
            location: "after",
            widget: "dxButton",
            options: {
                icon: option.icon,
                hint: option.hint,
                onClick: (function (o) {
                    return function (e) {
                        var data = dataGrid.option(o.data || "pdata");
                        o.onClick(data);
                    };
                })(option)
            }
        });
    }
}

Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份
        "d+": this.getDate(), //日
        "h+": this.getHours(), //小时
        "m+": this.getMinutes(), //分
        "s+": this.getSeconds(), //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
