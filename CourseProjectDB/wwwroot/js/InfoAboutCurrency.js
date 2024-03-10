var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/currency/getall' },
        "columns": [
            { data: 'name', width: "20%" },
            { data: 'askedCoursePriceTo', width: "25%" },
            { data: 'availOfAskedCourse', width: "30%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/currency/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pen"></i></a>
                        <a onClick=Delete('/admin/currency/delete/${data}') class="btn btn-primary mx-2 btn-danger"> <i class="bi bi-trash3"></i></a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Ви впевнені?",
        text: "Ви не зможете відновити видалине!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Ok"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}