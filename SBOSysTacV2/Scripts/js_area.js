var $tableArea;
var $selectedObject;

$(document).ready(function () {
    
    if ($.fn.DataTable.isDataTable('#table_area')) {

        $('#table_area').dataTable().fnDestroy();
        $('#table_area').dataTable().empty();

    }

    $tableArea = $('#table_area').DataTable({
        
            "serverSide": false,
            "processing": true,
            "language": {

                'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },
            "paging": true,
            "dom": "<'#customertop.row'<'col-sm-4'B><'col-sm-4'l><'col-sm-4'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#customerbottom.row'<'col-sm-5'i><'col-sm-7'p>>",
            "pagingType": "full_numbers",
            "ajax":
            {
                "url": areaUrl.url_arealList,
                "type": "Get",
                "datatype": "json"
            },
            "columns": [
                { "data": null },
           
              
                { "data": "areaDetails" }
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
                { 'width': '100%', 'targets': 1 }
           
              
            ],
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-areaId', data.areaId);
            },

            "order": [1, 'asc'],

            select: {
                style: 'os',
                selector: 'td:first-child'

            },

            buttons:
            [
                {
                    text: '<i class="fa fa-plus-square fa-fw"></i>',
                    className: 'btn btn-primary btnAddCustomer',
                    titleAttr: 'Add a new record',
                    action: function () {
                   
                        onAddNewArea();

                    }
                },
                {
                    text: '<i class="fa fa-edit fa-fw"></i>',
                    className: 'btn btn-primary btnModifyCustomer',
                    titleAttr: 'Modify record',
                    action: function () {
                   
                        onModifyArea();

                    }, enabled: false
                },
                {
                    text: '<i class="fa fa-trash fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveCustomer',
                    titleAttr: 'Remove record',
                    action: function () {
                      
                        onremoveArea();

                    }, enabled: false
                }
                ,
                {
                    text: '<i class="fa fa-print fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveCustomer',
                    titleAttr: 'Print record',
                    action: function () {
                      
                        //onremoveMenu();

                    }, enabled: false
                }
            ]



        

    });


    $('#table_area tbody').on('click', 'tr', function () {

        $selectedObject = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $tableArea.button(0).enable();
            $tableArea.button(1).disable();
            $tableArea.button(2).disable();
            $tableArea.button(3).disable();
        }

        else {
            $tableArea.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $tableArea.button(0).disable();
            $tableArea.button(1).enable();
            $tableArea.button(2).enable();
            $tableArea.button(3).enable();


            $selectedObject = $(this);
        }


    });

    var onAddNewArea = function() {

        $.ajax({
            type: 'Get',
            url: areaUrl.url_newArea,
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {
                var modal = $('#modal-areas');
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


    var onModifyArea = function() {


        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedAreaid = $this.attr('data-areaId');

            $.ajax({
                type: 'Get',
                url: areaUrl.url_modifyArea,
                contentType: 'application/html;charset=utf8',
                data: {areaId: selectedAreaid},
                datatype: 'html',
                cache: false,
                success: function (result) {
                    var modal = $('#modal-modify_areas');
                    modal.find('#modalcontentmodify').html(result);

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

    var onremoveArea = function() {

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedAreaid = $this.attr('data-areaId');

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Area..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed Operation!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: areaUrl.url_removeArea,
                        ajaxasync: true,
                        data: {areaId:selectedAreaid },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully removed!",
                                        type: "success"
                               

                                    });


                                $('#table_area').DataTable().ajax.reload();

                                setTimeout(function () {

                                        $tableArea.button(0).enable();
                                        $tableArea.button(1).disable();
                                        $tableArea.button(2).disable();
                                        $tableArea.button(3).disable();

                                    },
                                    600);

                            }
                            else {

                                Swal.fire('Unable to remove record!', data.message, 'error');


                            }
                        }
                    });//-----ajax end


                }
            });


        }


     

    };

});//end document

$(document).on('click',
    '#btn_regArea',
    function(e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Area Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation!'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#formregisterArea').attr('action');
                var form = $('[id*=formregisterArea]');

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

                                $('#table_area').DataTable().ajax.reload();

                                setTimeout(function() {
                                        $tableArea.button(0).enable();
                                        $tableArea.button(1).disable();
                                        $tableArea.button(2).disable();
                                        $tableArea.button(3).disable();

                                        $("#modal-areas").modal('hide');
                                    },
                                    600);



                            } else
                            {
                                
                                Swal.fire({
                                    title: "Unable to Save Data",
                                    text: data.message,
                                    type: "error"

                                });

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





//modify area

$(document).on('click',
    '#btn_modifyArea',
    function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Area Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation!'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#formmodifyArea').attr('action');
                var form = $('[id*=formmodifyArea]');

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

                                $('#table_area').DataTable().ajax.reload();

                                setTimeout(function () {
                                        $tableArea.button(0).enable();
                                        $tableArea.button(1).disable();
                                        $tableArea.button(2).disable();
                                        $tableArea.button(3).disable();

                                        $("#modal-areas").modal('hide');
                                    },
                                    600);



                            } else {

                                Swal.fire({
                                    title: "Unable to Save Data",
                                    text: data.message,
                                    type: "error"

                                });

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
