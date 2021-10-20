
var $selectedObject;

$(document).ready(function() {

    function RegisterAjaxFormEvents() {

        var form = $('[id*=addoncatform]');
        $.validator.unobtrusive.parse(form);

    }


    if ($.fn.DataTable.isDataTable('#table_addonscategory')) {

        $('#table_addonscategory').dataTable().fnDestroy();
        $('#table_addonscategory').dataTable().empty();

    }

    var tableaddonscategory = $('#table_addonscategory').DataTable(

        {
            "paging": true,
            "lengthChange": false,
            "pagingType": "simple_numbers",

            "dom": "<'#coursetop.row'<'col-sm-6'B><'col-sm-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#coursebottom.row'<'col-sm-5'i><'col-sm-7'p>>",

            "ajax":
            {
                "url": addonCatUrl.addoncat_LoadDatatable,
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
                { "orderable": true, "targets": 1, "data": "addoncatId" },
                { "orderable": true, "targets": 2, "data": "addoncatdetails" },
                { "width": "16%", "targets": 1 }
              
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

                        onCreateNewAddOnsCategory();
                        //window.location.href = courseUrl.urlAdd;
                    }
                },
                {
                    text: '<i class="fa fa-edit fa-fw"></i>',
                    className: 'btn btn-primary EditJob',
                    titleAttr: 'Modify record',
                    action: function (e, dt, node, config) {
                  
                        onmodifyAddonsCategory();
                    }, enabled: false
                },
                {
                    text: '<i class="fa fa-trash fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveAddonsCat',
                    titleAttr: 'Remove record',
                    action: function (e, dt, node, config) {

                        //onremoveCourse();

                    }
                    , enabled: false
                }
            ]
            ,
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-addoncatId', data.addoncatId);
            }
        }

        );// end of datatable


    $('.modal').on('hidden.bs.modal',
        function () {
            $('#table_addonscategory').DataTable().ajax.reload();

            tableaddonscategory.button(0).enable();
            tableaddonscategory.button(1).disable();
            tableaddonscategory.button(2).disable();

        });


    var onCreateNewAddOnsCategory = function () {

        $.ajax({
            type: 'Get',
            url: addonCatUrl.addoncat_Create,
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {
                var modal = $('#modalcreateaddoncat');
                modal.find('#modalcontent').html(result);

                //RegisterAjaxFormEvents();

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

    };//end of create addon category

    var onmodifyAddonsCategory = function () {

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedid = $this.attr('data-addoncatId');


            $.ajax({

                type: 'Get',
                url: addonCatUrl.addoncat_Modify,
                contentType: 'application/html;charset=utf8',
                data: {addoncatId:selectedid},
                datatype: 'html',
                cache: false,
                success: function (result) {
                    var modal = $('#modalmodifyaddoncat');
                    modal.find('#modalcontent').html(result);

                    //RegisterAjaxFormEvents();

                    modal.modal({
                            backdrop: 'static',
                            keyboard: false
                        },
                        'show');
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire('Error updating record!', 'Please try again', 'error');
                }


            });

        }

    };//end of modify addons




                $('#table_addonscategory tbody').on('click', 'tr', function () {

                $selectedObject = null;

                //alert('sadasd');

                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');

                    tableaddonscategory.button(0).enable();
                    tableaddonscategory.button(1).disable();
                    tableaddonscategory.button(2).disable();
                }

                else {
                    tableaddonscategory.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');

                    tableaddonscategory.button(0).disable();
                    tableaddonscategory.button(1).enable();
                    tableaddonscategory.button(2).enable();


                    $selectedObject = $(this);
                }


                });



    $('.btnRemoveAddonsCat').on('click', function (e) {
        e.preventDefault();

        //alert('asdasd');

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedid = $this.attr('data-addoncatId');

            // console.log(selectedcustomerid);

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Addon Category..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes,Proceed Removing..!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: addonCatUrl.addoncat_Remove,
                        ajaxasync: true,
                        data: { addoncatId: selectedid },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully removed!",
                                    type: "success"


                                });

                                setTimeout(function () {


                                    $('#table_addonscategory').DataTable().ajax.reload();

                                    $('#table_addonscategory').button(0).enable();
                                    $('#table_addonscategory').button(1).disable();
                                    $('#table_addonscategory').button(2).disable();

                                }, 600);


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



});//end of docready


$(document).on('click', '#btn_regaddoncategory', function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Adding New Addons Category..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#addoncatform').attr('action');
            var form = $('[id*=addoncatform]');

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

                            $('#table_addonscategory').DataTable().ajax.reload();

                            setTimeout(function () {
                                $("#modalcreateaddoncat").modal('hide');

                            }, 300);

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



$(document).on('click', '#btn_modifyaddoncategory', function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating  Addons Category..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Update it!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#addoncatform').attr('action');
            var form = $('[id*=addoncatform]');

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
                                text: "It was succesfully Updated!",
                                type: "success"

                            });

                            $('#table_addonscategory').DataTable().ajax.reload();

                            setTimeout(function () {
                                $("#modalmodifyaddoncat").modal('hide');

                            }, 300);

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



