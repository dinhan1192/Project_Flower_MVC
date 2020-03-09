var strUrl = $('#strImageUrl').val();
var myWidget = cloudinary.createUploadWidget({
    cloudName: 'debutwyfp',
    uploadPreset: 'ml_default'
}, (error, result) => {
    if (!error && result && result.event === "success") {
        //console.log('Done! Here is the image info: ', result.info.secure_url);
        strUrl = strUrl + ',' + result.info.secure_url;
        $('#strImageUrl').val(strUrl);
    }
}
)

document.getElementById("upload_widget").addEventListener("click", function () {
    debugger;
    myWidget.open();
}, false);