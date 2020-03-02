$(function () {
    $(".anchorDetail").click(function () {
        //console.log("123456")
        //debugger;
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-parameter');
        var postBackUrl = $buttonClicked.attr('data-urlPopup');
        console.log(id);
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: postBackUrl,
            contentType: "application/json; charset=utf-8",
            data: { "Id": id },
            datatype: "json",
            success: function (data) {
                $('#myModalContent').html(data);
                $('#myModal').modal(options);
                $('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
        var thisType = $buttonClicked.attr('data-type');
        var setValues = document.getElementById('addItem-' + thisType).setAttribute('data-parameter', "")
    });
    //$("#closebtn").on('click',function(){
    //    $('#myModal').modal('hide');

    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });
});