
$(document).ready(function () {

    var $selectedObj = "";

    if ($.fn.DataTable.isDataTable('#table_menu')) {

        $('#table_menu').dataTable().fnDestroy();
        $('#table_menu').dataTable().empty();

    }

    var menulist = $('#table_menu').DataTable(

        {

            "serverSide": true,
            "processing": true,
            "paging": true,
            "bInfo": false,
            //"dom": '<"top"B>rt<"bottom"lp><"clear">',

            "dom": "<'#menutop.row'<'col-sm-6'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#menubottom.row'<'col-sm-5'l><'col-sm-7'p>>",

            "pagingType": "full_numbers",
            "ajax":
            {
                "url": menuUrl.menuurl_loadMenu,
                "type": "POST",
                "datatype": "json"
            },
            "columns": [
                { "data": null },
                { "data": "menu_Id", "name": "menu_Id" },
                { "data": "menudesc", "name": "menudesc" },
                { "data": "coursecategory", "name": "coursecategory" },
                { "data": "departmentincharge", "name": "departmentincharge" },
                {
                    "data": "price", "name": "price",
                    "className": "text-right",
                    render: $.fn.dataTable.render.number(",", ".", 2)
                },
                {
                   "data": "menu_Id", "orderable": false, "searchable": false, "className": "text-center",
                    "mRender": function (data) {

                        return '<button class="btn btn-flat bg-olive btn-xs getImage" id=' + data +
                            '><i class="fa  fa-picture-o fa-sm"></i></button>';
                    }
                }
            ],
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
                { 'width': '10%', 'targets': 1 },
                { 'width': '30%', 'targets': 2 },
                { 'width': '15%', 'targets': 3 },
                { 'width': '15%', 'targets': 4 },
                { 'width': '10%', 'targets': 5 },
                { 'width': '5%', 'targets': 6 }
            ],
            "order": [1, 'asc'],

            select: {
                style: 'os',
                selector: 'td:first-child'

            },

            buttons:
            [
                {
                    text: '<i class="fa fa-plus-square fa-fw"></i>',
                    className: 'btn btn-primary btnAddMenu',
                    titleAttr: 'Add a new record',
                    action: function (dt, node, config) {

                        window.location.href = menuUrl.menuurl_add;

                    }
                },
                {
                    text: '<i class="fa fa-edit fa-fw"></i>',
                    className: 'btn btn-primary btnEditMenu',
                    titleAttr: 'Modify record',
                    action: function (e, dt, node, config) {
                        onEditMenu();
                    }
                    , enabled: false
                },
                {
                    text: '<i class="fa fa-trash fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveMenu',
                    titleAttr: 'Remove record',
                    action: function (e, dt, node, config) {

                        onremoveMenu();

                    }
                    , enabled: false
                }
            ]



        }

    );

    //============================= end of code =================================

    $('#table_menu tbody').on('click', 'tr', function (e) {

        e.stopPropagation();



        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            menulist.button(0).enable();
            menulist.button(1).disable();
            menulist.button(2).disable();

        }

        else {

            menulist.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            menulist.button(0).disable();
            menulist.button(1).enable();
            menulist.button(2).enable();

            $selectedObj = $(this);
        }

        //console.log($selectedObj);

    });


    $('#btnSearch').click(function () {


        menulist.columns(3).search($('#courseList').val().trim());

        menulist.columns(2).search($('#searchmenu').val().trim());


        menulist.draw();
    });


    $('#btn_savemenu').on('click', function (e) {

        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving New Menu..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!'
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#createMenu').attr('action');
                var form = $('[id*=createMenu]');

                $.validator.unobtrusive.parse(form);
                form.validate();

             

                if (form.valid()) {

                    var formdata = new FormData($('[id*=createMenu]').get(0));

                   
                    $.ajax({
                        type: 'POST',
                        url: formUrl,
                        //data: form.serialize(),
                        //datatype: 'json',
                        data:formdata,
                        contentType: false,
                        processData: false,
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                //console.log(data.url);
                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully Added!",
                                    type: "success",
                                    timer:5000
                                });

                                window.location.href = menuUrl.menuurl_Index;

                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {

                            Swal.fire("Error adding record!", "Please try again", "error");

                            //$('#spinn-loader').hide();
                        }
                    });


                } else {
                    $.each(form.validate().errorList, function (key, value) {
                        $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                        $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                        $errorSpan.show();
                    });
                }
            }
        }); //end swal


    });//end button save


    $('select[name=courseId]').on('change', function () {


        if ($(this).val() != '') {

            //alert('asdas');
            $('#validationcourse').hide();

        }

    });

    $('select[name=deptId]').on('change', function () {


        if ($(this).val() != '') {
            $('#validationdept').hide();

        }

    });

    function onremoveMenu() {

        if ($selectedObj.hasClass('selected')) {

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Menu..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Remove it!'
                //closeOnConfirm: true, closeOnCancel: true
            }).then((result) => {

                if (result.value) {

                    var _menuId = $selectedObj.closest("tr").find('td:eq(1)').text();

                    var row = $selectedObj.closest('tr');
                    var api = $('#table_menu').DataTable();

                    $.ajax({

                        type: 'post',
                        url: menuUrl.menuurl_remove,
                        ajaxasync: true,
                        dataType: 'json',
                        data: { menuId: _menuId },
                        success: function (data) {
                            if (data.success) {

                                Swal.fire({
                                    title: 'Deleted!',
                                    text: data.deletedMenu + ' was deleted successfully.',
                                    type: 'success',
                                    timer: 3000
                                });

                                api.row(row).remove().draw();

                                menulist.button(0).enable();
                                menulist.button(1).disable();
                                menulist.button(2).disable();

                            }

                            else

                            {

                                Swal.fire('Unable to remove', data.deletedMenu + ' is in used \n Please try again or contact sys admin', 'info');


                            }

                         
                        }

                        ,
                        error: function (xhr, status, errorThrown) {

                            // alert(xhr.status);

                            if (xhr.status === 403) {

                                var response = $.parseJSON(xhr.responseText);

                                //  console.log(response);
                                // window.location = response.LogOnUrl;
                                Swal.fire({
                                    title: "UnAuthorized Access",
                                    text: response.Error,
                                    type: "error"

                                });

                            } else {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }
                        }

                    });


                }

            });//end then


        
        }
    }// end remove function



    var onEditMenu = function () {

        if ($selectedObj.hasClass('selected')) {

            var _menuId = $selectedObj.closest("tr").find('td:eq(1)').text();

            window.location.href = menuUrl.menuurl_modify.replace("menucode", _menuId);

        }

    };


    $('#btn_modifymenu').on('click', function (e) {

        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Menu..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Update it!'
 
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#menumodifyform').attr('action');
                var form = $('[id*=menumodifyform]');

                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    var formdata = new FormData($('[id*=menumodifyform]').get(0));

                    $.ajax({
                        type: 'POST',
                        url: formUrl,
                        data: formdata,
                        contentType: false,
                        processData: false,
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                //console.log(data.url);
                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully updated!",
                                        type: "success",
                                        timer:3000

                                    });

                                    window.location.href = menuUrl.menuurl_Index;

                      
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

        });//end then
            

    });//end modify button
   



    var modal = document.getElementById("modalImage");

    //var spanclose = document.getElementsByClassName("menus-close");

    $('#table_menu tbody').on('click', 'tr .getImage',
        function (e) {
            e.preventDefault();


         
            $.ajax({
                type: 'Get',
                url: menuUrl.menuurl_getImage,
                data: { menuid: $(this).attr("id") },
                datatype: 'json',
                success: function (result) {

                    if (result.success) {

                        modal.style.display = "block";

                        $('#img01').attr('src', '/Content/UploadedImages/' + result.upimage);
                       

                    }
                        
                    
                   
                }
            });

          

          
        });


    // When the user clicks on <span> (x), close the modal

    //spanclose.onclick = function () {
    //    modal.style.display = "none";

    //    $('#table_menu').DataTable().ajax.reload();

    //    menulist.button(0).enable();
    //    menulist.button(1).disable();
    //    menulist.button(2).disable();

    //}
 

    //$("#modalImage").on("hidden", function () {
    //    alert('sadas');
    //});

    $('.menus-close').on('click',function(e) {
        e.preventDefault();

        modal.style.display = "none";

        $('#table_menu').DataTable().ajax.reload();

        menulist.button(0).enable();
        menulist.button(1).disable();
        menulist.button(2).disable();

    });
});

//document.getElementById("UpImage").onchange = function () {
//    //alert(this.value);
//    $('p.help-block').html(this.value);
//};

