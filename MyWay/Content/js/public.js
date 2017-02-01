

var pagedata =
    {
        IntoLoding: 100,        //进入加载状态时间
        MinLoding: 500,           //加载状态最小持续时间
        LodingTimeoutId: 0,      //加载状态的加载时间定时器，0代表没有开始计时器
        rLodingQueue: 0,     //请求队列数，如果有数量，就不会关闭遮罩层
        rLodingLayerId: 0    //请求达到指定数值时候，弹出遮罩层的Id
    };
$.post2 = function (url, data, success, failure) {
    var r = $.extend({}, rLoding, { success: success, failure: failure });
    r.init();
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        error: function () {
            r.ajaxData = "failure:无法连接服务器，请检查网络是否连接！";
            r.end();
        }, success: function (data) {
            r.ajaxData = data;
            r.end();
        }
    });
};
$.get2 = function (url, data, success, failure) {
    var r = $.extend({}, rLoding, { success: success, failure: failure });
    r.init();
    $.ajax({
        type: "GET",
        url: url,
        data: data,
        error: function () {
            r.ajaxData = "failure:无法连接服务器，请检查网络是否连接！";
            r.end();
        }, success: function (data) {
            r.ajaxData = data;
            r.end();
        }
    });
};

var rLoding =
    {
        success: null,
        failure: null,
        ajaxData: null,
        IsMinLoding: false,  //是否满足最小加载状态时间
        IsRequestEnd: false, //请求是否结束
        init: function () {
            if (pagedata.rLodingQueue > 0) {
                this.IsMinLoding = true;
                this.begin();
                return;
            }
            setTimeout(function (_rLoding) {
                if (_rLoding.end == 1) return;
                _rLoding.begin();
            }, pagedata.IntoLoding, this);
        },
        begin: function () {
            if (this.IsRequestEnd) return;
            if (pagedata.rLodingLayerId) {
                pagedata.rLodingQueue++;
                //console.log("++" + pagedata.rLodingQueue);
                this.IsMinLoding = true;
                pagedata.rLodingLayerId = layer.load(1, { shade: [0.5, '#000'] });
            } else {
                pagedata.rLodingLayerId = layer.load(1, { shade: [0.5, '#000'] });
                setTimeout(function (args) {
                    pagedata.rLodingQueue++;
                    //console.log("++" + pagedata.rLodingQueue);
                    args.IsMinLoding = true;
                    if (args.IsRequestEnd) {
                        args.over();
                    }
                }, pagedata.MinLoding, this);
            }
        },
        end: function (fun, data) {
            this.IsRequestEnd = true;
            if (this.IsMinLoding || pagedata.rLodingLayerId == 0) {
                this.over();
                return;
            }
        },
        over: function () {
            ajaxEnd(this.ajaxData, this.success, this.failure);
            (this.IsMinLoding) && pagedata.rLodingQueue-- &&
            console.log("--" + pagedata.rLodingQueue);
            if (pagedata.rLodingQueue == 0) {
                layer.close(pagedata.rLodingLayerId);
                pagedata.rLodingLayerId = 0;
            }
        }
    }

function ajaxEnd(data, success, failure) {
    if (data.indexOf("success") == 0) {
        success(data.substr(8));
    } else if (data.indexOf("code") == 0) {
        var code = data.substr(5);
        $.ajaxCode(code);
        if (failure) failure(data);
    } else if (data.indexOf("failure") == 0) {
        layer.msg(data.substr(8), {icon:5});
        if (failure) failure(data);
    } else {
        alert("获取数据失败")
        if (failure) failure(data);
    }
}


$.ajaxCode = function (code) {
    if (code == "101") {
        alert("登录过期，请重新登录！", function () {
            top.location.href = "/";
        });
    } else if (code == "103") {
        alert("还没有完善信息，需要完善信息才能开始营销旅程！", function () {
            top.location.href = "/fun/perfectInfo.html";
        });
    }
    else if (code == "201") {
        alert("你的微信公众号没有认证，暂时使用不了此功能")
    }
    else if (code == "202") {
        alert("你的微信公众号微信订阅号，暂时使用不了此功能，如想使用此功能，请升级为服务号")
    }
};

//获取url参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    // if (r != null) return unescape(r[2]); return null; //返回参数值
    if (r != null) return decodeURI(r[2]); return null;
}





window.alert = function (message, callback) {
    top.layer.alert(message, {
        title: "温馨提示",
        end: function () {
            callback && callback();
        }
    });
}

var window_confirm = confirm;
window.confirm = function (message, callback) {
    if (!callback) {
        console.log("confirm方法已经被重写，调用方式发生了变化，请修改代码。");
        return window_confirm(message);
    }
    var layerId = layer.confirm(message, {
        btn: ['确定', '取消'] //按钮
         , title: "温馨提示",
        end: function () {
            console.log("测试ok");
            layer.close(layerId);
        }
    }, function (index) {
        callback && callback("0");
        layer.close(index);
    }, function (index) {
        callback && callback("1");
        layer.close(index);
    }
    );
    return false;
}
function confirm3btn(message, btn, callback) {
    var b = [];
    for (var i = 0; i < 3; i++) {
        b[i] = btn[i].name;
    }
    var layerId = layer.confirm(message, {
        btn: [btn[0].name, btn[1].name, btn[2].name] //按钮
         , title: "温馨提示",
        end: function () {
            console.log("测试ok");
            layer.close(layerId);
        }
    }, function (index) {
        callback && callback(btn[0].val);
        layer.close(index);
    }, function (index) {
        callback && callback(btn[1].val);
        layer.close(index);
    }, function (index) {
        callback && callback(btn[2].val);
        layer.close(index);
    }
    );
    return false;
}


//百度统计
var _hmt = _hmt || [];
; (function () {
    if (location.host != "www.yajingling.com") return;
    var hm = document.createElement("script");
    hm.src = "//hm.baidu.com/hm.js?e2bbe3675250b90f7c0b507ecec400b1";
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm, s);
})();

//function LoginOut() {
//    $.post2("/ss/Account/LoginOut", {}, function (data) {
//        window.location = "/";
//    });
//}
//$(function () {
//    if (getUrlParam("isRegisterGo") == 1) {
//        //$.post2(
//        //    "/ss/BusinessInfo/GetIndexUrl",
//        //    {},
//        //    function (data) {
//        //        var QRImg = "/ss/Doctor/ShowQRCode?url=http://" + data;
//        //        var layerindex = layer.open({
//        //            type: 2,
//        //            title: false,
//        //            shadeClose: false, //点击遮罩关闭层
//        //            area: ['710px', '480px'],
//        //            content: ['/static/html/regSeccess.html?QRImg=' + QRImg, 'no'],
//        //            skin: 'layui-layer-nobg', //没有背景色
//        //            moveType: 0,
//        //            cancel: function (index) { return false; },
//        //            closeBtn: 0
//        //        });
//        //        $("#layui-layer" + layerindex).css("box-shadow", "none");
//        //    });
//    }
//});

//var yjlpageSite = {
//    layId: 0, OpenLoading: function (data) {
//        if (typeof (data) != "string" || data.length == 0) {
//            data = "数据加载中...";
//        }
//        this.layId = layer.open({
//            type: 1,
//            title: false,
//            closeBtn: 0,
//            shadeClose: true,
//            skin: 'yourclass',
//            content: '<div style="text-align: center;width:200px;height:100px;opacity: 0.5;padding-top: 15px;border-radius: 10px 10px 9px 9px;"> <img src="/static/images/loading2.gif" alt="加载图片" style="width:32px;" /><div><span>' + data + '</span></div></div>'    
//        });
//    }, CloseLoading: function () {
//        layer.close(this.layId);
//    }
//};

//我的咨询和预约的栏目切换
//$(function () {
//    //$(".frameTab").hasClass("ManyDescr") = false 
//    $(".frameTab li").click(function () {
//        if ($(".framePageDescriptionBox .framePageDescription").length == 0) {
//            return;
//        } else {
//            var count = $(this).index();
//            if ($(".framePageDescriptionBox").children().eq(count).length == 0) {
//                return;
//            }
//            $(".framePageDescription").addClass("framePageDescriptionHide");
//            var s = $(".framePageDescriptionBox").children().eq(count).removeClass("framePageDescriptionHide");

//        }
//    })
//});

//鼠标移入移出显示文本的公共事件 sector选择器，dcard取值的特性,tempsector显示的文本模板ID, tarry要替换的文本
//function MouseHoverArchive(url,sector,darry,tempsector,tarry) {
//        $(sector+".ArchiveCard").hover(function () {
//            var $this = $(this);
//            var darry = [];
//            // var id = $(this).attr("data-card");
//            if (typeof (darry) == "string") {
//                var keys = darry.splice("-");
//                var key0 = keys[1];
//                var va0 = $this.attr(darry);
//                var obj0 = {};
//                eval("obj0." + key0 + "=" + va0);
//            } else if(Array.isArray(darry)&&darry.length>0){
//                for (var i = 0; i < darry.length; i++) {
//                    var keys = darry.splice("-");
//                    var key0 = keys[1];
//                    var va0 = $this.attr(darry);
//                    var obj0 = {};
//                    eval("obj0." + key0 + "=" + va0);
//                }
//            }

//            var id = $(this).attr(dcard);

//            id = parseInt(id);
//            if (id == 0) {
//                return;
//            }
//            if ($this.find(".ArchiveCardBox").length == 0) {
//                $.post2(url, { id: id }, function (data) {  ///ss/Archive/GetArchiveById
//                   // var tempHtml = $("#ArchiveCardTemplate").html();
//                    var tempHtml = $(tempId).html();
//                    var obj = JSON.parse(data);

//                    //var htmlstr = tempHtml.replace("#ArchiveName#", obj.ArchiveName).replace("#ArchiveSex#", obj.ArchiveSex).replace("#ArchivePhone#", obj.ArchivePhone).replace("#ArchiveNote#", obj.ArchiveNote).replace("#ArchiveId#", id).replace("#EId#", eid);
//                    if (Array.isArray(tarry) && tarry.length > 0) {
//                        for (var i = 0; i < tarry.length; i++) {
//                            tempHtml = tempHtml.replace(tarry[i], eval("obj."+tarry[i]));
//                        }
//                    }
//                    $this.append(htmlstr);
//                }, function () { });
//            }
//        });
//}


//左边菜单
//$(function () {
//    //菜单隐藏展开
//    var zk_vcon, zk_vtitle;
//    zk_vtitle = $(".vtitle>em.v02").parent(".vtitle");
//    zk_vcon = zk_vtitle.next();
//    $('.vtitle').click(function () {
//        if (zk_vcon) {
//            zk_vcon.slideUp();
//            zk_vtitle.find("em").removeClass('v02').addClass('v01');
//        }
//        var $this = $(this);
//        $next = $this.next();
//        if (zk_vcon && zk_vcon[0] == $next[0]) {
//            zk_vcon = null;
//            zk_vtitle = null;
//            return
//        }
//        $next.slideDown();
//        $this.find("em").removeClass('v01').addClass('v02');
//        zk_vcon = $next;
//        zk_vtitle = $this;
//    });
//});