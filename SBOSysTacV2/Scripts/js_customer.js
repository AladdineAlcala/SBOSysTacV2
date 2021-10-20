
var $selectedObject;
var $customerList;

$(document).ready(function () {


    if ($.fn.DataTable.isDataTable('#table_customers')) {

        $('#table_customers').dataTable().fnDestroy();
        $('#table_customers').dataTable().empty();

    }

    $customerList = $('#table_customers').DataTable(

        {


            "serverSide": false,
            "processing": true,
            "language": {

                'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },
            "paging": true,
            "dom": "<'#customertop.row'<'col-sm-4'B><'col-sm-4'l><'col-sm-4'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#customerbottom.row'<'col-sm-5'i><'col-sm-7'p>>",
            "pagingType": "full_numbers",
            "ajax":
            {
                "url": customerUrl.url_customerloadDataTable,
                "type": "Get",
                "datatype": "json"
            },
            "columns": [
                { "data": null },
                {
                    "data": "c_Id"
                },
                {
                    "data": "lastname",
                    "className":"text-uppercase",
                    "render":function(data, type, customer) {
                        if (customer.middle === null) {
                            return customer.lastname + ' ,' + customer.firstname;
                        } else {
                            return customer.lastname + ' ,' + customer.firstname + ' ' + customer.middle;
                        }

                    }
                },
                { "data": "address" },
                {
                    "data": null,
                    "render": function(data, type, customer) {

                        if (customer.contact1 !== null && customer.contact2 !== null) {
                            return customer.contact1 + ' / ' + customer.contact2;

                        } else {
                            if (customer.contact1 !== null) {
                                return customer.contact1;
                            }
                            else if(customer.contact2!==null) {
                                return customer.contact2;
                            }
                        }

                        return " ";
                    }
                }
            ],
            "columnDefs": [
                {
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'width': '5%',
                    'className': 'select-checkbox',
                    'data': null,
                    'defaultContent': ''
                },
                { 'width': '10%', 'targets': 1 },
                { 'width': '30%', 'targets': 2 },
                { 'autowidth': true, 'targets': 3 },
                { 'autowidth':true, 'targets': 4 }
            ],
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-customerid', data.c_Id);
            },

            "order": [[ 1, "asc" ]],

            select: {
                style: 'os',
                selector: 'td:first-child'

            },

            buttons:
            [
                {
                    text: '<i class="fa fa-plus-square fa-fw"></i>',
                    className: 'btn btn-primary btnAddCustomer',
                    titleAttr: 'Add a new record',
                    action: function () {

                        window.location.href = customerUrl.url_customerAdd;

                    }
                },
                {
                    text: '<i class="fa fa-edit fa-fw"></i>',
                    className: 'btn btn-primary btnModifyCustomer',
                    titleAttr: 'Modify record',
                    action: function () {
                       
                        onModifyCustomer();

                    }, enabled: false
                },
                {
                    text: '<i class="fa fa-trash fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveCustomer',
                    titleAttr: 'Remove record',
                    action: function () {

                        //onremoveMenu();

                    }
                    , enabled: false
                }
                ,
                {
                    text: '<i class="fa fa-print fa-fw"></i>',
                    className: 'btn btn-primary btnRemoveCustomer',
                    titleAttr: 'Print record',
                    action: function () {
                      
                        //onremoveMenu();

                    }, enabled: false
                }
            ]



        }

    );

    //============================= end of code =================================


    var onModifyCustomer = function () {

        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedcustomerid = $this.attr('data-customerid');

            window.location.href = customerUrl.url_modifycustomer.replace("cusId", selectedcustomerid);

        }
    };




    $('#table_customers tbody').on('click', 'tr', function () {

        $selectedObject = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $customerList.button(0).enable();
            $customerList.button(1).disable();
            $customerList.button(2).disable();
            $customerList.button(3).disable();
        }

        else {
            $customerList.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $customerList.button(0).disable();
            $customerList.button(1).enable();
            $customerList.button(2).enable();
            $customerList.button(3).enable();

          
            $selectedObject = $(this);
        }


    });


    //Remove Customer
    $('.btnRemoveCustomer').on('click', function (e) {

        e.preventDefault();

    
        if ($selectedObject.hasClass('selected')) {

            var $this = $selectedObject;

            var selectedcustomerid = $this.attr('data-customerid');

           // console.log(selectedcustomerid);

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Customer..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Remove it!'
                //closeOnConfirm: true, closeOnCancel: true
            }).then((result) => {

                if (result.value) {
                        
                    $.ajax({
                        type: "post",
                        url: customerUrl.url_removecustomer,
                        ajaxasync: true,
                        dataType: 'json',
                        data: { customerId: selectedcustomerid },
                        //contentType: 'application/json; charset=utf-8',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                     

                                Swal.fire('Success', 'Record successfuly removed!', 'success');

                                $('#table_customers').DataTable().ajax.reload();

                                $customerList.button(0).enable();
                                $customerList.button(1).disable();
                                $customerList.button(2).disable();

                            }

                            else {

                      
                                Swal.fire('Failed', 'Unable to remove record with active customer..\n Pls contact admin', 'error');

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
                                    text: response.Error,
                                    type: "error"

                                });

                            } else {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }

                        }


                    });//-----ajax end


                }

            });//end then

        }

    });

    $('#btn_savecustomer').on('click', function (e) {

        e.preventDefault();

        var form = $(this).closest('form');


        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Customer Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {
                
                var formUrl = $('#newcustomer').attr('action');
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

                                Swal.fire('Success', 'Record successfuly added!', 'success');

                                window.location.href = customerUrl.url_customerIndex;

                
                            }
                            else {

                                Swal.fire('Failed',data.message, 'error');
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


    $('#btn_modifycustomer').on('click', function (e) {

        e.preventDefault();

        var form = $(this).closest('form');

       
        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Customer Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation !'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modifycustomer').attr('action');
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

                                Swal.fire('Success', 'Record successfuly Updated!', 'success');

                                window.location.href = customerUrl.url_customerIndex;

           

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

  

  


});//------ document end


