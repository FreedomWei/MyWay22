

//登录方法
function Login() {
    var username = $("#username").val();
    var pwd = $("#pwd").val();
    var isAuto = $("input[id='isAuto']").is(":checked");
    if (username.length == 0) {
        layer.msg('用户名不能为空',{icon:5});
        return;
    }
    if (pwd.length == 0) {
        layer.msg('密码不能为空', { icon: 5 });
        return;
    }
    $.post('/home/login', { username: username, pwd: pwd, isAuto: isAuto }, function (data) {
        var jsondata = eval("("+data+")");
        if (jsondata.state == "1") {
            $("#exampleModal").modal('hide');
            $("#denglu").html('<a href="#">' + jsondata.username + '</a>');
            $("#zhuce").html('<a href="#" onclick="LoginOut()">退出</a>');
            isLogin = "True";
            userId = jsondata.userId;
        } else {
            layer.alert("用户名或者密码错误", { icon: 6 });
        }
    });
    
}

//注册
function Reg() {
    var regusername = $("#regusername").val();
    var pwd = $("#regpwd").val();
    var pwd1 = $("#regpwd1").val();
    if (regusername.length == 0 || regusername.length < 6) {
        layer.msg('用户名不能为空或小于6位字符', { icon: 5 });
        return;
    }
    if (pwd.length == 0 || pwd.length < 6) {
        layer.msg('密码不能为空或小于6位字符', { icon: 5 });
        return;
    }
    if (pwd != pwd1) {
        layer.msg('两次密码不一致', { icon: 5 });
        return;
    }
    $.post('/home/reg', { regusername: regusername, pwd: pwd }, function (data) {
        if (data == "1") {
            //信息框-例1
            layer.alert('注册成功', { icon: 6 });
            $('#reg').modal('hide');
        } else {
            layer.alert(data, { icon: 5 });
        }
    });
}


//退出
function LoginOut() {
    //询问框
    layer.confirm('确定要退出吗？', {
        btn: ['确定', '取消'] //按钮
    }, function () {
        $.post('/home/LoginOut', {}, function (data) {
            isLogin = "False";
            location.href = "/";
        });
    }, function () {
        
    });
}

GetLabel();
//获取标签云
function GetLabel() {
    $.post2('/home/GetLabel', {}, function (data) {
        var josndata = eval('('+data+')');
        var html = '';
        for (var i = 0; i < josndata.length; i++) {
            html += '<li><a href="/home/LabelYunPage?labelid=' + josndata[i].Id + '&labelName=' + josndata[i].LabelName + '">' + josndata[i].LabelName + '</a></li>';
        }
        $('#biaoqianyun').html(html);
    });
}



//音乐播放暂停控制
//function Play() {
//    var isPlay = document.getElementById('mp3');

//    if(isPlay.paused){
//        isPlay.play();
//        $('#play').html('<span class="glyphicon glyphicon-pause" aria-hidden="true"></span>');
//        $('#play').attr('title', '暂停歌曲');
//    }else{
//        isPlay.pause();
//        $('#play').html('<span class="glyphicon glyphicon-play" aria-hidden="true"></span>');
//        $('#play').attr('title', '播放歌曲');

//    }
//}