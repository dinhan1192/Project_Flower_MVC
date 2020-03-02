
$(document).ready(function () {
    var $input = $('.typeahead');
    //var u = $input[i].data('request-url');
    $input.each(function () {
        debugger;
        var link = $(this).data('request-url');
        $(this).typeahead({
            autoSelect: true,
            items: 3,
            source: function (query, process) {
                $.ajax({
                    url: link,
                    type: 'GET',
                    success: function (response) {
                        //debugger;
                        //var u = links[i];
                        $.each(response, function () {
                            this.name = this.Code + ' - ' + this.Name;
                        });
                        return process(response);
                    }
                })
            }
        });

        $(this).change(function () {
            debugger;
            var current = $(this).typeahead("getActive");
            if (current) {
                var id = $(this).data('type');
                if (id === null) {
                    id = '';
                }
                if (current.name == $(this).val()) {
                    debugger;
                    $('#hidCode' + id).val(current.Code);
                    //console.log(current.Code)
                }
            }
        })
    })
    //$input[i].typeahead({
    //    name: id,
    //    autoSelect: true,
    //    items: 3,
    //    source: function (query, process) {
    //        debugger;
    //        var link = $input[i].typeahead.name;
    //        $.ajax({
    //            url: link,
    //            type: 'GET',
    //            success: function (response) {
    //                //debugger;
    //                //var u = links[i];
    //                $.each(response, function () {
    //                    this.name = this.Code + ' - ' + this.Name;
    //                });
    //                return process(response);
    //            }
    //        })
    //    }
    //});

    //if (id === "01") {
    //    strType = "01";
    //} else if (id === "02") {
    //    strType = "02";
    //}

    //$input[i].change(function () {
    //    var current = $(this).typeahead("getActive");
    //    if (current) {
    //        if (current.name == $(this).val()) {
    //            $('#hidCode' + id).val(current.Code);
    //            //console.log(current.Code)
    //        }
    //    }
    //})
})