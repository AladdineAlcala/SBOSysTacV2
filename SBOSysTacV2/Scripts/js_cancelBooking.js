var $cancelledList;

$(document).ready(function () {


    $('#canceldatepicker').datepicker("setDate", new Date());
    $('#canceldatepicker').datepicker({ autoclose: true, format: 'mm/dd/yyyy' });


    ////Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        //return this.optional(element) || moment(value, "DD-MMM-YYYY HH:mm A", true).isValid();
        return this.optional(element) || moment(value, "MM/DD/YYYY", true).isValid();
    }

    if ($.fn.DataTable.isDataTable('#cancelledlist')) {

        $('#cancelledlist').dataTable().fnDestroy();
        $('#cancelledlist').dataTable().empty();

    }


    $cancelledList =$('#cancelledlist').DataTable({
        "serverside": false,
        "processing": true,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },
        "ajax": {
            "url": url_cancelledbooking.bookingListIndex,
            "type": 'Get',
            "datatype":'json'
        }
        ,
        "columnDefs":
       [
           {
               'targets': 0,
               'width':'10%',
               'data': 'TransId'

           },
           {
               'targets': 1,
               'width':'25%',
               'data': 'CustomerFullname'

           }
           
           ,
           {
               'targets':2,
               'autowidth': true,
               'data': 'ReasonforCancel'

           }
           ,
           {
               'targets':3,
               'width': '10%',
               'data':'CancelDate',
               'datatype':'date',
               'render':function(d) {
                   return moment(d).format("MMM-DD-YYYY");
               }

           }

          
       ]
        

    });



    //========================== Save Cancelled Booking =================================================================

    $('#btn_savecanceledbooking').on('click', function (e) {

        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Operation..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                debugger;

                var formUrl = $('#formcancelbooking').attr('action');

                var form = $('[id*=formcancelbooking]');

                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    $.ajax({
                        type:'POST',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully cancelled!",
                                    type: "success"


                                });

                                window.location.href = url_cancelledbooking.bookUrl_IndexLoad;
                             


                            }
                        }, error: function (xhr, status, errorThrown) {

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

            }

            else {

            }


        });


    });


    //==============================================================================================================================



});