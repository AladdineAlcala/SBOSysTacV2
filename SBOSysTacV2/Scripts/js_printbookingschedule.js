var $tblbookingSchedule = null;

$(document).ready(function() {

    //Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD-MMM-YYYY", true).isValid();
    }

    //init datetimepcker
    $('#start_date').datetimepicker(
        {

            format: "DD-MMM-YYYY"

        });

    //init datetimepcker
    $('#end_date').datetimepicker(
        {

            format: "DD-MMM-YYYY"

        });


   

 


    $.fn.dataTable.moment = function (format, locale) {

        var types = $.fn.dataTable.ext.type;

        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ? 'moment-' + format : null;
        });

        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    }


    $('#btnFilterschedule').on('click', function (e) {
        e.preventDefault();

     

        var dateStart = $('#start_date').val();
        var dateEnd = $('#end_date').val();

      

        var optselected = $('input[type="radio"][name="optfilter"]:checked').val();

       

        if (dateStart == "" && dateEnd == "") {

            swal("Pls. enter date value!", "Unable to process request", "error");

        } else {

            //var date_Start = moment(dateStart).format("MMM-DD-YYYY hh:mm: A");

            //var date_End = moment(dateStart).format("MMM-DD-YYYY hh:mm: A");
          
            $('#spinn-loader').show();

            $.ajax({
                type: 'Get',
                url: bookingscheduleUrl.url_loadbookingSheduleFilterbyDate,
                //data: { start: dateStart, end: dateEnd },
                contentType: 'application/html;charset=utf8',
                datatype: 'html',
                cache: false,
                success: function (result) {

                    $('#filteredRecord').html(result);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    swal("Error on retrieving record!", "Please try again", "error");
                }


            }).done(function () {

                loadScheduleDateFilter(dateStart, dateEnd, optselected);

                $('#spinn-loader').hide();
            });

        }
       
    });//---- end button filter Click event

    function loadScheduleDateFilter(dateStart,dateEnd,optfilter) {

        if ($.fn.dataTable.isDataTable('#tbl_bookingschedule')) {

            $('#tbl_bookingschedule').DataTable().destroy();
            $('tbl_bookingschedule tbody').empty();

        }

      


        $tblbookingSchedule = $('#tbl_bookingschedule').DataTable({

            "serverSide": false,
            "processing": true,
            "language": {

                'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },

            "dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",

            "ajax":
            {
                "url": bookingscheduleUrl.url_loadbookingscheduleDataTable,
                "type": "Get",
                "data": { startDate: dateStart, endDate: dateEnd, filter: optfilter },
                "datatype": "json"
            },

            "columnDefs":
            [

                {
                    'autowidth': true, 'targets':0,
                    "data": "bookdatetime",
                    "type": "date",
                    "render": function (d) {
                        return moment(d).format("MMM-DD-YYYY hh:mm: A");
                    }

                },
                {
                    'autowidth': true, 'targets': 1,
                    "data": "transId"
                },

                {
                    'autowidth': true, 'targets': 2,
                    "data": "cusfullname"
                },

                {
                    'autowidth': true, 'targets': 3,
                    "data": "occasion"
                },
                {
                    'autowidth': true, 'targets': 4,
                    "data": "venue"
                },
                {
                    'autowidth': true, 'targets': 5,
                    "data": "package"
                },
                {
                    'autowidth': true,
                    'targets': 6,
                    "orderable": false,
                    "className": "text-right",
                    "data": "packageDue",
                    render: $.fn.dataTable.render.number(",", ".", 2)

                }

            ],

            "footerCallback": function (row, data, start, end, display) {
                var numFormat = $.fn.dataTable.render.number('\,', '.', 2).display;
                var api = this.api(), data;

                var intVal = function (i) {
                    return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                };

                total_Package = api.column(6).data().reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    },
                    0).toFixed(2);



                total_Package = parseFloat(total_Package);



                //update footer
                $(api.column(6).footer()).html(numFormat(total_Package));


                // $('#totalPayment').html(total_Payment.toFixed(2));
            }
            ,
            buttons:
            [
               
                {
                    extend: 'pdfHtml5',
                    footer:true,
                    text: '<i class="fa fa-print fa-sm fa-fw"></i>',
                    className: 'btn btn-primary btn-sm btnPrintPayment',
                    titleAttr: 'Print to PDF',
                    download: 'open',
                    orientation:'landscape',
                    exportOptions: {
                        columns: ':visible'
                    }
                    ////,
                    ////customize: function (win) {
                    ////    $(win.document.body)
                    ////        .css('font-size', '10pt')
                           

                    ////    $(win.document.body).find('table')
                    ////        .addClass('compact')
                    ////        .css('font-size', 'inherit');
                    ////}

                   


                }
            ]

        });

    }


});//=========== end document ready ===================
