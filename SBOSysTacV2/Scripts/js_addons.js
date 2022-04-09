
var $selectedObject;

$(document).ready(function () {

    if ($.fn.DataTable.isDataTable('#table_addons')) {

        $('#table_addons').dataTable().fnDestroy();
        $('#table_addons').dataTable().empty();

    }
  

    var addonslist = $('#table_addons').DataTable(

    {
        "paging": true,
        "lengthChange": false,
        "pagingType": "simple_numbers",

        "dom": "<'#coursetop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#coursebottom.row'<'col-sm-5'i><'col-sm-7'p>>",

        "ajax":
        {
            "url": addOnsUrl.addons_LoadDataTable,
            "type": "GET",
            "datatype": "json"
        },

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
            { "orderable": true, "targets": 1, "data": "addonId" },
         
            { "orderable": true, "targets": 2, "data": "addoncategory" },
            { "orderable": true, "targets": 3, "data": "AddonsDescription" },
            {
                "orderable": false, 'targets': 4, "data": "AddonAmount",
                "className": "text-right",
                render: $.fn.dataTable.render.number(",", ".", 2)
            },

            { "orderable": false, "targets": 5, "data": "Unit" },
           
            { "width": "3%", "targets": 0 },
            { "width": "6%", "targets": 1 },
            { "width": "25%", "targets": 2 },
            { "width": "10%", "targets": 4}
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        }
        ,
        buttons: [
            {
                text: '<i class="fa fa-plus-square fa-fw"></i>',
                className: 'btn btn-primary btnCreateAddons',
                titleAttr: 'Add a new record',
                action: function (dt, node, config) {

                    onCreateNewAddOns();
                    //window.location.href = courseUrl.urlAdd;
                }
            },
            {
                text: '<i class="fa fa-edit fa-fw"></i>',
                className: 'btn btn-primary btnmodifyAddons',
                titleAttr: 'Modify record',
                action: function (e, dt, node, config) {
                  
                    onmodifyAddons();
                }, enabled: false
            },
            {
                text: '<i class="fa fa-trash fa-fw"></i>',
                className: 'btn btn-primary btnRemoveAddons',
                titleAttr: 'Remove record',
                action: function (e, dt, node, config) {

                    //onremoveCourse();

                }
                , enabled: false
            }
        ]
        ,
        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-addonId', data.addonId);
        }
    }
    ); //============================= table addons end of code =================================

    var onCreateNewAddOns = function () {

        $.ajax({
            type: 'Get',
            url: addOnsUrl.addons_create,
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {
                var modal = $('#modalCreateAddons');
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

    };

    var onmodifyAddons = function() {
       
        debugger;

        if ($selectedObject.hasClass('selected')) {

         
            var $this = $selectedObject;
            var selectedid = $this.attr('data-addonid');

            $.ajax({
                type: 'Get',
                url: addOnsUrl.addons_modify,
                contentType: 'application/html;charset=utf8',
                data: {addonId:selectedid},
                datatype: 'html',
                cache: false,
                success: function (result) {
                    var modal = $('#modalModifyAddonsDetails');
                    modal.find('#modifymodalcontent').html(result);

                    modal.modal({
                            backdrop: 'static',
                            keyboard: false
                        },
                        'show');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire('Error modifying record!', 'Please try again', 'error');
                }


            });
        
           
        }
    };


    $('#table_addons tbody').on('click', 'tr', function () {

        $selectedObject = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            addonslist.button(0).enable();
            addonslist.button(1).disable();
            addonslist.button(2).disable();
        }

        else {
            addonslist.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            addonslist.button(0).disable();
            addonslist.button(1).enable();
            addonslist.button(2).enable();


            $selectedObject = $(this);

/*            console.log($selectedObject);*/
        }


    });



   

    $('.btnRemoveAddons').on('click', function (e) {
        e.preventDefault();

        //alert('asdasd');

        if ($selectedObject.hasClass('selected')) {


            var $this = $selectedObject;

            var selectedid = $this.attr('data-addonId');

            // console.log(selectedcustomerid);

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Addon..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes,Proceed Removing..!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: addOnsUrl.addons_remove,
                        ajaxasync: true,
                        data: { addonId: selectedid },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully removed!",
                                    type: "success"


                                });

                                setTimeout(function () {


                                    $('#table_addons').DataTable().ajax.reload();

                                    addonslist.button(0).enable();
                                    addonslist.button(1).disable();
                                    addonslist.button(2).disable();

                                }, 300);


                            }
                            else {

                                Swal.fire('Unable to remove record!', 'Please try again', 'error');


                            }
                        }
                    });//-----ajax end


                }
            });


        }

    });//=== remove button end



}); //============================= documents end of code =================================



$(document).on('click', '#btn_regaddons', function (e) {
    e.preventDefault();


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Adding New Addons..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#formcreateaddons').attr('action');
            var form = $('[id*=formcreateaddons]');

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

                            $('#table_addons').DataTable().ajax.reload();

                            setTimeout(function() {
                                    $("#modalCreateAddons").modal('hide');

                                },
                                300);

                        } else {
                            
                            Swal.fire({
                                title: "Success",
                                text: data.message,
                                type: "success"

                            });

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');

                        //$('#spinn-loader').hide();
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



$(document).on('click', '#btn_modifyAddons', function (e) {
    e.preventDefault();


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Modify Addons..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#formmodifyaddons').attr('action');
            var form = $('[id*=formmodifyaddons]');

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
                                text: "It was succesfully updated!",
                                type: "success"

                            });

                            $('#table_addons').DataTable().ajax.reload();

                            setTimeout(function () {
                                $("#modalModifyAddonsDetails").modal('hide');

                                },
                                300);

                        } else {

                            Swal.fire({
                                title: "Success",
                                text: data.message,
                                type: "success"

                            });

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');

                        //$('#spinn-loader').hide();
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

//$(document).on('click', '#btn_cancelbooking', function (event) {

//    setTimeout(function () {

//        //window.location.href = PackageUrl.url_details.replace("pId", data.packageId);
//        window.history.back();
//        // $('#spinn-loader').hide();
//    }, 500);

//});


$(document).on('click', '#btn_canceladdondetails', function (e) {

    e.preventDefault();

    setTimeout(function () { $("#modalModifyAddonsDetails").modal('hide') }, 300);
    
});


$(document).on('keypress', '#txtaddonAmt', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});