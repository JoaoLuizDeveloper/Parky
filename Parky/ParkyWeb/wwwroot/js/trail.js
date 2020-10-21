var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Trails/GetAllTrails",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "nationalPark.name", "width": "25%",
                "render": function (data) {
                    return `
                    <div>
                        <div class='text-black' style="color:black;>">
                            ${data}
                        </div>
                    </div>
                    `;
                }
            },
            {
                "data": "name", "width": "20%",
                "render": function (data) {
                    return `
                    <div>
                        <div class='text-black' style="color:black;>">
                            ${data}
                        </div>
                    </div>
                    `;
                }
            },
            {
                "data": "distance", "width": "15%",
                "render": function (data) {
                    return `
                    <div>
                        <div class='text-black' style="color:black;>">
                            ${data}
                        </div>
                    </div>
                    `;
                }
            },
            {
                "data": "elevation", "width": "15%",
                "render": function (data) {
                    return `
                    <div>
                        <div class='text-black' style="color:black;>">
                            ${data}
                        </div>
                    </div>
                    `;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Trails/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/Trails/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "30%"
            }
        ]
    });
}


function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmbuttonText: "Yes, delete!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'Delete',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}
//function Delete(url) {
//    swal({
//        title: "Are you sure you want to Delete?",
//        text: "You will not be able to restore the data!",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true
//    }).then((willDelete) => {
//        if (willDelete) {
//            $.ajax({
//                type: 'DELETE',
//                url: url,
//                success: function (data) {
//                    if (data.success) {
//                        toastr.success(data.message);
//                        dataTable.ajax.reload();
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            });
//        }
//    });
//}