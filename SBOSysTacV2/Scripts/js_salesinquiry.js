$(document).ready(function() {

    function currencyFormat(num) {
        return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }

    $('#spinn-loader').show();

    $.ajax({
        type: 'Get',
        url: salessummaryscheduleUrl.url_load_loadingScheduleDataTable,
        //data: { datefrom: dateStart, dateTo: dateEnd },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#filteredRecord').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    }).done(function () {

        loadScheduleDateFilter();

        $('#spinn-loader').hide();
    });


    //Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD-MMM-YYYY", true).isValid();
    }

    var dateNow = new Date();
    //init datetimepcker

    //init datetimepcker
    $('#start_date').datetimepicker(
        {

           
            allowInputToggle: true,
            enabledHours: false,
            locale: moment().local('en'),
            format: "DD-MMM-YYYY",
            defaultDate: dateNow

        });

    //init datetimepcker
    $('#end_date').datetimepicker(
        {

            allowInputToggle: true,
            enabledHours: false,
            locale: moment().local('en'),
            format: "DD-MMM-YYYY",
            defaultDate: dateNow

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


    //$('#btnFilterschedule').on('click',
    //    function(e) {
    //        e.preventDefault();


    //        var dateStart = $('#start_date').val();
    //        var dateEnd = $('#end_date').val();


    //        if (dateStart === "" && dateEnd === "") {

    //            swal("Pls. enter date value!", "Unable to process request", "error");

    //        } else {

           

    //            $('#spinn-loader').show();

    //            $.ajax({
    //                type: 'Get',
    //                url: salessummarycheduleUrl.url_load_loadingSscheduleDataTable,
    //                //data: { datefrom: dateStart, dateTo: dateEnd },
    //                contentType: 'application/html;charset=utf8',
    //                datatype: 'html',
    //                cache: false,
    //                success: function (result) {

    //                    $('#filteredRecord').html(result);
    //                },
    //                error: function (xhr, ajaxOptions, thrownError) {
    //                    swal("Error on retrieving record!", "Please try again", "error");
    //                }


    //            }).done(function () {

    //                loadScheduleDateFilter(dateStart, dateEnd);

    //                $('#spinn-loader').hide();
    //            });

    //        }

    //    });//---- end button filter Click event


    function loadScheduleDateFilter() {
        
        if ($.fn.dataTable.isDataTable('#tbl_salessummary')) {

            $('#tbl_salessummary').DataTable().destroy();
            $('tbl_salessummary tbody').empty();

        }

        var tablesales = $('#tbl_salessummary').DataTable({
           
            "serverSide": false,
            "processing": true,
            "language": {

                'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },

            "dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>"
            ,
            "ajax":
            {
                "url": salessummaryscheduleUrl.url_loadsalessummarylist,
                "type": "Get",
                //"data": {startDate:dateStart,endDate: dateEnd },
                "datatype": "json"
            }
            ,
            "columnDefs":
            [
                {
                    'autowidth': true, 'targets': 0,
                    "data": "accountname"
                },

                {
                    'autowidth': true, 'targets': 1,
                    "data": "dateTrans",
                    "type": "date",
                    "render": function (d) {
                        return moment(d).format("MMM-DD-YYYY hh:mm: A");
                    }

                },
            
                {
                    'autowidth': true, 'targets': 2,
                    "data": "particular"
                },
                //{
                //    'autowidth': true, 'targets': 3,
                //    "data": "reference"
                //},

            
                {
                    'autowidth': true,
                    'targets': 3,
                    "orderable": false,
                    "className": "text-right",
                    "data": "CashSales",
                    render: $.fn.dataTable.render.number(",", ".", 2)

                },
                {
                    'autowidth': true,
                    'targets': 4,
                    "orderable": false,
                    "className": "text-right",
                    "data": "OnAccount",
                    render: $.fn.dataTable.render.number(",", ".", 2)

                }

            ]
            ,
            "footerCallback":function(row, data, start, end, display) {
                var api = this.api(), data;

                var initVal=function(i) {
                    return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                }

                var gTotalCash= api.column(3)
                    .data()
                    .reduce(function(a,b) {
                        return initVal(a)+initVal(b);
                    }, 0);

                var pageTotalCash= api.column(3, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return initVal(a) + initVal(b);
                    }, 0);

                //console.log(api.column(3).data());
                $(api.column(3).footer()).html('P ' + currencyFormat(pageTotalCash) + '(' + currencyFormat(gTotalCash) + ')');


                var gTotalonAccount = api.column(4)
                    .data()
                    .reduce(function (a, b) {
                        return initVal(a) + initVal(b);
                    }, 0);

                var pageTotalonAccount = api.column(4, { page: 'current' })
                    .data()
                    .reduce(function (a, b) {
                        return initVal(a) + initVal(b);
                    }, 0);


                $(api.column(4).footer()).html('P ' + currencyFormat(pageTotalonAccount) + '(' + currencyFormat(gTotalonAccount) + ')');
            }
          
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
                    customize: function(doc) {

                        doc.styles.title.fontSize = 12;
                        doc.defaultStyle.fontSize = 10;
                        doc.styles.tableHeader.fontSize = 10;

                        //console.log(doc.content);

                      

                        doc.content[1].table.widths =
                            Array(doc.content[1].table.body[0].length + 1).join('*').split('');

                     


                    }


                }
            ]
        });
    }
});