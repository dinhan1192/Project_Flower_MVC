//debugger;
ClassicEditor
    .create(document.querySelector('#Description'), {
        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
    })
    .catch(err => {
        console.error(err.stack);
    });

//$("#editorDescription").change(function () {
//    debugger;
//    var value = document.getElementById("editorDescription").innerHTML;
//    $("Description").val(value);
//});

//CKEDITOR.replace("myEditor");
