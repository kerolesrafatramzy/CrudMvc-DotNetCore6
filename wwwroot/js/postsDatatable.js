function DeletePost(id) {

    var result = confirm('Are you sure that you want to delete this post?');

    if (result) {
        $.ajax({
            url: '/Posts/delete/' + id,
            success: function () {
                toastr.success('Post deleted');
                setTimeout(function () {// wait for 5 secs(2)
                    location.reload(); // then reload the page.(3)
                }, 5000);

            },
            error: function () {
                toastr.error('Something went wrong!');
            }
        });
    }
};


$(document).ready(function () {
    $("#Posts2").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/postss",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false


        }],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "title", "name": "Title", "autoWidth": true },
            { "data": "published_In", "name": "Published In", "autoWidth": true, "orderable": false },
            {
                "data": "id",
                "width": "150px",
                "orderable": false,
                "render": function (data, type, row) {
                    return '<div><button asp-controller="Posts" asp-action="Edit" asp-route-id="' + data + '" class="btn btn-success">Edit</button> <button class="btn btn-danger" onclick="DeletePost(' + data + ')">Delete</button></div>'
                }
            }
           
        ],
        //"language": {
        //    "zeroRecords": "Sorry we no data for this substance",
        //    "processing": '<i class="fa fa-spinner fa-spin fa-2x fa-fw"></i>'
        //}
    });
});  

