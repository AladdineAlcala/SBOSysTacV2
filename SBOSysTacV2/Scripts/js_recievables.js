var $tblCusRecievables = null;
$(document).ready(function() {

    $('#accncustomer :input').focus();



});  //end doc ready

function currencyFormat(num) {
    return (num
        .toFixed(2)
        .replace('.', ',')
        .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    );
}


var recievables = function loadDataRecievables(id) {

    if ($.fn.dataTable.isDataTable('#tbl-cusrecievables')) {

        $('#tbl-cusrecievables').DataTable().destroy();
        $('#tbl-cusrecievables tbody').empty();

    }



    $tblCusRecievables = $('#tbl-cusrecievables').DataTable({

        "serverSide": false,
        "processing": true,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },


        //"dom": "<'#bookingtop.row'<'col-sm-6'B><'col-sm-6'f>>" +
        //    "<'row'<'col-sm-12'tr>>" +
        //    "<'#bookingbottom.row'<'col-sm-5'l><'col-sm-7'p>>"
        //,

        "ajax":
        {
            "url": recievableUrl.inquiryUrl_loadbookingsbycustomer,
            "type": "Get",
            "data": { cusId: id },
            "datatype": "json"
        },
        "columnDefs":
        [
            {
                'autowidth': true,
                'targets': 0,
                "data": "transId"
            },
            {
                'autowidth': true,
                'targets': 1,
                "data": "bookdatetime",
                "type": "date",
                "render": function (d) {
                    return moment(d).format("MMM-DD-YYYY");
                }

            },
            {
                'autowidth': true,
                'targets': 2,
                "data": "occasion"
            },

            {
                'autowidth': true,
                'targets': 3,
                "data": "packagedetails"
            },
            {
                'autowidth': true,
                'targets': 4,
                "orderable": false,
                "className": "text-right",
                "data": "totalPackageAmount",
                render: $.fn.dataTable.render.number(",", ".", 2)

            },
            {
                'autowidth': true,
                'targets': 5,
                "orderable": false,
                "className": "text-right",
                "data": "totalPayment",
                render: $.fn.dataTable.render.number(",", ".", 2)

            },
            {
                'autowidth': true,
                'targets': 6,
                "orderable": false,
                "className": "text-right",
                "data": "balance",
                "render": function (data, type, row) {

                    // negative value occurs here---------
                    if (row.refunds > 0) {

                        return "w/ Refunded  ( " + currencyFormat(row.refunds) + ")";

                    } else {

                        return currencyFormat(row.balance);
                    }

                }

                //render: $.fn.dataTable.render.number(",", ".", 2)

            },

            {
                'width': '12%', 'targets': 7, "data": "transId", "orderable": false, "searchable": false, "className": "text-right",
                "mRender": function (data) {

                    return '<button class="btn btn-flat bg-olive btn-sm get_paymentdetails" id=' + data +
                        '><i class="fa fa-money fa-sm"></i> Payments </button>';
                }
            }
        ]
        ,
        "footerCallback": function (row, data, start, end, display) {

            var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;
            var api = this.api(), data;

            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            var total_Package = api.column(4).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                },
                0);

            var total_Payment = api.column(5).data().reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                },
                0);

            var total_unpaidBal = api.column(6).data().reduce(function (a, b) {

                    return intVal(a) + intVal(b);
                },
                0);

            total_Package = parseFloat(total_Package);

            total_Payment = parseFloat(total_Payment);

            total_unpaidBal = parseFloat(total_unpaidBal);

            //console.log(api.column(6).data());


            //update footer
            $(api.column(4).footer()).html(numFormat(total_Package));
            $(api.column(5).footer()).html(numFormat(total_Payment));
            $(api.column(6).footer()).html(numFormat(total_unpaidBal));

            // $('#totalPayment').html(total_Payment.toFixed(2));
        }
        //,
        //buttons:
        //[

        //    {
        //        text: '<i class="fa fa-file-text-o fa-sm fa-fw"></i>',
        //        className: 'btn btn-primary btn-sm btnAddPrinting',
        //        titleAttr: 'Print SOA',
        //        action: function () {

        //            //window.location.href = bookingsUrl.bookUrl_createBooking;
        //        }
        //    }
        //]
    });


} //end function


$(document).on('click', '.get_paymentdetails', function (e) {

    e.preventDefault();

    $('#spinn-loader').show();

    var _transid = ($(this).attr('id'));

    $.ajax({
        type: 'Get',
        url: bookingsUrl.bookUrl_getPackageId,
        data: { transactionId: _transid },
        success: function (result) {

            if (result.success) {
                window.location.href = bookingsUrl.bookUrl_paymentBooking.replace("tId", _transid);

            } else {
                Swal.fire('No Package Available on this transaction', 'Please try again', 'info');
            
            }

            $('#spinn-loader').hide();
        }
    });


    return false;
});