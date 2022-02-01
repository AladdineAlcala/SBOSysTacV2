var $selectedObject;

$(document).ready(function () {

    if ($.fn.DataTable.isDataTable('#table_course')) {

        $('#table_course').dataTable().fnDestroy();
        $('#table_course').dataTable().empty();

    }


    //----------- datatable load data -------------------

    $('#table_course').DataTable({
        paging: true,
        "lengthChange": false,
         //"dom": '<"top"Bf>rt<"bottom"lp><"clear">',

        "dom": "<'#coursetop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#coursebottom.row'<'col-sm-5'i><'col-sm-7'p>>",


        "pagingType": "simple_numbers",
        "columnDefs": [
            { "orderable": false, "targets": 0, "className": 'select-checkbox', "data": null, "defaultContent": '' },
            { "orderable": true, "targets": 1 },
            { "orderable": true, "targets": 2 },
            { "width": "5%", "targets": 0 },
            { "width": "9%", "targets": 1 },
            { "width": "18%", "targets": 4 }
        ],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        //dom: 'Bfrtip',
        buttons: [
            {
                text: '<i class="fa fa-plus-square fa-fw"></i>',
                className: 'btn btn-primary btnAddJob',
                titleAttr: 'Add a new record',
                action: function (dt, node, config) {

                    window.location.href = courseUrl.urlAdd;
                }
            },
            {
                text: '<i class="fa fa-edit fa-fw"></i>',
                className: 'btn btn-primary EditJob',
                titleAttr: 'Modify record',
                action: function (e, dt, node, config) {
                  
                    onmodifyCourse();
                }, enabled: false
            },
            {
                text: '<i class="fa fa-trash fa-fw"></i>',
                className: 'btn btn-primary btnRemoveJob',
                titleAttr: 'Remove record',
                action: function (e, dt, node, config) {

                    onremoveCourse();

                }
                , enabled: false
            }
        ]


    });

    //var is_maincourse = function (data, type, full, meta) {

    //    var is_extended = data === true ? "checked" : "";
    //    return '<input type="checkbox" class="checkbox" ' + is_extended + " disabled/>";
    //}


   



    var tablecourse = $('#table_course').DataTable();

    $('#table_course tbody').on('click', 'tr', function () {

        $deleteObj = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            tablecourse.button(0).enable();
            tablecourse.button(1).disable();
            tablecourse.button(2).disable();
        }

        else {
            tablecourse.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            tablecourse.button(0).disable();
            tablecourse.button(1).enable();
            tablecourse.button(2).enable();

            $deleteObj = $(this);
        }


    });


    function onremoveCourse() {

        if ($deleteObj.hasClass('selected')) {

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm Removing This Course..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Remove it!'
                //closeOnConfirm: true, closeOnCancel: true
            }).then((result) => {

                if (result.value) {

                    var cId = $deleteObj.closest("tr").find('td:eq(1)').text();
                    var row = $deleteObj.closest('tr');
                    var api = $('#table_course').DataTable();


                    $.ajax({

                        type: "post",
                        url: courseUrl.urlRemove,
                        ajaxasync: true,
                        data: { courseId: cId },
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                    title: 'Deleted!',
                                    text: data.deletedcourse + ' was deleted successfully.',
                                    type: 'info'
                                });
                                api.row(row).remove().draw();

                                //===== refresh button delete and edit ======================


                            } else {

                                //swal("Unable to remove this record! course is in used", "Please try again or contact sys admin", "error");
                                Swal.fire('Unable to remove',data.deletedcourse +' is in used \n Please try again or contact sys admin', 'error');

                            }

                            tablecourse.button(0).enable();
                            tablecourse.button(1).disable();
                            tablecourse.button(2).disable();
                        },
                        error: function (data) {
                            alert(data);
                        }

                    });



                }

            });//end then
         
        }
    }


    var onmodifyCourse = function() {

        if ($deleteObj.hasClass('selected')) {

            var cId = $deleteObj.closest("tr").find('td:eq(1)').text();
            //var row = $deleteObj.closest('tr');
            //var api = $('#table_course').DataTable();

            window.location.href = courseUrl.urlUpdateCourse.replace("cId", cId);
        }
    };



    //update course

    $('#btn_updateCourse').on('click', function (e) {

        e.preventDefault();

        var form = $(this).closest('form');


        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Course Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation !'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modify_course').attr('action');
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

                                //alert(data.success);

                                Swal.fire('Success', 'Record successfuly Updated!', 'success');

                                window.location.href = courseUrl.urlcourseIndex;


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



});//===document end

$(document).on('click','#btn_regCourse', function (e) {

    e.preventDefault();

    var form = $(this).closest('form');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Course..",
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

                            Swal.fire('Success', 'Record successfuly added!', 'success');

                            window.location.href = courseUrl.urlcourseIndex;


                        }
                        
                        else {

                            Swal.fire('Failed',data.message, 'error');
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        swal("Error adding record!", "Please try again", "error");
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
    }); //end swal


});