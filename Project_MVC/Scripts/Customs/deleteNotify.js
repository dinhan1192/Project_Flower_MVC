$(document).ready(function () {
    //debugger;
    //var lstLink = document.querySelectorAll('[name="deleteNotify"]');
    //var strType = "";
    //for (i = 0; i < lstLink.length; i++) {
    //    var id = lstLink[i].getAttribute("data-type");
    //    if (id === "Partial") {
    //        strType = "Partial";
    //    }
    //}
    //console.log("123")
    $('.btn-delete-notify').click(function () {
        debugger;
        var link = $(this).data('request-url');
        var id = $(this).attr("id").replace("delete-", "");
        if (confirm("Are you sure want to delete this item ?")) {
            var token = $('input[name="__RequestVerificationToken"]').val();
            $.ajax({
                url: link + id,
                type: "POST",
                data: {
                    __RequestVerificationToken: token
                },
                success: function (data) {
                    alert("Delete success!");
                    window.location.reload();
                },
                error: function (error) {
                    alert("Action fails! Please try again!");
                }
            });
        }
        return false;
    });
})


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