

GetPageList(cid, 1);

//show();
//遍历添加评论区的展开和收起事件
function show() {

    var pinglundiv = $('#pinglundiv');

    pinglundiv.children().find('div').each(function (index, e) {
        var pinglun = $("#" + e.id + "");
        var newbox = $('<div style="text-indent:2em;"></div>');
        var btn = $('<a class="pull-right" style="padding:20px;"></a>');
        var text = $.trim(pinglun.text());
        newbox.text(text.length > 100 ? text.substring(0, 100) + '......' : text);
        btn.text(text.length > 50 ? '展开' : '');

        btn.attr('http', '###');
        btn.click(function () {
            if (btn.text() == '展开') {
                btn.text('收起');
                newbox.text(text);
            } else {
                btn.text('展开');
                newbox.text(text.substring(0, 100) + '......');
            }
        });
        pinglun.html('');
        pinglun.append(newbox);
        pinglun.append(btn);
    })
}


//获取URL？后面的参数（bcode 诊所编号）(PS:支持获取多个参数。。。。)
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    // if (r != null) return unescape(r[2]); return null; //返回参数值
    if (r != null)
        return decodeURI(r[2]);
    return null;
}


//用户评论
function pingluntijiao() {
    if (isLogin == 'False') {
        layer.msg('请先登录才能评论哦', { icon: 6 });
        return;
    }
    var text = $('#pingluntext');
    if (text.length == 0) {
        layer.msg('请输入评论内容！', { icon: 6 });
        return;
    }
    //var textstr = TiHuanStr(text.val());
    $.post2('/content/PingLunTiTiao', { text: text.val(), cid: cid }, function (data) {
        layer.msg('评论成功', { icon: 6 });
        var jsondata = eval('(' + data + ')');
        var pinglundiv = $('#pinglundiv');
        var html = '';
        for (var i = 0; i < jsondata.length; i++) {
            html += '<div>';
            html += '<h4 class="text-primary">' + jsondata[i].UserName + '</h4>';
            html += '<div class="text-muted" style="text-indent:2em;" id="p_' + jsondata[i].Id + '">' + jsondata[i].Text + '</div>';
            html += ' <br />';
            html += '<footer style="color:blue;">评论时间:<cite title="Source Title">' + jsondata[i].StringTime + '</cite></footer>';
            html += '<hr /></div>';
        }
        text.val("");

        pinglundiv.html(html);
        GetPageList(cid, 1);
    });
}




GetLabel();
//获取标签云
function GetLabel() {
    $.post2('/home/GetLabel', {}, function (data) {
        var josndata = eval('(' + data + ')');
        var html = '';
        for (var i = 0; i < josndata.length; i++) {
            html += '<li><a href="#">' + josndata[i].LabelName + '</a></li>';
        }
        $('#biaoqianyun').html(html);
    });
}




var checkPage = 1;//保存总页数



function GetPageList(typeId, pageIndex) {
    if (pageIndex == null || pageIndex == "") {
        layer.msg('请输入跳转页数', { icon: 6 });
        return;
    }

    if (pageIndex > checkPage) {
        layer.msg('页码不能超过总页数', { icon: 6 });
        return;
    }
    $.post2('/content/GetCommentPageList', { cid: cid, pageIndex: pageIndex }, function (data) {

        var jsondata = eval('(' + data + ')');
        if (jsondata.pageCount != 0)
            checkPage = jsondata.pageCount;//赋值给总页数

        //生成数据列表
        var html = "";
        if (jsondata.cList.length > 0) {
            var arr = ['success', 'info', 'primary', 'warning', 'danger'];

            var comment;
            var huifu;
            for (var i = 0; len = jsondata.cList.length, i < len; i++) {
                comment = jsondata.cList[i];
                html += '<div class="panel panel-success">';
                html += '<div class="panel-heading">昵称：'+comment.UserName+'<span class="pull-right">#'+comment.LouCeng+'楼</span></div>';
                html += '<div class="panel-body" id="p_'+comment.Id+'">';
                html += '<div class="bs-example" data-example-id="media-list">';
                html += '<ul class="media-list">';
                html += '<li class="media">';
                html += '<div class="media-left"><a href="#"><img class="media-object" data-src="holder.js/64x64" alt="64x64" src="'+comment.UserImg+'" data-holder-rendered="true" style="width: 64px; height: 64px;"></a></div>';
                html += '<div class="media-body">';
                //html += '<h4 class="media-heading">'+comment.UserName+'</h4>';
                html += '<p>' + comment.Text + '</p>';
                //判断是否有用户回复
                if(comment.HuiFuList!=null){
                    for (var i = 0; lens = comment.HuiFuList.length, i< lens; i++) {
                        huifu = comment.HuiFuList[i];
                        html += '<div class="media">';
                        html += '<div class="media-left">';
                        html += '<a href="#"><img class="media-object" data-src="holder.js/64x64" alt="64x64" src="'+huifu.UserImg+'" data-holder-rendered="true" style="width: 64px; height: 64px;"></a></div>';
                        html += '<div class="media-body">';
                        //html += '<h4 class="media-heading">'+huifu.UserName+'回复'+huifu.HuiFuUserName+'</h4>';
                        html += huifu.Text;
                        html += '</div>';
                        html += '</div>';
                        html += '';
                    }
                }
                html += '</div>';
                html += '</li>';
                html += '</ul>';
                html += '</div>';
                html += '</div>';
                html += '<div class="panel-footer">';
                html += '评论时间:<cite>' + comment.StringTime + '</cite>';
                if (!comment.IsZhiChiOrFanDui) {
                    html += '<button type="button" id="FanDuia_' + comment.Id + '" class="pull-right btn btn-default btn-xs" onclick="FanDui(' + comment.Id + ')">反对(<strong id="FanDui_' + comment.Id + '" >' + comment.FanDui + '</strong>)</button>';
                    html += '<button style="margin-right: 10px;" type="button" id="ZhiChia_' + comment.Id + '" class="pull-right btn-default btn btn-xs" onclick="ZhiChi(' + comment.Id + ')">支持(<strong id="ZhiChi_' + comment.Id + '" >' + comment.ZhiChi + '</strong>)</button></div>';
                } else {
                    if (comment.ZhiChiOrFanDui == 0) {
                        html += '<button type="button" id="FanDuia_' + comment.Id + '" class="pull-right btn btn-default btn-xs" disabled="disabled">反对(<strong id="FanDui_' + comment.Id + '" >' + comment.FanDui + '</strong>)</button>';
                        html += '<button style="margin-right: 10px;" type="button" id="ZhiChia_' + comment.Id + '" class="pull-right btn-default btn btn-xs active" onclick="chongfu()">支持(<strong id="ZhiChi_' + comment.Id + '" >' + comment.ZhiChi + '</strong>)</button></div>';
                    } else {
                        html += '<button type="button" id="FanDuia_' + comment.Id + '" class="pull-right btn btn-default btn-xs active" onclick="chongfu()">反对(<strong id="FanDui_' + comment.Id + '" >' + comment.FanDui + '</strong>)</button>';
                        html += '<button style="margin-right: 10px;" type="button" id="ZhiChia_' + comment.Id + '" class="pull-right btn-default btn btn-xs" disabled="disabled">支持(<strong id="ZhiChi_' + comment.Id + '" >' + comment.ZhiChi + '</strong>)</button></div>';
                    }
                }

                //html += '<button type="button" id="FanDuia_' + comment.Id + '" class="pull-right btn btn-default btn-xs" onclick="FanDui(' + comment.Id + ')">反对(<strong id="FanDui_' + comment.Id + '">'+comment.FanDui+'</strong>)</button>';
                //html += '<button type="button" id="FanDuia_' + comment.Id + '" class="pull-right btn btn-default btn-xs" onclick="ZhiChi(' + comment.Id + ')">支持(<strong id="FanDui_' + comment.Id + '">'+comment.ZhiChi+'</strong>)</button>';
                //html += '<a class="pull-right" style="padding-right:10px;" href="javascript:;" onclick="HuiFu()">回复</a>';
                html += '</div>';
                html += '</div>';
            }
               

            if (jsondata.Count > 5) {
                //跳转页面的div
                html += '<div class="row">';
                html += '<nav class="text-center">';
                html += '<ul class="pagination" id="page" style="margin-bottom: 0px;">';
                html += '</ul>';
                html += '</nav>';
                html += '<div class="text-center" id="tiaozhuan" style="padding:5px;">';
                html += '</div>';
                html += '</div>';
            }
        } else {
            html += '<h3 class="text-center">暂无评论</h3>';
        }
        $('#pinglundiv').html(html);



        if (jsondata.Count > 5) {
            //生成分页按钮
            var htmlPage = '';
            htmlPage += '<li id="shouye"></li>';
            htmlPage += '<li id="shangyiye" style="float:left;"></li> ';
            htmlPage += ' <li style="float:left;padding:0 3px 0 3px;"><select class="form-control" onchange="GetPageList(' + cid + ',this.value)">';
            for (var i = 1; i <= jsondata.pageCount; i++) {
                if (i == jsondata.pageThis) {
                    htmlPage += '<option selected value="' + i + '">' + i + '</option>';
                } else {
                    htmlPage += '<option value="' + i + '">' + i + '</option>';
                }
            }
            htmlPage += '</select></li> ';

            htmlPage += ' <li id="xiayiye"><a href="javascript:;" aria-label="Next"><span aria-hidden="true">下一页</span></a></li>';
            htmlPage += '<li id="weiye"><a href="javascript:;" onclick="GetPageList(' + cid + ',' + jsondata.pageCount + ');">尾页</a></li>';
            $('#page').html(htmlPage);




            //计算上一页下一页
            var shangyiye;
            var xiayiye;
            var shangyiyehtml = $('#shangyiye');
            var xiayiyehtml = $('#xiayiye');
            var shouye = $('#shouye');
            var weiye = $('#weiye');
            if (jsondata.pageThis == 1) {
                shouye.addClass('disabled');
                shangyiyehtml.addClass('disabled');
                shouye.html('<a href="javascript:;">首页</a>');
                shangyiyehtml.html('<a href="javascript:;" aria-label="Previous"><span aria-hidden="true">上一页</span></a>');
            } else {
                shouye.removeClass('disabled');
                shangyiyehtml.removeClass('disabled');
                shangyiye = jsondata.pageThis - 1;
                shangyiyehtml.html('<a href="javascript:;" aria-label="Previous" onclick="GetPageList(' + cid + ',' + shangyiye + ');"><span aria-hidden="true">上一页</span></a>');
                shouye.html('<a href="javascript:;" onclick="GetPageList(' + cid + ',1);">首页</a>');
            }
            if (jsondata.pageThis == jsondata.pageCount) {
                weiye.addClass('disabled');
                xiayiyehtml.addClass('disabled');
                xiayiyehtml.html('<a href="javascript:;" aria-label="Next"><span aria-hidden="true">下一页</span></a>');
                weiye.html('<a href="javascript:;">尾页</a>');
            } else {
                weiye.removeClass('disabled');
                xiayiyehtml.removeClass('disabled');
                xiayiye = jsondata.pageThis + 1;
                xiayiyehtml.html('<a href="javascript:;" aria-label="Next" onclick="GetPageList(' + cid + ',' + xiayiye + ');"><span aria-hidden="true">下一页</span></a>');
                weiye.html('<a href="javascript:;" onclick="GetPageList(' + cid + ',' + jsondata.pageCount + ');">尾页</a>');
            }

            //生成跳转页码按钮
            var tiaozhuan = '';
            tiaozhuan += '<span><input class="text text-info" type="text" id="tiaozhuanpage" name="number" value="' + jsondata.pageThis + '" style="width:50px" onkeyup="key(this.value)"/> <button class="btn btn-sm btn-primary" onclick="GetPageList(' + cid + ',GetPage());"> 跳转 </button></span> ';
            tiaozhuan += '<span>总页数：<strong style="color:blue;">' + jsondata.pageCount + '</strong> 页 </span><span> 总条数：<strong style="color:blue;">' + jsondata.Count + '</strong> 条 </span>';
            $('#tiaozhuan').html(tiaozhuan);
        }
    })
}



//支持
function ZhiChi(commentId) {
    if (isLogin == 'False') {
        layer.msg('请先登录才能支持哦', { icon: 6 });
        return;
    }
    $.post2('/content/ZhiChi', { commentId: commentId }, function (data) {
        $("#ZhiChia_" + commentId).attr('disabled', 'disabled');
        $("#FanDuia_" + commentId).attr('onclick', 'chongfu()');
        $("#FanDuia_" + commentId).addClass('active');
        $("#ZhiChi_" + commentId).text(data);
    });
}

//反对
function FanDui(commentId) {
    if (isLogin == 'False') {
        layer.msg('请先登录才能反对哦', { icon: 6 });
        return;
    }
    $.post2('/content/FanDui', { commentId: commentId }, function (data) {
        $("#FanDuia_" + commentId).attr('disabled', 'disabled');
        $("#ZhiChia_" + commentId).attr('onclick', 'chongfu()');
        $("#ZhiChia_" + commentId).addClass('active');
        $("#FanDui_" + commentId).text(data);
    });
}

function chongfu() {
    layer.msg('请不要重复投票', { icon: 6 });
}

//回复
function HuiFu(uid) {
    if (isLogin == 'False' || userId == 0) {
        layer.msg('请先登录才能支持哦', { icon: 6 });
        return;
    }
    if (uid == userId) {
        layer.msg('不能回复给自己哦', { icon: 5 });
        return;
    }
    var text = "";
    //默认prompt
    layer.prompt(function (val) {
        text = val;
        if (text.length == 0) {
            layer.msg("请输入内容", { icon: 5 });
            return;
        }
        $.post2('/content/huifu', { uid: uid, userId: userId, text: text }, function (data) {
            layer.msg('评论成功', { icon: 6 });
        })
    });

}


function key(value) {
    value = value.replace(/\D/gi, "")
    $('#tiaozhuanpage').val(value);

}


function GetPage() {
    var a = $('#tiaozhuanpage').val();
    return a;
}


//替换‘<’,‘>’符号
function TiHuanStr(str) {
    if (str.length > 0) {
        str = str.replace(/</g, '&lt;');
        str = str.replace(/>/g, '&gt;');
    }
    return str;
}