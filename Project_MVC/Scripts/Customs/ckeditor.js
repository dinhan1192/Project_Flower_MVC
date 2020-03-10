debugger;
ClassicEditor
    .create(document.querySelector('#editor'), {
        // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
    })
    .then(editor => {
        window.editor = editor;
        console.log(editor);
    })
    .catch(err => {
        console.error(err.stack);
    });

$("#editorDescription").change(function () {
    debugger;
    var value = document.getElementById("editorDescription").innerHTML;
    $("Description").val(value);
});
