

(function ($) {
    $.fn.Paging2 = function (options) {
        var opts = $.extend({}, { elm: this }, defaults, options)
        opts.OnInit();
        return opts;
    };
   
    var defaults = {

        Url: null,     //接口地址
        SearchForm: ".SearchForm",  //搜索的表单，通过监听提交事件来触发搜索
        page2: ".page2",     //分页按钮等分页信息 
        PagingItemTemplateSelector: ".PagingItemTemplate", //分页项模版
        PagingItemTemplate: null,
        PagingItemTemplatePlaceholder1: [],
        PagingItemTemplatePlaceholder2: [],     //这个是不带井号的
        BindDataType: 0,     //0=默认，基本的替换TableBodyHTML；1=使用模版替换字符串
        IsCssDebug: false,
        searchForm_elm: null,
        pagesize: 10,
        pagecount: 0,
        EndPageNum: 0,   //最后一页有多少个
        index: 1,
        Total: -1,   //-1还未查询。
        TableBox: ".tbody",    //表格容器，存储查询结果
        PagingHtml: "<div class='pager'>总计<span class=\"CountPage\">0</span>页，当前第<span class=\"orpage index\">0</span>页 | <button class=\"pagenum first_page\" type='text'>   第一页   <i class=\"icon-i55\"></i></button> <button class=\"pagenum last\" type='text'>  上一页   <i class=\"icon-i55\"></i></button> <button class=\"pagenum next\" type='text'>   下一页<i class=\"icon-i54\"></i></button>  <button class=\"pagenum last_page\" type='text'>  末 页<i class=\"icon-i55\"></i></button></div>",
        //PagingHtml: "<span class=\"orpage index\">0</span><span>/</span><span class=\"CountPage\">0</span><button class=\"pagenum last disabled\"><i class=\"icon-i55\"></i></button><button class=\"pagenum next disabled\"><i class=\"icon-i54\"></i></button><input type=\"text\" class=\"txt_index\"><a class=\"greenbtn1 btn_go\">跳转</a>",
        Searchdata: null,
        OnInit: function () {
            //搜索表单
            this.searchForm_elm = $(this.elm).find(this.SearchForm);
            this.searchForm_elm.submit({ Paging: this }, this.OnSearchSubmit);
            this.Url = this.Url ? this.Url : this.searchForm_elm.attr("action");
            this.InitTempData();
            //分页按钮
            $(this.elm).find(this.page2).html(this.PagingHtml);
            $(this.elm).find(".last").click({ Paging: this, type: "last" }, this.TurnToPage);
            $(this.elm).find(".next").click({ Paging: this, type: "next" }, this.TurnToPage);
            $(this.elm).find(".btn_go").click({ Paging: this, type: "go" }, this.TurnToPage);

            $(this.elm).find(".first_page").click({ Paging: this, type: "first_page" }, this.TurnToPage);
            $(this.elm).find(".last_page").click({ Paging: this, type: "last_page" }, this.TurnToPage);

            $(this.elm).find(".txt_index").keypress({ Paging: this, type: "Enter_index" }, this.TurnToPage);
            $(this.elm).find(".txt_index").keyup(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            this.searchForm_elm.submit();
        },
        InitTempData:function(){
            this.PagingItemTemplate = $(this.elm).find(this.PagingItemTemplateSelector).html();
            if (this.PagingItemTemplate) {
                this.PagingItemTemplatePlaceholder1 = this.PagingItemTemplate.match(/#(.*?)#/g);
                for (var i = 0; i < this.PagingItemTemplatePlaceholder1.length; i++) {
                    var $t = this.PagingItemTemplatePlaceholder1[i];
                    this.PagingItemTemplatePlaceholder2[i] = $t.substr(1, $t.length - 2);
                }
            }
        },
        OnSearchSubmit: function (e) {
            e.preventDefault();
            var $Paging = e.data.Paging;
            $Paging.Total = -1;
            $Paging.index = 1;
            $Paging.OnSearch();
            return false;
        },
        OnSearch: function ($Paging, IsRefresh) {
            $Paging = $Paging || this;
            if ($Paging.Total == -1) {
                if (!IsRefresh) {
                    $Paging.Searchdata = GetSearchStr($Paging.searchForm_elm);
                    $Paging.Searchdata.pagesize = $Paging.pagesize;
                }
                $Paging.Searchdata.index = $Paging.index == 0 ? 1 : $Paging.index;
                $Paging.Searchdata.Total = $Paging.Total;
            } else {
                $Paging.Searchdata.index = $Paging.index == 0 ? 1 : $Paging.index;
                $Paging.Searchdata.Total = $Paging.Total;
            }
            $.post2($Paging.Url, $Paging.Searchdata, function (data) {
                //数据处理,列表以及分页
                var jsonData = eval('(' + data + ')');
                $Paging.Total = jsonData.Total;
                //判断当前页是否有数据，若没数据，且大于第一页，则再查询一遍且停止本次查询。
                if (jsonData.GetLst != null && jsonData.GetLst.length == 0 && $Paging.index > 1) {
                    $Paging.index--;
                    $Paging.OnSearch();
                    return;
                }
                $Paging.pagecount = parseInt($Paging.Total % $Paging.pagesize == 0 ? $Paging.Total / $Paging.pagesize : ($Paging.Total / $Paging.pagesize) + 1);
                //$($Paging.elm).find($Paging.TableBox).html(jsonData.TableBodyHTML);
                window.scrollTo(0, 0);
                if (jsonData.Total == 0) {
                    $(".NotData").show();
                    //$(".page2").hide();
                    $($Paging.TableBox).hide();
                } else {
                    $(".NotData").hide();
                    //$(".page2").show();
                    $($Paging.TableBox).show();
                }
                $Paging.BindData($Paging, jsonData.TableBodyHTML, jsonData.GetLst);
                SetPaging($Paging, jsonData);
            });
        },
        BindData: function ($Paging, TableBodyHTML, Lst) {
            if ($Paging.BindDataType == 0) {
                $($Paging.elm).find($Paging.TableBox).html(TableBodyHTML);
            } else if ($Paging.BindDataType == 1) {
                var TableBodyHTML = "";
                if (Lst != null) {
                    for (var i = 0; i < Lst.length; i++) {
                        var $t = $Paging.PagingItemTemplate + "";
                        for (var j = 0; j < $Paging.PagingItemTemplatePlaceholder1.length; j++) {
                            $t = $t.replace($Paging.PagingItemTemplatePlaceholder1[j], eval("Lst[i]." + $Paging.PagingItemTemplatePlaceholder2[j]));
                        }
                        TableBodyHTML += $t;
                    }
                }
                $($Paging.elm).find($Paging.TableBox).html(TableBodyHTML);
            }
            $Paging.BindDataEnd($Paging.Total);
        },
        BindDataEnd: function (Total) {

        },
        Refresh: function () {
            this.Total = -1;
            this.OnSearch(this, true);
        },
        TurnToPage: function (e) {
            var $Paging = e.data.Paging;
            var type = e.data.type;

            var index = $Paging.index;
            if (type == "last") {
                if ($($Paging.elm).find(".last").hasClass("disabled")) {
                    return;
                }
                index -= 1;
            } else if (type == "next") {
                if ($($Paging.elm).find(".next").hasClass("disabled")) {
                    return;
                }
                index += 1;
            } else if (type == "first_page") {
                if ($($Paging.elm).find(".first_page").hasClass("disabled")) {
                    return;
                }
                index = 1;
            } else if (type == "last_page") {
                if ($($Paging.elm).find(".last_page").hasClass("disabled")) {
                    return;
                }
                index = $Paging.pagecount;
            }
            else if (type == "go") {
                var indextxt = $($Paging.elm).find(".txt_index").val();
                if (!indextxt || isNaN(indextxt)) return;
                index = parseInt(indextxt);
            } else if (type == "Enter_index") {
                if (e.which == 13) {
                    var indextxt = $($Paging.elm).find(".txt_index").val();
                    if (!indextxt || isNaN(indextxt)) return;
                    index = parseInt(indextxt);
                } else {
                    return;
                }
            }
            if (index < 1 || index > $Paging.pagecount) {
                alert("没有此页码");
                return;
            } else if (index == $Paging.index) {
                alert("当前已经是显示第" + index + "页");
                return;
            }
            $Paging.index = index;
            $Paging.OnSearch($Paging);
        }
    };
    function GetSearchStr(searchForm_elm) {
        var arr = searchForm_elm.serializeArray();
        var str = "";
        for (var i = 0; i < arr.length; i++) {
            str += arr[i].name + ":'" + arr[i].value + "',";
        }
        if (str.length > 0) {
            str = str.substr(0, str.length - 1);
        }
        var jsondata = eval("({" + str + "})");
        return jsondata;
    }
    function SetPaging($Paging, jsonData) {
        var index = $Paging.Searchdata.index;
        var Total = $Paging.Total;
        var pagesize = $Paging.pagesize;
        var CountPage = $Paging.pagecount;

        $(".cont_page").val(CountPage);
         
        //上一页
        if (index <= 1) {
            $($Paging.elm).find(".last").addClass("disabled");
            $($Paging.elm).find(".first_page").addClass("disabled");
            
        } else {
            $($Paging.elm).find(".last_page").removeClass("disabled");
            $($Paging.elm).find(".last").removeClass("disabled");
        }

        //下一页
        if (index >= CountPage) {
            $($Paging.elm).find(".next").addClass("disabled");
            $($Paging.elm).find(".last_page").addClass("disabled");
        }  
        else {
            $($Paging.elm).find(".first_page").removeClass("disabled");
            $($Paging.elm).find(".next").removeClass("disabled");

        }
        $($Paging.elm).find(".index").html(CountPage == 0 ? 0 : index);
        $($Paging.elm).find(".CountPage").html(CountPage);
        $($Paging.elm).find(".txt_index").val(CountPage == 0 ? 0 : index);
    }
})(jQuery);