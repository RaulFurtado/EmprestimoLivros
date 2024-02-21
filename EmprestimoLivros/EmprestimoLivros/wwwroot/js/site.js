$(document).ready(function () {
    $('#Emprestimos').DataTable();

    setTimeout(function () {
        $(".alert").fadeOut("slow", function () {
            $(this).alert('close');
        })
    }, 2000)
});

