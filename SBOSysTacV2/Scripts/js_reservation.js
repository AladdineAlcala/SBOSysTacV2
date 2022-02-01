

var $selectedObject;
var $table_reservations;

$(document).ready(function () {


    //Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD-MMM-YYYY HH:mm A", true).isValid();
    }

    $.fn.dataTable.moment('MMM-DD-YYYY hh:mm');

    loadReservationsDataTable();

    function loadReservationsDataTable() {


        if ($.fn.DataTable.isDataTable('#tbl_eventreservation')) {

            $('#tbl_eventreservation').dataTable().fnDestroy();
            $('#tbl_eventreservation').dataTable().empty();

        }

        $table_reservations = $('#tbl_eventreservation').DataTable({

            "serverSide": false,
            "processing": true,
            "language": {

                'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },
            "paging": true,

            "order": [[1, 'asc']],

            "select": {
                "style": 'os',
                "selector": 'td:first-child'

            },

            "dom": "<'#bookingtop.row'<'col-sm-6'B><'col-sm-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#bookingbottom.row'<'col-sm-5'l><'col-sm-7'p>>",

            "pagingType": "full_numbers",

            "ajax":
            {
                "url": reservationUrl.reserveUrl_listofReservations,
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
                }
                ,
                {
                    'autowidth': true, 'targets': 1,
                    "data": "reserveDate",
                    "type": "date",
                    "render": function (d) {
                        return moment(d).format("MMM-DD-YYYY hh:mm: A");
                    }

                },
                {
                    'autowidth': true, 'targets': 2,
                    "data": "fullname"

                },
                {
                    'autowidth': true, 'targets': 3,
                    "data": "occasion"

                }
                ,
                {
                    'autowidth': true, 'targets': 4,
                    "data": "noofperson"

                }
                ,
                {
                    'autowidth': true, 'targets': 5,
                    "data": "eventVenue"

                }
                ,
                {
                    'autowidth': true, 'targets': 6, "data": "reservationId", "orderable": false, "searchable": false, "className": "text-center",
                    "mRender": function (data) {
                        return '<button class="btn btn-flat btn-sm bg-olive addreservationtoBooking" id=' +
                            data +
                            '><i class="fa fa-book fa-sm"></i> Book </button>';

                        //' <button class="btn btn-flat bg-purple btn-sm getdetails" id=' + data + '><i class="fa fa-info-circle fa-sm"></i> Details </button>';
                    }
                }
            ]
            ,
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-reservation_id', data.reservationId);
            }
            ,
            buttons:
            [
                {
                    text: '<i class="fa fa-plus-square fa-sm fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnAddReservation',
                    titleAttr: 'Create new reservation',
                    action: function () {



                    }
                },
                {
                    text: '<i class="fa fa-edit fa-sm fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnModifyReservation',
                    titleAttr: 'Modify record',
                    action: function () {

                        onModifyReservation();


                    }, enabled: false
                },
                {
                    text: '<i class="fa fa-trash fa-sm fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnRemoveReservation',
                    titleAttr: 'Remove record',
                    action: function () {

                    onremoveBooking();

                    }, enabled: false
                }


            ]

        });

    };//----- end loadreservationDatatable Function


    var onremoveBooking = function () {

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var reservationId = $this.attr('data-reservation_id');

           // console.log(reservationId);


                
            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Reservation..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed operation!'

            }).then((result) => {

                if (result.value) {

                    $.ajax({
                        type: "post",
                        url: reservationUrl.reserveUrl_removeReservation,
                        ajaxasync: true,
                        data: { reservationId: reservationId },
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully removed!",
                                        type: "success"

                                    });



                              

                                setTimeout(function () {
                                    $('#tbl_eventreservation').DataTable().ajax.reload();
                                    $table_reservations.button(0).enable();
                                    $table_reservations.button(1).disable();
                                    $table_reservations.button(2).disable();


                                }, 600);




                            }
                        }
                    });

                }
            });

          


        }

    };



    var onModifyReservation=function() {
        
        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedReservationId = $this.attr('data-reservation_id');

            window.location.href = reservationUrl.reserveUrl_modifyReservation.replace("resId", selectedReservationId);


        }
    }



    $('.btnAddReservation').on('click', function (e) {

        e.preventDefault();

        window.location.href = reservationUrl.reserveUrl_createNewReservation;
    });








    //$('#btn_modifyreservation').on('click', function (e) {

    //    e.preventDefault();

      

    //});



    $.fn.dataTable.moment = function (format, locale) {

        var types = $.fn.dataTable.ext.type;

        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ? 'moment-' + format : null;
        });

        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    }



   


    //cancel booking click event

    $('#btn_cancelbooking').on('click',
        function (e) {

            e.preventDefault();


            Swal.fire('Cancel Booking', 'Cancel this Booking', 'info');

            window.history.back();
        

        });


  

    $('#tbl_eventreservation tbody').on('click', 'tr .addreservationtoBooking',
        function (e) {
             
            e.preventDefault();

            var _reservationId = $(this).attr('id');

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Add this reservaton to booking?",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Proceed Operation.!'

            }).then((result) => {

                if (result.value) {

                    window.location.href = reservationUrl.reserveUrl_bookReservation.replace("resId", _reservationId);

                }
            });

         

            //$.ajax({
            //    type: 'GET',
            //    url: bookingsUrl.bookUrl_getPackageId,
            //    data: { transactionId: $(this).attr('id') },
            //    success: function (result) {
            //        if (result.success) {
            //            window.location.href = bookingsUrl.bookUrl_packageBooking.replace("trans_Id", _transid);
            //        } else {
            //            alert('No Package Available on this transaction');
            //        }
            //    }
            //});


        });



    $('#tbl_eventreservation tbody').on('click', 'tr', function (e) {

        e.stopPropagation();

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $table_reservations.button(0).enable();
            $table_reservations.button(1).disable();
            $table_reservations.button(2).disable();



        } else {

            $table_reservations.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $table_reservations.button(0).disable();
            $table_reservations.button(1).enable();
            $table_reservations.button(2).enable();


            $selectedObject = $(this);

        }
    });


    $('#btn_cancelreservation').on('click',
        function (e) {

            e.preventDefault();

            Swal.fire('Cancel Booking', 'Cancel this Booking', 'info');

            window.history.back();

            //swal({
            //        title: "Are You Sure ?",
            //        text: "Confirm Cancel ",
            //        type: "warning",
            //        showCancelButton: true,
            //        confirmButtonColor: '#3085d6',
            //        cancelButtonColor: '#d33',
            //        confirmButtonText: 'Yes, Cancel Operation!',
            //        closeOnConfirm: true, closeOnCancel: true

            //    },
            //    function (isConfirm) {
            //        if (isConfirm) {

            //            window.history.back();

            //        }

            //    });

        });



}); //document end


$(document).on('click','#btn_regreservation', function (e) {

    e.preventDefault();

    var form = $(this).closest('form');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Reservation..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Operation !'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#reservation').attr('action');

            $.validator.unobtrusive.parse(form);

            form.validate();

            if (form.valid()) {


                $.ajax({
                    type: 'POST',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    //contentType:'application/json;charset=utf-8',
                    success: function (data) {
                        if (data.success) {

                            //alert(data.success);

                            Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully added!",
                                    type: "success"
                             
                                });


                            setTimeout(function () {

                                window.location.href = reservationUrl.reserveUrl_Index;

                                // $('#spinn-loader').hide();
                            }, 1000);


                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
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
    });

});


$(document).on('click',
    '#btn_modifyreservation',
    function(e) {

        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Reservation..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation!'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modifyreseserve').attr('action');

                var form = $('[id*=modifyreseserve]');

                $.validator.unobtrusive.parse(form);
                form.validate();
                
                if (form.valid()) {


                    $.ajax({
                        type: 'POST',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        //contentType:'application/json;charset=utf-8',
                        success: function (data) {
                            if (data.success) {

                                //alert(data.success);

                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully updated!",
                                    type: "success"

                                });


                                setTimeout(function () {

                                    window.location.href = reservationUrl.reserveUrl_Index;

                                    // $('#spinn-loader').hide();
                                }, 1000);


                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
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
        });


    });