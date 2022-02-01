
$(document).ready(function () {

    var currentYear = new Date().getFullYear();
  
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
    ];


    var itemMonths = [],
        itemCountList = [];
    var end = 11;
    var month;
    var year = currentYear;


    var start = 0;

    var Result = [];


    $.ajax({

        type: 'Get',
        url: dashboardUrl.dashUrl_loadbarchart,
        contentType: "application/json",
        data: { thisYear: currentYear },
        dataType: "json",
        success: function (result) {

            //console.log(result);

            for (var i = 0; i < result.length; i++) {

                Result.push({
                        Months:monthName(result[i].Period),
                        Years: result[i].Year,
                        Counts: result[i].Count
                });

            }

            //console.log(Result);


            for (var i = 0; i < 12; i++) {

                var months = monthNames[start];
                var monthValue = 0;
                itemMonths.push(months + year);
                start = start + 1;

                if (start == 12) {
                    start = 0;
                    year = year + 1;
                }

                var dataObj = $.grep(Result, function (a) {
                    return a.Months == months

                })[0];

                var monthValue = dataObj !== undefined ? dataObj.Counts : 0;

                itemCountList.push(monthValue);

            } // end loop

            //console.log(itemCountList);

            var mybookings_barChart = null;

            var ctx = document.getElementById("bookings_Chart").getContext("2d");

            // start chart
            mybookings_barChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: itemMonths,
                    datasets: [{
                        label: 'Total Count',
                        backgroundColor: "#26B99A",
                        data: itemCountList
                    }]
                },

                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                steps: 10,
                                stepValue: 5,
                                max: 100

                            }
                        }],
                        xAxes: [{

                            steps: 10,
                            stepValue: 5,
                            max: 12
                        }]

                    }
                }
            }); // end chart 


        }

    });


        function monthName(mon) {
        return ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][mon - 1];
    }

        var Ordercount_Result = [];
         var itemMenus = [],
                 itemCount = [];

    $.ajax({

        type: 'Get',
        url: dashboardUrl.dashUrl_getMenusOrderCount,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {


            //for (var i = 0; i < result.length; i++) {

            //    Ordercount_Result.push({
                  
            //        Menu: result[i].MenuName,
            //        Ordercounts: result[i].CountMenuOrder

            //    });

            //}//end for


            $.each(result,function(index, data) {
                itemMenus.push(data.MenuName);
                itemCount.push(data.CountMenuOrder);
            });

            //console.log(Ordercount_Result);


  

            var ordercountctx = document.getElementById("menus_ordercount").getContext("2d");


            ordercount_barChart = new Chart(ordercountctx, {
                type: 'horizontalBar',
                data: {
                    labels: itemMenus,
                    datasets: [{
                        label: 'Top 10 most ordered menu',
                        backgroundColor: "#26B99A",
                        data: itemCount
                    }]
                },

                options: {
                    scales: {
                        yAxes: [{
                          stacked:true
                        }]

                        ,
                        xAxes: [{

                            ticks: {
                                beginAtZero: true,
                                steps: 10,
                                stepValue: 5,
                                max: 100

                            }
                        }]

                    }
                }
            }); // end char



        }//end sucess

    });// end ajax



    if ($.fn.DataTable.isDataTable('#tbl_dashBooking')) {

        $('#tbl_dashBooking').dataTable().fnDestroy();
        $('#tbl_dashBooking').dataTable().empty();

    }

    var tabledashbookings = $('#tbl_dashBooking').DataTable(
                                            {
                                                "info": false,
                                                "serverSide": false,
                                                "searching": false,
                                                "paging": true,
                                                "sDom":'<"header">',
                                             
                                                "ajax":
                                                {
                                                    "url": dashboardUrl.dashUrl_getBookingsToday,
                                                    "type": "Get",
                                                    "datatype": "json"
                                                }

                                                ,
                                                "columnDefs":
                                                [
                                                   

                                                    {
                                                        'autowidth': true, 'targets': 0,
                                                        "data": "cusfullname"

                                                    },
                                                    {
                                                        'autowidth': true, 'targets': 1, "data": "occasion"

                                                    },
                                                    {
                                                        'autowidth': true, 'targets': 2,
                                                        "data": "bookdatetime",
                                                        "type": "date",
                                                        "render": function (d) {
                                                            return moment(d).format("MMM-DD-YYYY hh:mm: A");
                                                        }

                                                    },

                                                    {
                                                        'autowidth': true, 'targets': 3,
                                                        "data": "package"

                                                    }
                                                     ,

                                                    {
                                                        'autowidth': true,
                                                        'targets': 4,
                                                        "type": "num-fmt",
                                                        "orderable": false,
                                                        "className": "text-right",
                                                        "data": "packageDue",
                                                        render: $.fn.dataTable.render.number(",", ".", 2)

                                                    }
                                                    ,
                                                    {
                                                        'autowidth': true, 'targets': 5,
                                                        "data": null,
                                                        "className": "text-center",
                                                        "mRender": function (data, type, row) {
                                                            if (data.isServe === false && data.isCancelled === true) {

                                                                return 'Cancelled';

                                                            }
                                                            else if (data.isServe === false && data.isCancelled === false) {

                                                                return 'Unserve';
                                                            }
                                                            else {
                                                                return 'Served';
                                                            }
                                                        }

                                                    }
                                                ]

                                            });
    $('div.header').html('List of Bookings Today');
    $('div.header').css({
        "font-weight": "bold",
        "padding-bottom":"12px;"
    });
});