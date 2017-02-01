


var checkPage = 1;//保存总页数

GetPageList(typeId,1);

function GetPageList(typeId, pageIndex) {
    if (pageIndex == null || pageIndex=="") {
        layer.msg('请输入跳转页数', { icon: 6 });
        return;
    }

    if (pageIndex > checkPage) {
        layer.msg('页码不能超过总页数', { icon: 6 });
        return;
    }
    $.post2('/home/GetConentPageList', { typeId: typeId, pageIndex: pageIndex }, function (data) {
       
        var jsondata = eval('(' + data + ')');
        checkPage = jsondata.pageCount;//赋值给总页数

        //生成数据列表
        var html = "";
        if (jsondata.cList.length > 0) {
        for (var i = 0; i < jsondata.cList.length; i++) {

            html += '<div class="media" style="padding:10px;border-bottom:1px dotted #dddddd;margin-top:0px;">';
            html += '<div class="media-body">';
            html += '<p class="media-heading" style="font-weight:bold;font-size:20px;"><a href="/home/detail?id='+jsondata.cList[i].Id+'">'+jsondata.cList[i].Title+'</a></p>';
            html += jsondata.cList[i].Describe;
            html += '<p style="color:#999;">' + jsondata.cList[i].dateString + '</p>';
            html += '</div>';
            html += '<div class="media-right media-middle">';
            //html += '<img class="media-object" data-src="holder.js/64x64" alt="'+jsondata.cList[i].Title+'" src="'+jsondata.cList[i].ImagePath+'" data-holder-rendered="true" style="width: 80px; height: 80px;">';
            html += '</div></div>';   
        }
        $('#pageList').html(html);

        
        //生成分页按钮
        var htmlPage = '';
        htmlPage += '<li id="shouye"></li>';
        htmlPage += '<li id="shangyiye" style="float:left;"></li> ';
        htmlPage += ' <li style="float:left;padding:0 3px 0 3px;"><select class="form-control" onchange="GetPageList(' + typeId + ',this.value)">';
        for (var i = 1; i <= jsondata.pageCount; i++) {
            //if (i == jsondata.pageThis) {
            //    htmlPage += '<li class="active"><a href="javascript:;" onclick="GetPageList(2,' + i + ');">' + i + '<span class="sr-only">(current)</span></a></li>';
            //} else {
            //    htmlPage += '<li><a href="javascript:;" onclick="GetPageList(2,' + i + ');">' + i + '</a></li>';
            //}
            if (i == jsondata.pageThis) {
                htmlPage += '<option selected value="' + i + '">' + i + '</option>';
            } else {
                htmlPage += '<option value="' + i + '">' + i + '</option>';
            }
        }
        htmlPage += '</select></li> ';

        htmlPage += ' <li id="xiayiye"><a href="javascript:;" aria-label="Next"><span aria-hidden="true">下一页</span></a></li>';
        htmlPage += '<li id="weiye"><a href="javascript:;" onclick="GetPageList(' + typeId + ',' + jsondata.pageCount + ');">尾页</a></li>';
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
            shangyiyehtml.html('<a href="javascript:;" aria-label="Previous" onclick="GetPageList(' + typeId + ',' + shangyiye + ');"><span aria-hidden="true">上一页</span></a>');
            shouye.html('<a href="javascript:;" onclick="GetPageList(' + typeId + ',1);">首页</a>');
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
            xiayiyehtml.html('<a href="javascript:;" aria-label="Next" onclick="GetPageList(' + typeId + ',' + xiayiye + ');"><span aria-hidden="true">下一页</span></a>');
            weiye.html('<a href="javascript:;" onclick="GetPageList(' + typeId + ',' + jsondata.pageCount + ');">尾页</a>');
        }




        //生成跳转页码按钮
        var tiaozhuan = '';
        tiaozhuan += '<span><input class="text text-info" type="text" id="tiaozhuanpage" name="number" value="' + jsondata.pageThis + '" style="width:50px" onkeyup="key(this.value)"/> <button class="btn btn-sm btn-primary" onclick="GetPageList(' + typeId + ',GetPage());"> 跳转 </button></span> ';
        tiaozhuan += '<span>总页数：<strong style="color:blue;">' + jsondata.pageCount + '</strong> 页 </span><span> 总条数：<strong style="color:blue;">' + jsondata.Count + '</strong> 条 </span>';
        $('#tiaozhuan').html(tiaozhuan);
        } else {
            $('#page').html("<p>未找到相关数据</p>");
        }
    })
}




function key(value) {
    value = value.replace(/\D/gi, "")
    $('#tiaozhuanpage').val(value);

}


function GetPage() {
    var a = $('#tiaozhuanpage').val();
    return a;
}

GetLabel();
//获取标签云
function GetLabel() {
    $.post2('/home/GetLabel', {}, function (data) {
        var josndata = eval('(' + data + ')');
        var html = '';
        for (var i = 0; i < josndata.length; i++) {
            html += '<li><a href="/home/LabelYunPage?labelid=' + josndata[i].Id + '&labelName=' + josndata[i].LabelName + '">' + josndata[i].LabelName + '</a></li>';

        }
        $('#biaoqianyun').html(html);
    });
}
