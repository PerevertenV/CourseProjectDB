var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable(
        {
            "ajax": { url: '/customer/purchase/getall' },
            "columns": [
                { data: 'id', width: "20%" },
                { data: 'state', width: "20%" },
                { data: 'infoAboutCurrency.name', width: "20%" },
                { data: 'user.userName', width: "20%" },
                {
                    data: 'id',
                    "render": function (data) {
                        return `<div class="w-75 btn-group" role="group">
                        <a href="/Customer/Purchase/Details?ID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pen"></i></a>
                        </div>`
                    },
                    "width": "15%"
                }
            ]
        });
}