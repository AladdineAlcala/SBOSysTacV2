var $selectedObject;

$(document).ready(function () {

    if ($.fn.DataTable.isDataTable('#table_discount')) {

        $('#table_discount').dataTable().fnDestroy();
        $('#table_discount').dataTable().empty();

    }

        var discountTable = $('#table_discount').DataTable(
            {
                "bInfo": false,
                "dom": "<'#menutop.row'<'col-sm-6'B>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'#menubottom.row'<'col-sm-5'l><'col-sm-7'p>>",

                "serverSide": false,
                "paging": true,

                "ajax":
                {
                    "url": discountUrl.url_getdiscountList,
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
                    { 'width': '25%', 'targets': 1, "data": "discCode" },
                    { 'width': '15%', 'targets': 2, "data": "disctype" },
                    {
                        'width': '15%', 'targets': 3, "data": "discount_amt",
                        "className": "text-right",
                        render: $.fn.dataTable.render.number(",", ".", 2)
                    },

                                {
                                    'width': '20%', 'targets': 4,
                                    "data": "discStartdate",
                                    "type": "date",
                                    "render": function (d) {
                                        if (d == null) {
                                            return '';
                                        } else {
                                            return moment(d).format("MMM-DD-YYYY");
                                        }
                                        
                                    }
                                    
                                },
                                {
                                    'width': '20%', 'targets': 5,
                                    "data": "discEnddate",
                                    "type": "date",
                                    "render": function (d) {

                                        if (d == null) {
                                            return '';
                                        } else {
                                            return moment(d).format("MMM-DD-YYYY");
                                        }
                                    }
                                }

                            ],

                            select: {
                                style: 'os',
                                selector: 'td:first-child'

                                },

                                buttons:
                                [
                                    {
                                        text: '<i class="fa fa-plus-square fa-fw"></i>',
                                        className: 'btn btn-primary btnnewdiscount',
                                        titleAttr: 'Add a new discount',
                                        action: function (dt, node, config) {
                                          
                                            onAddNewDiscount();

                                        }
                                    }
                                    ,
                                    {
                                        text: '<i class="fa fa-edit fa-fw"></i>',
                                        className: 'btn btn-primary btneditdiscount',
                                        titleAttr: 'Modify record',
                                        action: function (e, dt, node, config) {
                                           
                                            onModifyDiscount();
                                        }, enabled: false
                                    },
                                    {
                                        text: '<i class="fa fa-trash fa-fw"></i>',
                                        className: 'btn btn-primary btnremovediscount',
                                        titleAttr: 'Remove record',
                                        action: function (e, dt, node, config) {

                                           

                                        }
                                        , enabled: false
                                    }
                                ]
                                ,
                                    createdRow: function (row, data, dataIndex) {
                                        $(row).attr('data-discountId', data.disc_Id);
                                    }
                        });


                var onAddNewDiscount = function() {
        
                    $.ajax({
                        type: 'Get',
                        url: discountUrl.url_addnewdiscount,
                        contentType: 'application/html;charset=utf8',
                        datatype: 'html',
                        cache: false,
                        success: function (result) {
                            var modal = $('#modalCreateDiscount');
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

                var onModifyDiscount = function() {


                    if ($selectedObject.hasClass('selected')) {

                        var $this = $selectedObject;

                        var selectedid = $this.attr('data-discountId');

                      
                        $.ajax({

                            type: 'Get',
                            url: discountUrl.url_modifydiscount,
                            contentType: 'application/html;charset=utf8',
                            data: {discountId:selectedid},
                            datatype: 'html',
                            cache: false,
                            success: function (result) {
                                var modal = $('#modalModifyDiscount');
                                modal.find('#modalcontent').html(result);

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

                };

    //var row = $('#table_discount tbody >tr');

    //for (var i = 0; i <= row.length; i++) {

    //    var disctype = $(row[i].cells[2]).text();
    //    if (disctype === "percentage") {

    //        console.log('percentage');
    //    }
    //}


       $('#table_discount tbody').on('click', 'tr', function () {

        $selectedObject = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            discountTable.button(0).enable();
            discountTable.button(1).disable();
            discountTable.button(2).disable();
        }

        else {
            discountTable.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            discountTable.button(0).disable();
            discountTable.button(1).enable();
            discountTable.button(2).enable();


            $selectedObject = $(this);
        }


       });


       $('.btnremovediscount').on('click', function (e) {
        e.preventDefault();


        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedid= $this.attr('data-discountId');

           // console.log(selectedcustomerid);

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Discount..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes,Proceed Removing..!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: discountUrl.url_removediscount,
                        ajaxasync: true,
                        data: { discountId: selectedid },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully removed!",
                                        type: "success"
                       

                                    });

                                setTimeout(function () {


                                    $('#table_discount').DataTable().ajax.reload();

                                    $('#table_discount').button(0).enable();
                                    $('#table_discount').button(1).disable();
                                    $('#table_discount').button(2).disable();

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




});//=== document end

$(document).on('click','#btn_regdiscount',function(e) {
    e.preventDefault();


            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Adding New Discount..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Save it!'

            }).then((result) => {

                if (result.value) {

                    var formUrl = $('#discountform').attr('action');
                    var form = $('[id*=discountform]');

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

                                    $('#table_discount').DataTable().ajax.reload();

                                    setTimeout(function () {
                                        $("#modalCreateDiscount").modal('hide');

                                    }, 300);

                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error adding record!', 'Please try again', 'error');

                                $('#spinn-loader').hide();
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


$(document).on('click', '#btn_modifydiscount',function(e) {

    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating Discount..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed operation!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#modifydiscountform').attr('action');
            var form = $('[id*=modifydiscountform]');

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

                            $('#table_discount').DataTable().ajax.reload();

                            setTimeout(function () {
                                $("#modalCreateDiscount").modal('hide');

                            }, 300);

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');

                        $('#spinn-loader').hide();
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