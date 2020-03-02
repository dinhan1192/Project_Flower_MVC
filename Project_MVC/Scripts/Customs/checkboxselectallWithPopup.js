var lstLink = document.querySelectorAll('a[name="addItemName"]');
var strType = "";
for (i = 0; i < lstLink.length; i++) {
    var id = lstLink[i].getAttribute("data-type");
    if (id === "AtView") {
        strType = "AtView";
    } else if (id === "AtPopup") {
        strType = "AtPopup";
    }
}
//console.log(strType);

$(document).ready(
    manageCheckboxGroup('chkAffectCheckboxGroup' + strType, 'checkbox-group' + strType)
);

function manageCheckboxGroup(masterCheckboxId, slaveCheckboxesClass) {
    var values = $("#" + "addItem-" + strType).attr('data-parameter');
    $("#" + masterCheckboxId).click(function () {
        $("." + slaveCheckboxesClass).prop('checked', this.checked);
        if (this.checked) {
            var list = document.querySelectorAll("." + slaveCheckboxesClass);
            for (i = 0; i < list.length; i++) {
                var item = list[i].getAttribute("id");
                //console.log(item)
                if (values == "") {
                    values = item;
                } else {
                    values = values + ',' + item;
                }
                //console.log(values);
            }
            //console.log("checked");
        } else if (!this.checked) {
            values = "";
            //console.log(values);
            //console.log("unchecked");
        }
        //console.log(values);
        //console.log(strType);
        var setValues = document.getElementById("addItem-" + strType).setAttribute('data-parameter', values);
        //if (document.getElementById("addItem-" + strType).getAttribute("data-parameter") === "") {
        //    var name01 = document.getElementById("addItem-" + strType).getAttribute("data-parameter");
        //    console.log(name01)
        //    console.log("rỗng !!!")
        //} else {
        //    var name01 = document.getElementById("addItem-" + strType).getAttribute("data-parameter");
        //    console.log(name01)
        //    console.log("Không rỗng !!!")
        //}
    });

    $("." + slaveCheckboxesClass).click(function () {
        var item = $(this).attr("id");
        if (!this.checked) {
            $("#" + masterCheckboxId).prop('checked', false);
            var arrIds = values.split(',');
            arrIds.splice(arrIds.indexOf(item), 1);
            console.log(arrIds.indexOf(item));
            values = arrIds.join(',');
            //console.log(values);
        }
        else if ($("." + slaveCheckboxesClass).length == $("." + slaveCheckboxesClass + ":checked").length) {
            $("#" + masterCheckboxId).prop('checked', true);
        }
        if (this.checked) {
            if (values == "") {
                values = item;
            } else {
                values = values + ',' + item;
            }
            //console.log(values);
        }
        var setValues = document.getElementById("addItem-" + strType).setAttribute('data-parameter', values);
    });
    //console.log(values);
}