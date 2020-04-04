$(document).ready(function () {
    function addRequestVerificationToken(data) {
        data.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();
        return data;
    };
    debugger;
    var arrDelete = document.querySelectorAll(".btn-delete-notify");
    $.each(arrDelete, function (i, val) {
        $(this).click(function () {
            debugger;
            var link = $(this).attr('data-request-url');
            var id = $(this).attr("id");
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.confirm({
                title: 'Delete item!',
                content: 'Are you sure want to delete this item ?',
                buttons: {
                    confirm: function () {
                        $.ajax({
                            url: link + id,
                            type: "POST",
                            data: {
                                __RequestVerificationToken: token
                            },
                            success: function (data) {
                                window.location.reload();
                            },
                            error: function (error) {
                                $.alert("Action fails! Please try again!");
                            }
                        });
                        //$.ajax({
                        //    sync: true,
                        //    url: link,
                        //    type: "POST",
                        //    //data: {
                        //    //    __RequestVerificationToken: token
                        //    //},
                        //    data: addRequestVerificationToken({ "id": id }),
                        //    //data: '{__RequestVerificationToken: "'+ token +'", id: "' + id + '" }',
                        //    contentType: "application/json; charset=utf-8",
                        //    dataType: "json",
                        //    success: function (data) {
                        //        debugger;
                        //        if (data == true) {
                        //            window.location.reload();
                        //        }
                        //    },
                        //    error: function (error) {
                        //        $.alert("Action fails! Please try again!");
                        //        //$.alert(error);
                        //    }
                        //});
                    },
                    cancel: function () {
                        $.alert('Canceled!');
                    },
                }
            });
        });
    });
})
 //$.confirm({
            //    async: false,
            //    title: 'Delete item!',
            //    content: 'Are you sure want to delete this item ?',
            //    buttons: {
            //        confirm: function () {
            //            debugger;
            //            $.ajax({
            //                url: link + id,
            //                type: "POST",
            //                data: {
            //                    __RequestVerificationToken: token
            //                },
            //                //data: '{__RequestVerificationToken: "' + token + '", id: "' + id + '" }',
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                success: function (data) {
            //                    window.location.reload();
            //                },
            //                error: function (error) {
            //                    $.alert("Action fails! Please try again!");
            //                }
            //            });
            //        },
            //        cancel: function () {
            //            $.alert('Canceled!');
            //        }
            //    }
            //});
 //if (confirm("Are you sure want to delete this item ?")) {
        //    var token = $('input[name="__RequestVerificationToken"]').val();
        //    $.ajax({
        //        url: link + id,
        //        type: "POST",
        //        data: {
        //            __RequestVerificationToken: token
        //        },
        //        success: function (data) {
        //            alert("Delete success!");
        //            window.location.reload();
        //        },
        //        error: function (error) {
        //            alert("Action fails! Please try again!");
        //        }
        //    });
        //}
        //return false;


//$(document).ready(function () {
//    console.log("123");
//    var valueOfThisDocument = document.getElementsByName('deleteNotifyButton')[0].value;
//    var link = valueOfThisDocument.split(' ')[1];
//    var className = valueOfThisDocument.split(' ')[0];
//    var variable = valueOfThisDocument.split(' ')[2];
//    $(className).click(function () {
//        var id = $(this).attr("id").replace("delete-", "");
//        if (confirm("Are you sure want to delete this " + variable + " ?")) {
//            var token = $('input[name="__RequestVerificationToken"]').val();
//            $.ajax({
//                url: link + id + this.getElementById(''),
//                type: "POST",
//                data: {
//                    __RequestVerificationToken: token
//                },
//                success: function (data) {
//                    alert("Delete success!");
//                    window.location.reload();
//                },
//                error: function (error) {
//                    alert("Action fails! Please try again!");
//                }
//            });
//        }
//        return false;
//    });
//})