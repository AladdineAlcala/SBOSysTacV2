var $tble_refund;
var $table_refundLedger;
var $selectedObject;


$(document).ready(function () {

    var createBookingRefund = null;


    ////Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        //return this.optional(element) || moment(value, "DD-MMM-YYYY HH:mm A", true).isValid();
        return this.optional(element) || moment(value, "MM/DD/YYYY", true).isValid();
    }


    if ($.fn.DataTable.isDataTable('#refundTable')) {

        $('#refundTable').dataTable().fnDestroy();
        $('#refundTable').dataTable().empty();

    }

  
    function currencyFormat(num) {
        return (num
            .toFixed(2)
            .replace('.',',')
            .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        );
    }

    //$('.refundcurrency').html(currencyFormat(this.val));

    $tble_refund = $('#refundTable').DataTable({
        "serverSide": false,
        "processing": true,
        "language": {
            'processing':
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },

        "dom": "<'#bookingtop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#bookingbottom.row'<'col-sm-5'l><'col-sm-7'p>>",

        "select": {
            "style": 'os',
            "selector": 'td:first-child'

        },
        "ajax":
        {
            "url": bookingsrefundUrl.RefundTableLoad,
            "type": "Get",
            "datatype": "json"
        },

        "columnDefs":
        [
            {
                'targets': 0,
                'searchable': false,
                'orderable': false,
                'width': '5%',
                'className': 'select-checkbox',
                'data': null,
                'defaultContent': ''
            },
            {
                'width': '10%',
                'targets': 1,
                'data': 'TransId'
            },
            {
                'autowidth': true,
                'targets': 2,
                'data': 'CustomerName'
            },
            {
                'autowidth': true,
                'targets': 3,
                'data': 'EventLocation'
            },
            {
                'autowidth': true,
                'targets': 4,
                'data': 'Eventdate',
                'type': 'date',
                'render': function(d) {
                    return moment(d).format("MMM/DD/YYYY");
                }
            },
            {
                'width': '15%',
                'targets': 5,
                "className": "text-right",
                'data': 'RefundAmount',
                //render: $.fn.dataTable.render.number(",", ".", 2)
                'render':function(data, type,row) {
                    if (row.isCancelled === false) {

                        return currencyFormat(row.RefundAmount);

                    } else {

                        return currencyFormat(row.PaymemntAmount);
                    }
                    console.log(data);

                   // return currencyFormat(row.RefundAmount);
                }
            },
            {
                'autowidth': true,
                'targets': 6,
                'data': 'Status',
                'render': function(data) {
                    return 'pending';
                }
            },
            {
                'width': '10%',
                'targets': 7,
                'data':null,
                'className': 'text-center',
                'searchable': false,
                'render': function (data, type, row) {

                   // console.log(row.hasrefundEntry);

                    if (row.hasrefundEntry) {
                        return '<button class="btn btn-sm btn-primary get-refundEntry" id= ' +
                            row.TransId +
                            '> <i class="fa fa-book"></i> </button>';
                    } else {

                        return '<button class="btn btn-sm btn-primary disabled"> <i class="fa fa-book"></i> </button>';
                    }
                }
            }
        ],

        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-transid', data.TransId);
        },

        buttons:
        [
            {
                text: '<i class="fa fa-plus-square fa-sm fa-fw"></i>',
                className: 'btn btn-primary btn-sm btnCreateRefundEntry',
                titleAttr: 'Create Refund Entry',
                action: function() {

                    createBookingRefund();
                },
                enabled: false
            }

        ]

       

       });

    //---------------------------- create refund -------------------------------------------------

    createBookingRefund = function () {

        debugger;

        var $this = $selectedObject;

        var transId= $this.attr('data-transid');

        $.ajax({
            type: 'Get',
            url: bookingsrefundUrl.CheckRefundEntry,
            data: { transId },
            dataType: 'json',
            cache: false,
            success: function (data) {
                    if (data.recordexist) {

                        Swal.fire({
                            title: "Operation Failed",
                            text: "Refund Entry Already Exist",
                            type: "info"

                        });

                    } else {
                        loadRefundEntry(transId);



                    }
                }

            });



            }

    // --------------------------- end of code ----------------------------------------------------


    function loadRefundEntry(id) {

        var transId = id;

        $.ajax({
            type: 'Get',
            url: bookingsrefundUrl.CreateRefundEntry,
            contentType: 'application/html;charset=utf8',
            data: { transId },
            datatype: 'html',
            cache: false,
            success: function (result) {

                var modal = $('#modal-createRefund');
                modal.find('#modalcontent').html(result);

                $('#refunddatepicker').datepicker("setDate", new Date());
                $('#refunddatepicker').datepicker({ autoclose: true, format: 'mm/dd/yyyy' });

                modal.modal({
                        backdrop: 'static',
                        keyboard: false
                    },
                    'show');
            },

            error: function (xhr, status, errorThrown) {
                if (xhr.status === 403) {
                    var response = $.parseJSON(xhr.responseText);
                    Swal.fire('Unable to redirect', response.Error, 'error');
                } else {
                    Swal.fire('Error on parsing data', 'Pls try again', 'error');
                }
            }
        });
    }


    $('#refundTable tbody').on('click', 'tr .get-refundEntry', function (e) {

        e.preventDefault();

        //var transId = $(this).attr('id');

        window.location.href = bookingsrefundUrl.GetRefundEntry.replace("tid", this.id);
      

    });
   


        $('#refundTable tbody').on('click', 'tr', function (e) {

        e.stopPropagation();

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $tble_refund.button(0).disable();
           

        } else {

            $tble_refund.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            
            $tble_refund.button(0).enable();

            //if (hassuperadminrights === 'true') {

            //    $tablebookings.button(4).enable();

            //}
            //;
            //$tablebookings.button(5).disable();

            $selectedObject = $(this);

        }
    });



    //loadRefundLedgerTransaction();

  
    //function loadRefundLedgerTransaction() {
        
    //    if ($.fn.DataTable.isDataTable('#refundledgertable')) {

    //        $('#refundledgertable').dataTable().fnDestroy();
    //        $('#refundledgertable').dataTable().empty();

    //    }

    //    $table_refundLedger = $('#refundledgertable').DataTable({
            
    //        "serverSide": false,
    //        "filter": false,
    //        "paging": false,
    //        "binfo": false,
    //        "bPaganate":false,
    //        "bLengthChange": false,
          

    //        //"dom": "<'#payment_top.row'<'col-sm-6'B><'col-sm-6'f>>" +
    //        //    "<'row'<'col-sm-12'tr>>" +
    //        //    "<'#payment_bottom.row'<'col-sm-12 '>>",
            
    //        "columnDefs": [
    //            { "orderable": false, "targets": 4 },
    //            { "orderable": false, "targets": 6 },
    //            { "width": "150px", "targets": 4 },
    //            { "width": "150px", "targets": 5 },
    //            { "width": "150px", "targets": 6 }
    //        ]
           

    //    });
    //}


    $('#add_refundpayment').on('click', function (e) {
        e.preventDefault();

        var transId = $(this).attr('data-id');

        $.ajax({
            type: 'Get',
            url: bookingsrefundUrl.CreatePaymentRefundAccn,
            contentType: 'application/html;charset=utf8',
            data: { transId },
            datatype:'html',
            cache: false,
            success: function (result) {

                var modal = $('#modal-PaymentRefund');
                modal.find('#modalcontent').html(result);

                //$('#paymentrefunddatepicker').datepicker("setDate", new Date());
                //$('#paymentrefunddatepicker').datepicker({ autoclose: true, format: 'mm/dd/yyyy' });
               

                modal.modal({
                        backdrop: 'static',
                        keyboard: false
                    },
                    'show');
            }
        });

    });


    //var result = null;

    //$.ajax({
    //    url: bookingsrefundUrl.bookrefundUrl_DataTableLoad,
    //    type: 'get',
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    async: false,
    //    success: function (data) {
    //        result = data;
    //    }
    //});

    //console.log(result);

});
//======================= end doc ready ==============================================================================



$(document).on('click', '#save_refund', function (e) {

    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Refund Entry..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Transaction..'

    }).then((result) => {

        if (result) {

            var formUrl = $('#refundBooking').attr('action');
            var form = $('[id*=refundBooking]');

            $.validator.unobtrusive.parse(form);
            form.validate();

            if (form.valid()) {

                $.ajax({
                    type: 'Post',
                    url: formUrl,
                    data: form.serialize(),
                    dataType: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {
                            Swal.fire({
                                title: "Success",
                                text: "Refund Entry Successfully created",
                                type: "success"

                            });


                            setTimeout(function () {

                                $('#modal-createRefund').modal('hide');
                                $('#refundTable').DataTable().ajax.reload();
                            }, 1000);

                           
                        }
                    }
                });

            }
        }
    });


});


$(document).on('click','#btn_regcreditentry',function(e) {
    e.preventDefault();


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Refund Entry..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Transaction..'

    }).then((result) => {

        if (result) {

            $('#spinn-loader').show();

            var formUrl = $('#form_registercreditentry').attr('action');
            var form = $('[id*=form_registercreditentry]');

            $.validator.unobtrusive.parse(form);
            form.validate();

            if (form.valid()) {

                $.ajax({
                    type: 'Post',
                    url: formUrl,
                    data: form.serialize(),
                    dataType: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            Swal.fire({
                                title: "Success",
                                text: "Refund Entry Successfully created",
                                type: "success"

                            });

                            setTimeout(function () {

                                $('#refundEntry').load(data.url);
                                $('#spinn-loader').hide();
                            }, 1000);

                           
                        }
                    }
                });

            }
        }
    });


});

$(document).on('keypress', '#txtpayentry_amt', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});