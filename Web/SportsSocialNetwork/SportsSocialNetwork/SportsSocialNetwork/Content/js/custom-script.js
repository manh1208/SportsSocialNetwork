//$(function () {
//    //tool tip
//    $('[data-toggle="tooltip"]').tooltip();

//    //toogle
//    $('[data-toggle="toggle"]').bootstrapToggle();


//    //show loading message when using ajax
//    $(document).ajaxStart(function () {
//        $('#loading-image').fadeIn(40);
//    }).ajaxStop(function () {
//        $('#loading-image').fadeOut(40, function () {
//            $(this).trigger('onFadeOutComplete');
//        });
//    });

//    //date input mask
//    $(".date-mask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });

//    //date picker
//    $('.date-picker').daterangepicker({
//        singleDatePicker: true,
//        showDropdowns: true
//    });

//    //icheck
//    $('.icheck').iCheck({
//        checkboxClass: 'icheckbox_square-blue',
//        radioClass: 'iradio_square-blue',
//        increaseArea: '20%' // optional
//    });

//    //icheck
//    $('.icheck.green').iCheck({
//        checkboxClass: 'icheckbox_square-green',
//        radioClass: 'iradio_square-green',
//        increaseArea: '20%' // optional
//    });
//    $('.navbar .sidebar-toggle').click(function () {
//        if ($(window).width() > 760 && $(window).width() < 980 && !$("body").hasClass('sidebar-open')) {
//            $('body').addClass('sidebar-open');
//        } else if ($("body").hasClass('sidebar-open') && $(window).width() > 980) {
//            $('body').removeClass('sidebar-open');
//        }
//    })
//})

$(document).ready(function () {
    var offset = 220;
    var duration = 500;
    $(window).scroll(function () {
        if ($(this).scrollTop() > offset) {
            $('.ks-back-to-top').fadeIn(duration);
        } else { //www.crawlist.blogspot.com
            $('.ks-back-to-top').fadeOut(duration);
        }
    });

    $('.ks-back-to-top').click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    })
});


/*==================== Sweet alert =====================*/
function showMessage(text, type, tbnText) {
    var color = "#00a65a";
    if (type === "info") {
        color = "#8cd4f5";
    } else if (type === "error") {
        color = "#dd4b39";
    }
    swal({
        title: "",
        text: text,
        type: type,
        showCancelButton: false,
        confirmButtonColor: type,
        confirmButtonText: tbnText,
        closeOnConfirm: true,
        closeOnCancel: true,
        animation: false
    });
}

function showConfirmDeleteItem(text, callback) {
    swal({
        title: "",
        text: text,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3c8dbc",
        confirmButtonText: "Xóa",
        cancelButtonText: "Không",
        closeOnConfirm: false,
        closeOnCancel: true,
        animation: false
    }, callback);
}

function showConfirmMessage(text, callback) {
    swal({
        title: "",
        text: text,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3c8dbc",
        confirmButtonText: "Đồng ý",
        cancelButtonText: "Hủy bỏ",
        closeOnConfirm: false,
        closeOnCancel: true,
        animation: false
    }, callback);
}

function showPrompt(title, text, placeholder, callbackk) {
    swal({
        title: title,
        text: text,
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: false,
        inputPlaceholder: placeholder,
    }, callbackk);
}


/* ========================= dataTable ================ */
var dataTable_vi = {
    "decimal": "",
    "emptyTable": "Không có dữ liệu",
    "info": "Hiển thị _START_ đến _END_ trong tổng số _TOTAL_ dòng",
    "infoEmpty": "Hiển thị 0 đến 0 trong tổng số 0 dòng",
    "infoFiltered": "(lọc từ _MAX_ dòng)",
    "infoPostFix": "",
    "thousands": ",",
    "lengthMenu": "Hiện  _MENU_ dòng",
    "loadingRecords": "Đang tải...",
    "processing": "Đang xử lý...",
    "search": "Tìm kiếm",
    "zeroRecords": "Không tìm thấy dữ liệu",
    "paginate": {
        "first": "Về đầu",
        "last": "Về cuối",
        "next": "Trang kế",
        "previous": "Trang trước"
    },
    "aria": {
        "sortAscending": ": sắp xếp từ nhỏ đến lớn",
        "sortDescending": ": sắp xếp từ lớn đến nhở"
    }
};

function useDatatable(selector, url, columns, columnDefs) {
    return $(selector).DataTable({
        "filter": true,
        "retrieve": true,
        "serverSide": true,
        "scrollCollapse": true,
        "ajax": {
            "type": "POST",
            "url": url,
            "contentType": 'application/json; charset=utf-8',
            'data': function (data) { return data = JSON.stringify(data); }
        },
        "language": dataTable_vi,
        "processing": true,
        "columnDefs": columnDefs,
    });
}

$.fn.customDataTable = function (param) {
    $(this).DataTable({
        "bFilter": true,
        "bRetrieve": true,
        "bServerSide": true,
        "bScrollCollapse": true,
        "sAjaxSource": param.url,
        "sServerMethod": "POST",
        "bProcessing": true,
        "autoWidth": true,
        "language": dataTable_vi,
        "fnServerParams": function (aoData) {
            for (var i = 0; i < param.data.length; i++) {
                aoData.push({ "name": param.data[i].name, "value": param.data[i].value });
            }
        },
        "aoColumnDefs": param.colDefs,
        "bAutoWidth": false,
    });
}

$.fn.reloadDataTable = function () {
    var t = $(this).DataTable();
    t.ajax.reload(null, false);
}

var bootgrid_vi = {
    noResults: "Không có kết quả",
    all: "Tất cả",
    infos: "Hiển thị {{ctx.start}} đến {{ctx.end}} trong tổng số {{ctx.total}} dòng",
    loading: "Đang tải...",
    refresh: "Tải lại",
    search: "Tìm kiếm"
}




/*==================== Utils =====================*/
//require input number
$.fn.requireNumber = function () {
    $(this).keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
}

//remove unicode
var removeUnicode = function (str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, "-");

    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
    str = str.replace(/^\-+|\-+$/g, "");

    return str;
}

//genrate SEO name
var generateSeoTitle = function (origin) {
    var output = removeUnicode(origin);

    output = output.replace(/[^a-zA-Z0-9]/g, ' ').replace(/\s+/g, "-").toLowerCase();
    // remove first dash
    if (output.charAt(0) === '-') output = output.substring(1);
    // remove last dash
    var last = output.length - 1;
    if (output.charAt(last) === '-') output = output.substring(0, last);

    // Max Length: 255
    if (output.length > 255) {
        output = output.substr(0, 255);
    }

    return output;
}
// SEO
var activateSeoGenerator = function () {
    $(".seo-source[data-seo-target]").change(function () {
        var e = $(this);
        var target = e.attr("data-seo-target");
        e.closest("form").find(target).val(generateSeoTitle(e.val()));
    });
}

//serialize object in form
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//format date
JSON.parseWithDate = function (data) {
    return JSON.parse(data, function (key, value) {
        if (typeof value === 'string') {
            var d = /\/Date\((\d*)\)\//.exec(value);
            return (d) ? new Date(+d[1]) : value;
        }
        return value;
    });
};
// todate js
function j2d(jsonDateString) {
    var d = /\/Date\((\d*)\)\//.exec(jsonDateString);
    return (d) ? new Date(+d[1]) : jsonDateString;
}

//show preview image
function showPreviewImage(input, id) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + id).attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
    try {
        $('#RemoveImg').val("false");
    } catch (e) {
        console.log(e.message);
    }
}

// show errors from server 
function showErrors(o) {
    var err = "";
    for (var error in o) {
        if (o.hasOwnProperty(error)) {
            o[error].forEach(function (item, index) {
                err += item + "\n";
            });
        }
    }
    showMessage(err, "error", "OK");
}