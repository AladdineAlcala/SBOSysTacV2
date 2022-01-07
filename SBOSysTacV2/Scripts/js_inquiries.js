var $tblCusBookings = null;

$(document).ready(function () {

    $('#customerbookinginquiry').on('click', function (e) {

        e.preventDefault();
        debugger;
        var cusId = $('#hiddencusId').val();

         if (cusId === "") {

             Swal.fire('Error on retrieving record!', 'Please try again', 'error');

         } else {
             
             $.ajax({
                 type: 'Get',
                 url: inquiryUrl.inquiryUrl_customerbookings,  // "@Url.Action("GetCustomerBookings", "Inquiry")",
                 contentType: 'application/html;charset=utf8',
                 beforeSend: function () {
                     $('#spinn-loader').show();
                 },
                 datatype: 'html',
                 cache: false,
                 success: function (result) {

                     $('#filteredRecord').html(result);
                 },
                 error: function (xhr, ajaxOptions, thrownError) {
                     Swal.fire('Error on retrieving customer!', 'Please try again', 'error');
                 }


             }).done(function () {



                 loadDataTableBookings(cusId);

                 $('#spinn-loader').hide();
             });


         }

       
    });


    function loadDataTableBookings(id) {

        if ($.fn.dataTable.isDataTable('#tbl-cusBookings')) {

            $('#tbl-cusBookings').DataTable().destroy();
            $('#tbl-cusBookings tbody').empty();

        }

        //console.log('DataTable');

        $tblCusBookings = $('#tbl-cusBookings').DataTable({
            "serverSide": false,
            "processing": true,
            "language": {
                'processing':
                    '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },

            "dom": "<'row'<'col-sm-2'B><'col-sm-10'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-2'i><'col-sm-10'p>>",

            "ajax":
            {
                "url": inquiryUrl.inquiryUrl_loadbookingsbycustomer, // "@Url.Action("LoadBookingsByCustomer", "Inquiry")",
                "type": "Get",
                "data": { cusId: id },
                "datatype": "json"
            },
            "columnDefs":
            [
                {
                    'autowidth': true,
                    'targets': 0,
                    "className": "text-left",
                    "data": "transId"
                },
                {
                    'autowidth': true,
                    'targets': 1,
                    "data": "bookdatetime",
                    "className": "text-left",
                    "type": "date",
                    "render": function(d) {
                        return moment(d).format("MMM-DD-YYYY hh:mm: A");
                    }

                },
                {
                    'autowidth': true,
                    'targets': 2,
                    "className": "text-left",
                    "data": "occasion"
                },
                {
                    'autowidth': true,
                    'targets': 3,
                    "className": "text-left",
                    "data": "venue"
                },
                {
                    'autowidth': true,
                    'targets': 4,
                    "className": "text-left",
                    "data": "package"
                },
                {
                    'autowidth': true,
                    'targets': 5,
                    "type": "num-fmt",
                    "orderable": false,
                    "className": "text-right",
                    "data": "packageDue",
                    render: $.fn.dataTable.render.number(",", ".", 2)

                },
                {
                    'autowidth': true,
                    'targets': 6,
                    "data": null,
                    "className": "text-center",
                    "mRender": function(data, type, row) {
                        if (data.isServe === false && data.isCancelled === true) {

                            return 'Cancelled';

                        } else if (data.isServe === false && data.isCancelled === false) {

                            return 'Unserve';
                        } else {
                            return 'Served';
                        }
                    }

                },
                {
                    'width': '15%', 'targets': 7, "data":'transId', "orderable": false, "searchable": false, "className": "text-left",
                        "mRender": function (data, type, row) {

                            var button = '<div class="btn-group">' +
                                '<button type="button" class="btn btn-default btn-sm btn-flat" style="width:100px;"> Action </button>' +
                                '<button type="button" class="btn btn-default btn-sm btn-flat dropdown-toggle" data-toggle="dropdown">' +
                                '<span class="caret"></span>' +
                                '<span class="sr-only">Toggle Dropdown</span>' +
                                '</button>' +
                                '<ul class="dropdown-menu" role="menu">' +
                                '<li><a href="#" class="get_packagemenus" id=' +
                                data +
                                '><i class="fa fa-cube fa-sm"></i> Menus </a></li>' +
                                '<li><a href="#" class="get_packagedetails" id=' +
                                data +
                                '><i class="fa fa-info-circle fa-sm"></i> Details </a></li>';

                            if (row.isServe === false && row.isCancelled === true) {

                                button += '<li><a href="#"class="restore_Active" id=' + data + '><i class="fa fa-recycle fa-sm"></i> Restore Cancelled </a></li>';
                            }
                                        
                            else if (row.isServe === true && row.isCancelled === false) {
                                button += '<li><a href="#" class="restore_Unsered" id=' + data + '><i class="fa fa-recycle fa-sm"></i> Restore Unserved </a></li>';
                            }


                            button += '<li><a href="#" class="getTransHistory" id=' + data + '><i class="fa fa-clock-o fa-sm"></i> View History </a></li>' +
                                    '</ul>' +
                                    ' </div>';



                            return button;
                        }
                    }

                ]
                ,
            buttons:
            [

                {
                    extend: 'pdfHtml5',
                    text: '<i class="fa fa-print fa-sm fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnPrintPayment',
                    titleAttr: 'Print to PDF',
                    download: 'open',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    exportOptions: {
                        columns: ':visible'
                    },
                    footer: true,
                    customize: function (doc) {

                        doc.styles.title.fontSize = 12;
                        doc.defaultStyle.fontSize = 10;
                        doc.styles.tableHeader.fontSize = 10;

                        console.log(doc.content);



                        doc.content[1].table.widths =
                            Array(doc.content[1].table.body[0].length + 1).join('*').split('');

                       // $(win.document.body).find('table tbody td:nth-child(5)').css('text-align', 'right');

                        //$(win.document.body)
                        //    .addClass('asset-print-body')
                        //    .css(/* CSS for entire BODY here... */)
                        //    .prepend($('<img />')
                        //        .attr('src', 'https://sasset.io/wp-content/uploads/2015/08/sasset_logo-300x87.png')
                        //        .addClass('asset-print-img')
                        //    );

                        //$(win.document.body)
                        //    .find('table')
                        //    .addClass('compact')
                        //    .css({
                        //        color: '#FF0000',
                        //        margin: '20px'
                        //        /* Etc CSS Styles..*/
                        //    });


                    }


                }
            ]


        });


        $('#tbl-cusBookings tbody').on('click', 'tr .restore_Unsered',
            function (e) {

                e.preventDefault();

                //var _transid = ($(this).attr('id'));
                // alert(_transid);

                Swal.fire({
                    title: "Are You Sure ?",
                    text: "Confirm Restore Restore Booking ..",
                    type: "question",
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Save it!'

                }).then((result) => {

                    if (result.value) {

                        $.ajax({
                            type: 'POST',
                            url: inquiryUrl.bookUrl_getPackageBookingRestoreServedBooking,
                            data: { transactionId: $(this).attr('id') },
                            success: function (result) {
                                if (result.success) {


                                    Swal.fire({
                                        title: "Success",
                                        text: "Booking Successfully Restored",
                                        type: "success"

                                    });


                                }

                            }


                            //error 
                            ,
                            error: function (xhr, status, errorThrown) {

                                //alert(xhr.status);

                                if (xhr.status === 403) {

                                    var response = $.parseJSON(xhr.responseText);

                                    var responsetxt = "You Are Not Authorized to Access this Operation";
                                    //  console.log(response);
                                    // window.location = response.LogOnUrl;
                                    Swal.fire({
                                        title: "UnAuthorized Access",
                                        text: responsetxt,
                                        type: "error"

                                    });

                                } else {
                                    Swal.fire('Operation Error !', 'Please try again', 'error');
                                }

                            }

                        });


                    }
                });

            });



        $('#tbl-cusBookings tbody').on('click',
            'tr .restore_Active',
            function (e) {

                e.preventDefault();

                //var _transid = ($(this).attr('id'));
               // alert(_transid);

                Swal.fire({
                    title: "Are You Sure ?",
                    text: "Confirm Restore Restore Booking ..",
                    type: "question",
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Save it!'

                }).then((result) => {

                    if (result.value) {

                        $.ajax({
                            type: 'POST',
                            url: inquiryUrl.bookUrl_getPackageBookingRestoreCancelledBooking,
                            data: { transactionId: $(this).attr('id') },
                            success: function (result) {
                                if (result.success) {


                                    Swal.fire({
                                        title: "Success",
                                        text: "Booking Successfully Restored",
                                        type: "success"

                                    });

                             
                                }

                            }

                            
                            //error 
                            ,
                            error: function (xhr, status, errorThrown) {

                                //alert(xhr.status);

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
                });

            });


        $('#tbl-cusBookings tbody').on('click', 'tr .get_packagedetails',
            function (e) {

                e.preventDefault();
               var trnsId = $(this).attr("id");

                $.ajax({
                    type: 'Get',
                    url: inquiryUrl.bookUrl_getPackageId,
                    data: { transactionId:trnsId},
                    success: function (result) {
                        if (result.success) {
                            window.location.href = inquiryUrl.bookUrl_getPackageBookingDetailsId.replace("trans_Id", trnsId);
                        }
                        else {

                            Swal.fire({
                                title: "Operation Failed",
                                text: "No Package Available on this transaction",
                                type: "info"

                            });

                        }
                    }
                });


            });


        $('#tbl-cusBookings tbody').on('click',
            'tr .get_packagemenus',
            function (e) {

                e.preventDefault();

                var _transid = ($(this).attr('id'));
                // console.log(_transid);

                $.ajax({
                    type: 'GET',
                    url: inquiryUrl.bookUrl_getPackageId,
                    data: { transactionId: $(this).attr('id') },
                    success: function (result) {
                        if (result.success) {
                            window.location.href = inquiryUrl.bookUrl_packageBooking.replace("trans_Id", _transid);
                        } else {

                            Swal.fire({
                                title: "Operation Failed",
                                text: "No Package Available on this transaction",
                                type: "info"

                            });

                        }
                    }
                });
            });



        $('#tbl-cusBookings tbody').on('click',
            'tr .getTransHistory',
            function (e) {

                e.preventDefault();

                var _transid = ($(this).attr('id'));
                // console.log(_transid);

                window.location.href = inquiryUrl.bookUrl_BookingTransHistory.replace("trans_Id", _transid);


            });


    }//end function

   
 
});// end doc.

//$(document).ajaxStart(function() {
//    $('#spinn-loader').show();
//}).ajaxStop(function() {
//    $('#spinn-loader').show();
//});