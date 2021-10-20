var $tableinquiryreport = null;
var $cusId;

$(document).ready(function () {

    //console.log($('#hiddencusId').val());

    function currencyFormat(num) {
        return (num
            .toFixed(2)
            .replace('.', ',')
            .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        );
    }

    if ($.fn.dataTable.isDataTable('#tblcusrecievablesReport')) {

        $('#tblcusrecievablesReport').DataTable().destroy();
        $('#tblcusrecievablesReport tbody').empty();

    }

    $('#tblcusrecievablesReport').DataTable({

        "serverSide": false,
        "processing": true,

        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        }
        ,
        "dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>"
        
        ,
        "ajax": {

            "url": url_AccountRecievables.loadAccnrecievebyCusId,
            "type": "Get",
            "data": { cusId: $('#hiddencusId').val() },
            "datatype": "json"
        }
        ,
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
                "render": function(d) {
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
                'targets':5,
                "orderable": false,
                "className": "text-right",
                "data": "totalPayment",
                render: $.fn.dataTable.render.number(",", ".", 2)

            },
            {
                'autowidth': true,
                'targets':6,
                "orderable": false,
                "className": "text-right",
                "data":"balance",
                "render":function(data,type,row) {
                        
                    // negative value occurs here---------
                    if (row.balance < 0) {

                        return "w/ Refunded "+ currencyFormat(0);

                    } else {

                        if (row.iscancelled) {

                            if (row.refunds > 0) {

                                return "w/ Refunded " + currencyFormat(0);

                            } else {

                                return currencyFormat(row.balance);
                            }
                        }


                        else {
                            return currencyFormat(row.balance);
                        }
                           
                    }

                       
                }

                //render: $.fn.dataTable.render.number(",", ".", 2)

            }
        ]
        ,
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            var initVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            }

            var gTotalAmountDue = api.column(4)
                .data()
                .reduce(function (a, b) {
                    return initVal(a) + initVal(b);
                }, 0);

           // console.log(api.column(4).data());

            //var pageTotalCash = api.column(3, { page: 'current' })
            //    .data()
            //    .reduce(function (a, b) {
            //        return initVal(a) + initVal(b);
            //    }, 0);

          
            $(api.column(4).footer()).html('P ' + currencyFormat(gTotalAmountDue));


            var gTotalAmountPaid = api.column(5)
                .data()
                .reduce(function (a, b) {
                    return initVal(a) + initVal(b);
                }, 0);

            // console.log(api.column(4).data());

            //var pageTotalCash = api.column(3, { page: 'current' })
            //    .data()
            //    .reduce(function (a, b) {
            //        return initVal(a) + initVal(b);
            //    }, 0);


            $(api.column(5).footer()).html('P ' + currencyFormat(gTotalAmountPaid));


            var gTotalAmountBal = api.column(6)
                .data()
                .reduce(function (a, b) {
                    return initVal(a) + initVal(b);
                }, 0);

            // console.log(api.column(4).data());

            //var pageTotalCash = api.column(3, { page: 'current' })
            //    .data()
            //    .reduce(function (a, b) {
            //        return initVal(a) + initVal(b);
            //    }, 0);


            $(api.column(6).footer()).html('P ' + currencyFormat(gTotalAmountBal));

        }
        ,
        "buttons":
                [
                    {
                        text: '<i class="fa fa-print fa-sm fa-fw"></i>',
                        className: 'btn btn-primary btn-sm btnPrintSoa',
                        titleAttr: 'Print Statement of Account',
                        action: function () {

                            showReportRecievableClient($('#hiddencusId').val());

                        }
                    }
                 
                ]
        
       

    });


    function showReportRecievableClient(clientId) {


        window.location.href = "/Inquiry/PrintSoa?cusId=" + clientId;


        //$.ajax({
        //    type: 'Get',
        //    url: url_AccountRecievables.printSoaAccnrecievebyClient,
        //    data:{}


        //});
    }

   

});