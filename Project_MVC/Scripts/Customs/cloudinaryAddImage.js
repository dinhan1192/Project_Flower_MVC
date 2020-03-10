debugger;
var strUrl = $('#strImageUrl').val();
var arrUrl = [];
if (strUrl != null && strUrl != '') {
    arrUrl = strUrl.substring(1, strUrl.length - 1).split(',');
}

var i = arrUrl.length;

document.getElementById("filesCount").innerHTML = i + ' files';
var myWidget = cloudinary.createUploadWidget({
    cloudName: 'debutwyfp',
    uploadPreset: 'ml_default'
}, (error, result) => {
    if (!error && result && result.event === "success") {
        //console.log('Done! Here is the image info: ', result.info.secure_url);
        strUrl = strUrl + ',' + result.info.secure_url;
        $('#strImageUrl').val(strUrl);
        i++;
        $('#filesCount').val(i)
        document.getElementById("filesCount").innerHTML = i + ' files';
    }
}
)

document.getElementById("upload_widget").addEventListener("click", function () {
    debugger;
    myWidget.open();
}, false);