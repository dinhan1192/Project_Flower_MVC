$(document).ready(function () {
    $("#parentList").change(function () {
        var parameterName = $(this).val();
        var url = $(this).attr("data-request-url");
        var firstItem = $(this).attr("data-first-item");

        $.getJSON(url, { name: parameterName },
            function (data) {
                var select = $("#childList");
                select.empty();
                select.append($('<option/>', {
                    value: 0,
                    text: firstItem
                }));
                $.each(data, function (index, itemData) {
                    select.append($('<option/>', {
                        value: itemData.Value,
                        text: itemData.Text
                    }));
                });
            });
    });
});
