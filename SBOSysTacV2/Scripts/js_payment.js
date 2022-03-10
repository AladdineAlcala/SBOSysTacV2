var $tablePayments = null;



function cleanDecimal(val) {

    return parseFloat(val.replace(/,/g, '')).toFixed(2);
}

function currencyFormat(num) {
    return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}


$(document).ready(function() {


    var row = $('#tbl_paymentBooking tbody >tr');

    var data = $("#tbl_paymentBooking tr:eq(1)");

    /* var x = parseFloat(data.find("td:eq(5)").text().replace(/,/g, '')).toFixed(2);*/

    var initBal = cleanDecimal(data.find("td:eq(6)").text());

    //console.log(init_bal);


    var temp;
    var totaldebit = 0;
    var balance = 0;
    var initialBal = initBal;


    var tfrow = $("#tbl_paymentBooking tfoot >tr");



    for (var i = 1; i < row.length; i++) {

        //var r = $(row[i]);

        //var prevrow = r.prev();

        //var credit_val = cleanDecimal($(row[i].cells[3]).text());
        var debitVal = cleanDecimal($(row[i].cells[4]).text());

        //console.log("debit " + val);

        if (debitVal > 0) {

            temp = initBal - debitVal;
            balance = temp;
            totaldebit = parseFloat(totaldebit) + parseFloat(debitVal);
            
           // console.log(totaldebit);
        }

        totaldebit = parseFloat(totaldebit);


    

       // var prevBal = prevrow.find("td:eq(5)").text();
       // init_bal = cleanDecimal(prevBal);
        initBal = balance;
    


        if (isNaN(parseFloat(balance))) {
            balance = 0;

        } else {

            //balance = balance.toFixed(2);

            $(row[i].cells[6]).text(currencyFormat(balance));
        }

       
    }

    $('#totalPayment').html(currencyFormat(totaldebit));

    if (balance <= 0) {

        initBal = parseFloat(initBal);
       // console.log(currencyFormat(init_bal));
        $('#endbalance').html(currencyFormat(initBal));

    } else {
          $('#endbalance').html(currencyFormat(balance));

    }

    //if refund is present in transactions get value of refund 
    if (tfrow.length > 1) {
        var refund = 0;
        var refundEndBalance = 0;

        refund = $(tfrow[1].cells[3]).text();

        //console.log(parseFloat(initialBal));

        refundEndBalance = parseFloat(initialBal) - parseFloat(totaldebit);

        //console.log(refundEndBalance);


    }


  

}); //======= end of code document.ready====================



//--------- load table payments--------------------------
function loadTablePayments() {

    if ($.fn.DataTable.isDataTable('#tbl_paymentBooking')) {

        $('#tbl_paymentBooking').dataTable().fnDestroy();
        $('#tbl_paymentBooking').dataTable().empty();

    }

    //var id = tranId;
    //var total_Payment = 0;

    $tablePayments = $('#tbl_paymentBooking').DataTable({
        "serverSide": false,
        "dom": "<'#payment_top.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#payment_bottom.row'<'col-sm-12 '>>",


        "columnDefs": [
            { "orderable": false, "targets": 5 },
            { "width": "150px", "targets": 5 },
            { "width": "100px", "targets": 6 }
        ],

        buttons:
        [
            {
                text: '<i class="fa fa-plus-square fa-sm fa-fw"></i>',
                className: 'btn btn-primary btn-sm btnAddPayment',
                titleAttr: 'Create new payment',
                action: function () {

                   // onAddNewPayment(id);
                }


            },
            {
                text: '<i class="fa fa-print fa-sm fa-fw"></i>',
                className: 'btn btn-primary btn-sm btnPrintPayment',
                titleAttr: 'Print',
                action: function () {

                    //onAddNewPayment(id);
                }


            }
        ]


    });
}


$(document).on('click', '#btn_regPayment', function (e) {
    e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Adding Payment..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Transaction..'
          
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#paymentform').attr('action');
                var form = $('[id*=paymentform]');

                $.validator.unobtrusive.parse(form);
                form.validate();



                if (form.valid()) {


                    $('#spinn-loader').show();
                    $.ajax({
                        type: 'POST',
                        url: formUrl,               //Payments/SavePayment
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                $('#paymentsTable').load(data.url);

                                $("#modal-PaymentBooking").modal('hide');

                                //console.log(data.url);
                                setTimeout(function () {

                                    Swal.fire({
                                        title: "Success",
                                        text: "Payment was succesfully Added!",
                                        type: "success"

                                    });

                                    $('#spinn-loader').hide();

                                }, 1000);

                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');

                            $('#spinn-loader').hide();
                        }
                    });

                }
                else {

                    $('#spinn-loader').hide();

                    $.each(form.validate().errorList, function (key, value) {
                        $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                        $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                        $errorSpan.show();
                    });

                }
          
            }
        });


});

$(document).on('click',
    '#btn_cancelpaymnt',
    function (e) {
        e.preventDefault();

        setTimeout(function () { $("#modal-PaymentBooking").modal('hide') }, 300);
    });

$(document).on('click',
    '#btn_cancelupdatePaymnt',
    function (e) {
        e.preventDefault();

        setTimeout(function () { $("#modal-PaymentBooking").modal('hide') }, 300);
    });



//remove payment
$(document).on('click', '#removepayment', function (e) {
    e.preventDefault();

    var paymentId = $(this).closest('td').attr('data-id');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing This Payment..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed for removing!'

    }).then((result) => {

        if (result.value) {

            $('#spinn-loader').show();

            $.ajax({

                type: "post",
                url: paymentsUrl.payUrl_removePayment,
                ajaxasync: true,
                data: { pmtNo: paymentId },
                cache: false,
                success: function (data) {

                    if (data.success) {

                        Swal.fire({
                                title: "Success",
                                text: "Payment was succesfully removed!",
                                type: "success"
                         
                            });


                        setTimeout(function () {

                            $('#paymentsTable').load(data.url);
                            $('#spinn-loader').hide();
                        }, 500);



                        $('#spinn-loader').hide();
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
                            text: data.Error,
                            type: "error"

                        });

                    } else {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }

            });
            //ajax end

        }
    });


});

// update payment

$(document).on('click', '#updatepayment', function (e) {
    //var pymtId = $(this).closest('td').attr('data-id');

    UpdatePayment($(this).closest('td').attr('data-id'));


});

function UpdatePayment(pymtId) {

    var id = pymtId;

    $.ajax({

        type: 'Get',
        url: paymentsUrl.payUrl_updatePayment,
        contentType: 'application/html;charset=utf8',
        data: { pymtId: id },
        datatype: 'html',
        cache: false,
        success: function (result) {
            var modal = $('#modal-PaymentBooking');
            modal.find('#modalcontent').html(result);

            $('#paymentdatepicker').datepicker("setDate", new Date());
            $('#paymentdatepicker').datepicker({ autoclose: true, format: 'mm/dd/yyyy' });

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

//update payment post

$(document).on('click', '#btn_updatePayment', function (e) {
    e.preventDefault();

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Updating Payment..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed Update it!'

            }).then((result) => {

                if (result.value) {

                    $('#spinn-loader').show();

                    var formUrl = $('#Updatepaymentform').attr('action');
                    var form = $('[id*=Updatepaymentform]');
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

                                    Swal.fire({
                                            title: "Success",
                                            text: "It was succesfully Updated!",
                                            type: "success"
                                   

                                        });
                                    $('#paymentsTable').load(data.url);

                                    setTimeout(function () {
                                        $("#modal-PaymentBooking").modal('hide');

                                        $('#spinn-loader').hide();
                                    }, 500);

                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error updating record!', 'Please try again', 'error');
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



 getBalance=()=> $('#endbalance').html();




//function onAddNewPayment(id) {

$(document).on('click','#add_payment',function(e) {
        e.preventDefault();


   /* console.log(getBalance());*/
    var _balance = cleanDecimal(getBalance());

/*    console.log(_balance);*/

    $.ajax({
        type: 'Get',
        url: paymentsUrl.payUrl_addPayment,    // url:'http://localhost:Sals/Payments/Add_PaymentPartialView',
        contentType: 'application/html;charset=utf8',
        data: { transactionId: $(this).data('id'), balance: _balance},
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-PaymentBooking');
            modal.find('#modalcontent').html(result);

            $('#paymentdatepicker').datepicker("setDate", new Date());
            $('#paymentdatepicker').datepicker({ autoclose: true, format: 'mm/dd/yyyy' });

            modal.modal({
                    backdrop: 'static',
                    keyboard: false
                },
                'show');
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Swal.fire('Error adding record!', 'Please try again', 'error');
        }


    });
});

$(document).on('keypress', '#amtPay', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});


$(document).on('click', 'a.print_Payment', function (e) {
    e.preventDefault();
    e.stopPropagation();

    var transid = $(this).data('id');
    var url = $(this).attr('href');

    window.location.href = url + '?transId=' + transid;

});

$(document).on('keypress', '#txtpayamt', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});

$(document).on('click', '#printInvoice', function(e) {


    var modal = $('#modal-PrintInvoice');
/*    modal.find('#modalcontent').html(result);*/

    modal.modal({
            backdrop: 'static',
            keyboard: false
        },
        'show');

});