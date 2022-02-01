var $userRoleId;
var $userId;
var $selectedObject;
var $tableUsers;

$(document).ready(function () {


    if ($.fn.DataTable.isDataTable('#table_users')) {

        $('#table_users').dataTable().fnDestroy();
        $('#table_users').dataTable().empty();

    }

  $tableUsers = $('#table_users').DataTable(

        {
            "bInfo": false,
            "dom": "<'#menutop.row'<'col-sm-6'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#menubottom.row'<'col-sm-5'l><'col-sm-7'p>>",

            "ajax":
            {
                "url": usersUrl.url_listofusers,
                "type": "GET",
                "datatype": "json"
            },

            "pagingType": "full_numbers",
           

         
         

            "columnDefs": [
                {
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'width': '5%',
                    'className': 'select-checkbox',
                    'data': null,
                    'defaultContent': ''
                },
                { 'width': '25%', 'targets': 1, "data": "username" },
                { 'width': '28%', 'targets': 2, "data": "email" },
                { 'width': '30%', 'targets': 3, "data": "roles" }
                //{
                //    'width': '12%', 'targets': 4, "data": "userId", "orderable": false, "searchable": false, "className": "text-center",
                //    "mRender": function (data) {

                //        return '<button class="btn btn-flat bg-purple btn-xs get_userroles" id=' + data +
                //            '><i class="fa fa-cog fa-sm"></i> Roles </button>';
                //    }
                //}
                

            ],
            select: {
                style: 'os',
                selector: 'td:first-child'

            }
            ,
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-user_id', data.userId);
            }
            ,
            buttons:
            [
                {
                    text: '<i class="fa fa-user-plus fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnnewuser',
                    titleAttr: 'Add a new user',
                    action: function () {



                    }
                }
                ,
                {
                    text: '<i class="fa fa-user-secret fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnmodifyuser',
                    titleAttr: 'Modify user details',
                    action: function () {

                        onModifyUser();

                    }, enabled: false
                }
                
                ,
                {
                    text: '<i class="fa fa-user-times fa-fw"></i>',
                    className: 'btn btn-danger btn-sm btnremoveuser',
                    titleAttr: 'Remove user',
                    action: function () {
                     
                        onRemovingUser();

                    }, enabled: false
                }
                ,
                {
                    text: '<i class="fa  fa-gear fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnadduserRoles',
                    titleAttr: 'Add User Roles',
                    action: function () {
                      
                        onUserAddRoles();

                    }, enabled: false
                }
            ]

        }


    );

    $('#table_users tbody').on('click', 'tr', function (e) {

        e.stopPropagation();

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $tableUsers.button(0).enable();
            $tableUsers.button(1).disable();
            $tableUsers.button(2).disable();
            $tableUsers.button(3).disable();

        } else {

            $tableUsers.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $tableUsers.button(0).disable();
            $tableUsers.button(1).enable();
            $tableUsers.button(2).enable();
            $tableUsers.button(3).enable();

            $selectedObject = $(this);

        }
    });


    var onRemovingUser = function() {

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            $userId = $this.attr("data-user_id");

          
            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This User..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed Operation!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: usersUrl.url_removeUser,
                        datatype: 'json',
                        data: { id: $userId },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully removed!",
                                        type: "success"
                                  

                                    });


                                //$tableUsers.DataTable().ajax.reload();
                                $('#table_users').DataTable().ajax.reload();

                                setTimeout(function () {
                                    $tableUsers.button(0).enable();
                                    $tableUsers.button(1).disable();
                                    $tableUsers.button(2).disable();
                                    $tableUsers.button(3).disable();

                                }, 600);


                            }
                        }
                    });



                }
            });
         
        }

    };


    var onUserAddRoles = function() {
        


        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            $userId = $this.attr("data-user_id");


            $.ajax({
                type: 'Get',
                url: usersUrl.url_userroles,
                contentType: 'application/html;charset=utf8',
                datatype: 'html',
                data: { id: $userId },
                cache: false,
                success: function (result) {
                    var modal = $('#modal-users');
                    modal.find('#modalcontent').html(result);

                    var tblusersRole = $('#tbl-userRole').DataTable({ bLengthChange: false, bFilter: false });

                    tblusersRole.columns.adjust();

                    modal.modal({
                            backdrop: 'static',
                            keyboard: false
                        },
                        'show');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire('Error adding record!', 'Please try again', 'error');
                }


            });


        }

      

    };


    if ($.fn.dataTable.isDataTable('#tbl-userRole')) {


        $('#tbl-userRole').DataTable().destroy();
        $('#tbl-userRole tbody').empty();

    }

    //var $user_Id = $('#userId').attr('id');


    var tblusersRole = $('#tbl-userRole').DataTable({
        serverSide: false,
        destroy: true,
        responsive: true,
        bLengthChange: false,
        bFilter: false
        //ajax: {
        //    url: usersUrl.url_userrolesDataTable,
        //    type: "Get",
        //    datatype: "json"
        //},
       
        //"columnDefs":
        //[
        //    {
        //        'autowidth': true, 'targets': 0,
        //        'data': "Name"

        //    }
        //]
    });



});//---- end doc ready

$(document).on('click', '.btnnewuser', function (e) {
    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: usersUrl.url_addnewuser,
        // url:'http://localhost:Sals/Payments/Add_PaymentPartialView',
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {
            var modal = $('#modal-users');
            modal.find('#modalcontent').html(result);

            modal.modal({
                backdrop: 'static',
                keyboard: false
            },
                'show');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error adding record!', 'Please try again', 'error');
        }


    });

});

$(document).on('click', '#registerUser', function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Adding User..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'

    }).then((result) => {

        if (result.value) {

            $('#spinn-loader').show();

            var formUrl = $('#registerFormUser').attr('action');
            var form = $('[id*=registerFormUser]');
            $.validator.unobtrusive.parse(form);
            form.validate();

            if (form.valid()) {

                $.ajax({
                    type: 'POST',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            //console.log(data.url);
                            Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully Added!",
                                    type: "success"
                         

                                });

                            $('#table_users').DataTable().ajax.reload();

                            setTimeout(function () {
                                $('#table_users').button(0).enable();
                                $('#table_users').button(1).disable();
                                $('#table_users').button(2).disable();
                                $('#table_users').button(3).disable();

                                $("#modal-users").modal('hide');
                              

                            }, 600);

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }
                });

            }
            else {

                $.each(form.validate().errorList, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                    $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                    $errorSpan.show();
                });

            }


        }
    });


});

$(document).on('click','.get_userroles',function(e) {
    e.preventDefault();

    $userId = $(this).attr("id");
    
    $.ajax({
        type: 'Get',
        url: usersUrl.url_userroles,
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        data:{id:$userId},
        cache: false,
        success: function (result) {
            var modal = $('#modal-users');
            modal.find('#modalcontent').html(result);

            var tblusersRole = $('#tbl-userRole').DataTable({ bLengthChange: false, bFilter: false });

            tblusersRole.columns.adjust();

            modal.modal({
                    backdrop: 'static',
                    keyboard: false
                },
                'show');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error adding record!', 'Please try again', 'error');
        }


    });
});


$(document).on('click',
    '#addroletouser',
    function(e) {

        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Adding Role to User..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation!'

        }).then((result) => {

            if (result.value) {

                $('#spinn-loader').show();

                var formUrl = $('#formaddroletoUser').attr('action');
                var form = $('[id*=formaddroletoUser]');

                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    $.ajax({
                        type: 'POST',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                //console.log(data.url);
                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully Added!",
                                        type: "success"
                             

                                    });

                                $('#table_users').DataTable().ajax.reload();

                                setTimeout(function () {

                                        $tableUsers.button(0).enable();
                                        $tableUsers.button(1).disable();
                                        $tableUsers.button(2).disable();
                                        $tableUsers.button(3).disable();

                                    },
                                    600);


                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });

                } else {

                    $.each(form.validate().errorList,
                        function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                            $errorSpan.show();
                        });

                }


            }
        });




    });

$(document).on('click', '.remove_user_role', function (e) {
    e.preventDefault();
  
    var $userRoleId = $(this).attr('data-role-id');
    var username = $(this).attr('data-user-id');
 
    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing This Customer..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Operation!'

    }).then((result) => {

        if (result.value) {

            $.ajax({
                type: "post",
                url: usersUrl.url_removeUserRole,
                ajaxasync: true,
                data: { user: username, roleId: $userRoleId },
                cache: false,
                success: function (data) {

                    if (data.success) {

                        Swal.fire({
                                title: "Success",
                                text: "It was succesfully removed!",
                                type: "success"
                        

                            });


                        $('#table_users').DataTable().ajax.reload();

                        setTimeout(function () {

                                $tableUsers.button(0).enable();
                                $tableUsers.button(1).disable();
                                $tableUsers.button(2).disable();
                                $tableUsers.button(3).disable();

                            },
                            600);

                    }
                    else {

                        Swal.fire('Unable to remove record!', 'Please try again', 'error');


                    }
                }
            });//-----ajax end


        }
    });
});