
$tblMainCourse = null;
$selectedObj = null;
$selectedId = null;
var $tblBookMenu;

function RegisterAjaxFormEvents() {

    var form = $('[id*=frmaddmenus]');
    $.validator.unobtrusive.parse(form);

}

function RegisterAjaxFormEventsModify() {

    var form = $('[id*=frmmodifymenus]');
    $.validator.unobtrusive.parse(form);

}

function OnSuccess(data) {

    //debugger;

            if (data.isRecordExist === false) {

                        Swal.fire({
                            title: "Success",
                            text: "It was succesfully added!",
                            type: "success"


                        });

                        setTimeout(function() {

                            //LoadDataTabletoModal();

                                $('#bookmenus').load(data.url);
                              
                                // $('#spinn-loader').hide();

                            },
                            1000);

     
            }
            else {

                Swal.fire({
                    title: "Failed",
                    text: data.ShowErrMessageString["param2"],
                    type: "warning"


                });

            }
            
            $('#txtselectedmenu').val(" ");
            $('#modal-searchPackageBooking').modal('hide');
            $selectedObj = null;
            $selectedId = null;
}


function ModifySuccess(data) {

    debugger;

    if (data.success === false) {

        Swal.fire({
            title: "Failed",
            text: data.StatMessageString["param2"],
            type: "warning"


        });

    } else {

        Swal.fire({
            title: "Success",
            text: data.StatMessageString["param2"],
            type: "info"


        });

        setTimeout(function () {

                //LoadDataTabletoModal();

                $('#bookmenus').load(data.url);

                // $('#spinn-loader').hide();

            },
            1000);


        $('#txtselectedmenu').val(" ");
        $('#modal-searchPackageBooking').modal('hide');
        $selectedObj = null;
        $selectedId = null;
    }
}


function OnFailure(data) {

            Swal.fire({

                title: "Failed",
                text: "HTTP Status Code:" + data.param1 + '  ' + data.param2,
                type: "warning"


            });
            $('#txtselectedmenu').val(" ");

            $selectedObj = null;
            $selectedId = null;
}





$(document).on('click', '#tbl-maincourse tbody tr', function () {

    $selectedObj = $(this);

    $selectedId = "";

   

        if ($selectedObj.hasClass('selected')) {
            var tr = $(this).closest('tr');
            var id = tr.children('td:eq(0)').attr('data-id');

            $('#hiddenmenuId').val(id);

            $selectedId = id;

            $('#txtselectedmenu').val(tr.children('td:eq(1)').html());

            //alert(tr.children('td:eq(1)').html());

            $('#txtserving').val('1');

        } else {
            $('#hiddenmenuId').val(" ");
            $('#txtselectedmenu').val(" ");
            $selectedId = "";
            $('#txtserving').val(' ');
        }

    //  console.log($selectedId);

});


//=======================================================================================================================
// ====================  Function Discription : Add new menu to list of menus selected by customer..
// ====================  Fucntion Controller : Bookings
//=====================  Function Action :  GetListofCourse
//=====================  Function Parameter :  
//=======================================================================================================================

$(document).on('click', '#menu_add', function (e) {
    e.preventDefault();

    debugger;

    var elem = $(this).closest('li').find('div[id=course]');
    var courseId = elem.attr('data-id');

    //  <<<<<=========      urlSearchpackagebookingbybookNo: "@Url.Action("GetListofCourse", "Bookings")",

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlSearchpackagebooking,  
        contentType: 'application/html;charset=utf8',
        data: { transactionId: $(this).closest('div.tools').data('transid'), courseId: courseId },
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-searchPackageBooking');

            modal.find('#modalPackagecontent').html(result);

            //var insertopt = 0;
            RegisterAjaxFormEvents();

            LoadDataTabletoModal(courseId);

            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');

        }, error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error adding record!', 'Please try again', 'error');
        }



    });



});



$(document).on('click', '#menu_change', function (e) {
    e.preventDefault();

    // var menu_No = $(this).attr("data-menuid");
    // data: { transactionId: $(this).closest('td').attr('data-id'), bookmenuNo: $(this).attr("data-menuid") },
    debugger;

    var elem = $(this).closest('li').find('div[id=course]');
    var selectedcourseId = elem.attr('data-id');


    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlSearchpackagebookingbybookNo,    //  <<<<<=========      urlSearchpackagebookingbybookNo: "@Url.Action("GetListofCourseforChange", "Bookings")",
        contentType: 'application/html;charset=utf8',
        data: { bookmenuNo: $(this).attr("data-menuid") },
        datatype: 'html',
        cache: false,
        success: function (result) {

            //var modal = $('#modal-change_menu');

            //modal.find('#modal_changemenu_content').html(result);

            var modal = $('#modal-searchPackageBooking');

            modal.find('#modalcontent').html(result);


            RegisterAjaxFormEventsModify();

            LoadDataTabletoModal(selectedcourseId);


            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');

        }, error: function (xhr, ajaxOptions, thrownError) {

            Swal.fire('Error adding record!', 'Please try again', 'error');
        }



    });



});


function LoadDataTabletoModal(_courseId) {
   
    if ($.fn.dataTable.isDataTable('#tbl-maincourse')) {

        // console.log('DataTable');
        $('#tbl-maincourse').DataTable().destroy();
        $('#tbl-maincourse tbody').empty();

    }

    debugger;

    //var courseid = _courseId;

    $tblMainCourse = $('#tbl-maincourse').DataTable(
        {

            bLengthChange: false,
            select: {
                style: 'os',
                //style: 'multi',
                selector: 'td:first-child'
            }
            ,
            //"dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
            "dom": "<'row'<'col-sm-12'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",

            //==============>>>>>>  "@Url.Action("LoadListMenus", "Bookings")",  <<<<<<<<<<<<<<<<<<<<<
            "ajax":
            {
                "url": PackageBookingUrl.urlMenuList,   
                "data": { courseId: _courseId},
                 "type": "Get",
                 "datatype": "json"
            },

            //  "aoColumns": [{   "sTitle": "<input type='checkbox' id='selectAll'></input>"}],

            "columnDefs":
            [
                {
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'width': '2%',
                    'className': 'select-checkbox',
                    'data': null,
                    'defaultContent': ''


                },
                {
                    'autowidth': true, 'targets': 1,
                    "data": null,
                    "render": function (data, type, full) {

                        if (full.isMainMenu === true) {

                            return full.menu_name + " ( Main Menu )";
                        } else {

                            return full.menu_name;
                        }
                    }

                },
                {
                    'autowidth': true, 'targets': 2,
                    "data": "course"
                }
                //,
                //{

                //    "data": "menuId",'visisble':false,'targert':3
                //}


            ],
            createdRow: function (row, data, indice) {
                $(row).find("td:eq(0)").attr('data-id', data.menuId);
            }
           

        });


   

}




function LoadDataTabletoModalModify() {

    if ($.fn.dataTable.isDataTable('#tbl-maincourse')) {

        // console.log('DataTable');
        $('#tbl-maincourse').DataTable().destroy();
        $('#tbl-maincourse tbody').empty();

    }

    //var opt = 1;
    var opturl = new String;

    //if (opt === 1) {
    //    opturl = PackageBookingUrl.urlMenuList_Modify.replace("courseid",);

    //} else {

    //}

    $tblMainCourse = $('#tbl-maincourse').DataTable(
        {
            bLengthChange: false,

            select: {
                style: 'os',
                //style: 'multi',
                selector: 'td:first-child'
            }
            ,
            //"dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
            "dom": "<'row'<'col-sm-3'><'col-sm-9'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",


            "ajax":
            {
                "url": PackageBookingUrl.urlMenuList,  //==============>>>>>>  "@Url.Action("LoadListMenus", "Bookings")",  <<<<<<<<<<<<<<<<<<<<<
                "type": "Get",
                "datatype": "json"
            },

            //  "aoColumns": [{   "sTitle": "<input type='checkbox' id='selectAll'></input>"}],

            "columnDefs":
            [
                {
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'width': '2%',
                    'className': 'select-checkbox',
                    'data': null,
                    'defaultContent': ''


                },
                {
                    'autowidth': true, 'targets': 1,
                    "data": null,
                    "render": function (data, type, full) {

                        if (full.isMainMenu === true) {

                            return full.menu_name + " ( Main Menu )";
                        } else {

                            return full.menu_name;
                        }
                    }

                },
                {
                    'autowidth': true, 'targets': 2,
                    "data": "course"
                }
                //,
                //{

                //    "data": "menuId",'visisble':false,'targert':3
                //}


            ],
            createdRow: function (row, data, indice) {
                $(row).find("td:eq(0)").attr('data-id', data.menuId);
            }


        });


    


}






$(document).on('click', '#addon_Information', function (e) {

    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlAddOnsInformation,
        contentType: 'application/html;charset=utf8',
        data: { transactionId: $(this).data('id') },
        datatype: 'html',
        cache: false,
        success: function (result) {
            var modal = $('#modal-Addons');

            modal.find('#modal_addoncontents').html(result);

            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
        }

    });
});

$(document).on('click', '#addon_upgrades', function (e) {

    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlAddOnsUpgrades,
        contentType: 'application/html;charset=utf8',
        data: { transactionId: $(this).data('id') },
        datatype: 'html',
        cache: false,
        success: function (result) {
            var modal = $('#modal-Addons');

            modal.find('#modal_addoncontents').html(result);
            var tableSearchaddons = $('#tbl-addonsdetails').DataTable({ bLengthChange: false, bFilter: false });


            tableSearchaddons.columns.adjust();


            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
        }

    });
});

$(document).on('change',
    '#' +
    'selectlistcategoryaddons' +
    '',
    function(e) {

        e.preventDefault();
        e.stopPropagation();

        if ($.fn.dataTable.isDataTable('#tbl-addonsdetails')) {

            // console.log('DataTable');


            $('#tbl-addonsdetails').DataTable().destroy();
            $('#tbl-addonsdetails tbody').empty();

        }

      

        var tableSearchaddons = $('#tbl-addonsdetails').DataTable({
                                        destroy: true,
                                        responsive: true,
                                        bLengthChange: false,
                                        bFilter: false,
                                        ajax: {
                                            url: PackageBookingUrl.urlGetListAddonsbyCat,
                                            data: {addonCatId: $(this).val() },
                                            type: "Get",
                                            datatype: "json"
                                        },
                                        
                                        "columnDefs":
                                             [
                                                 //{
                                                 //    'autowidth': true, 'targets': 0,
                                                 //    'data': "No"

                                                 //},
                                                 {
                                                     'width': '20%', 'targets': 0,
                                                     'data': "addoncategory"

                                                 },
                                                 {
                                                     'autowidth': true, 'targets': 1,
                                                     'data': "AddonsDescription"

                                                 },
                                                 {
                                                     'autowidth': true, 'targets': 2,
                                                     'data': "Unit"

                                                 }
                                                 ,
                                                 {
                                                     'autowidth': true, 'targets': 3,
                                                     'data': "AddonAmount",
                                                     render: $.fn.dataTable.render.number(",", ".", 2)

                                                 }
                                                 ,
                                                 {
                                                     'width':'4%', 'targets': 4,
                                                     'data': "No",
                                                     'searchable': false,
                                                     'orderable': false,
                                                     'className': 'dt-body-center text-center',
                                                     render: function (data, type, row) {
                                                         var addonNo = data;

                                                         return '<button class="btn bg-olive btn-flat btn-sm" type="button" id="btn-selectaddon"  data-id="' + addonNo + '"> <i class="fa fa-check-square-o"></i> Select </button>';

                                                     }

                                                 }

                                               
                                             ]
                            
                                }
                                );//end table

    });


$(document).on('click', '#btn_saveaddOns',
    function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Addons..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#save_addons').attr('action');

                var form = $('[id*=save_addons]');

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
                                        text: "It was succesfully added!",
                                        type: "success"
                                 

                                    });

                                //$('#addonDescription').val("");

                                $('#addons').load(data.url);
                                $('#modal-Addons').modal('hide');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });
                } else {
                    $.each(form.validate().errorList,
                        function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                            $errorSpan.show();
                        });
                }

            }
        });


    });


// remove booking menus
$(document).on('click', '#menu_remove', function (e) {
    e.preventDefault();

    var menuNo = $(this).attr("data-menuid");

    Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Removing This Menu..",
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
                            url: PackageBookingUrl.urlRemoveBookingMenus,
                            ajaxasync: true,
                            data: { bookmenuNo: menuNo },
                            cache: false,
                            success: function (data) {

                                if (data.success) {

                                    Swal.fire({
                                        title: "Success",
                                        text: "It was succesfully removed!",
                                        type: "success"


                                        //  window.location.href = bookingsUrl.bookUrl_IndexLoad;
                                    });


                                    $('#bookmenus').load(data.url);

                                } else {
                                    Swal.fire({
                                        title: 'ERROR!',
                                        text: 'Unable to remove record..',
                                        type: 'warning'
                                    });
                                }
                            }
                            ,
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error removing record!', 'Please try again', 'error');
                            }

                        });

                    }

            });//end then

});


$(document).on('click', '#addons_remove', function (e) {
    e.preventDefault();

    var addonId = $(this).attr("data-id");

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Removing This Addon..",
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
                    url: PackageBookingUrl.urlRemoveBookingAddOns,
                    ajaxasync: true,
                    data: { addonNo: addonId },
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            Swal.fire({
                                title: "Success",
                                text: "It was succesfully removed!",
                                type: "success"
                            });


                            $('#addons').load(data.url);

                            setTimeout(function () { $("#modal-Addons").modal('hide') }, 300);
                        }
                        else {
                            Swal.fire({
                                title: 'ERROR!',
                                text: 'Unable to remove record..',
                                type: 'error'
                            });
                        }

                    }
                });

            }

        });//end then
        
        
  
});




$(document).on('click', '#btn_canceladdon', function (e) {
    e.preventDefault();

    setTimeout(function () { $("#modal-Addons").modal('hide') }, 300);
});


$(document).on('click', '#btn_cancelmodifyaddon', function (e) {
    e.preventDefault();

    setTimeout(function () { $("#modal-modifyAddons").modal('hide') }, 300);
});

$(document).on('click', '#btn_cancelselectedaddon', function (e) {
    e.preventDefault();

    setTimeout(function () { $("#modal-modifyAddons").modal('hide') }, 300);
});




$(document).on('click', '#addons_modify', function (e) {
    e.preventDefault();


                var itemNo = $(this).attr("data-id");

                $.ajax({
                    type: 'Get',
                    url: PackageBookingUrl.urlModifyBookingAddOns,
                    contentType: 'application/html;charset=utf8',
                    data: {addonItemNo: itemNo},
                    datatype: 'html',
                    cache: false,
                    success: function (result) {



                        var modal = $('#modal-modifyAddons');

                        modal.find('#modal_modifyaddoncontents').html(result);

                        modal.modal({
                            backdrop: 'static',
                            keyboard: false
                        }, 'show');
                        }

                    });


});



$(document).on('click', '#btn_modifyaddOns',
    function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Addons..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Update it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modify_addons').attr('action');

                var form = $('[id*=modify_addons]');

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
                                    text: "It was succesfully updated!",
                                    type: "success"


                                });

                                //$('#addonDescription').val("");

                                $('#addons').load(data.url);
                                $('#modal-modifyAddons').modal('hide');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });
                } else {
                    $.each(form.validate().errorList,
                        function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                            $errorSpan.show();
                        });
                }

            }
        });


    });







$(document).on('click', '#btn-selectaddon',function(e) {
        e.preventDefault();

        var addonId = $(this).attr("data-id");
        var transId = $('#transbookingId').val();

         //alert(addonId);   
        $('#modal-Addons').modal('hide');

            $('#modal-Addons').on('hidden.bs.modal',
                function() {
                    // Load up a new modal...
                
                    $.ajax({
                        type: 'Get',
                        url: PackageBookingUrl.urlGetSelectedAddons,
                        contentType: 'application/html;charset=utf8',
                        data: { selectedaddonId: addonId,bookId:transId},
                        datatype: 'html',
                        cache: false,
                        success: function (result) {
                            var modal = $('#modal-seletedaddons');

                            modal.find('#modal-content_selectedaddon').html(result);

                            modal.modal({
                                backdrop: 'static',
                                keyboard: false
                            }, 'show');
                        }

                    });


                  

             });

   

});


$(document).on('click', '#btn_regselctedaddons',
    function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Addons..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Saving!'
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#selectedaddonform').attr('action');

                var form = $('[id*=selectedaddonform]');

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
                                    text: "It was succesfully saved!",
                                    type: "success"


                                });

                                //$('#addonDescription').val("");

                                $('#addons').load(data.url);
                                $('#modal-seletedaddons').modal('hide');
                            }
                            else
                            {
                                Swal.fire({
                                    title: "Failed",
                                    text: "Add on selected already in the list",
                                    type: "error"


                                });

                                //$('#addonDescription').val("");

                                //$('#addons').load(data.url);
                                $('#modal-seletedaddons').modal('hide');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });
                } else {
                    $.each(form.validate().errorList,
                        function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                            $errorSpan.show();
                        });
                }

            }
        });


    });


$(document).on('click', '#btn_modifyselctedaddons',
    function (e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Modify Addons..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Update!'
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modifyselectedaddonform').attr('action');

                var form = $('[id*=modifyselectedaddonform]');

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
                                    text: "It was succesfully saved!",
                                    type: "success"


                                });

                                //$('#addonDescription').val("");

                                $('#addons').load(data.url);
                                $('#modal-modifyAddons').modal('hide');
                            }
                            else {
                                Swal.fire({
                                    title: "Failed",
                                    text: "Add on selected already in the list",
                                    type: "error"


                                });

                                //$('#addonDescription').val("");

                                //$('#addons').load(data.url);
                                $('#modal-seletedaddons').modal('hide');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });
                } else {
                    $.each(form.validate().errorList,
                        function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:#a94442'>" + value.message + "</span>");
                            $errorSpan.show();
                        });
                }

            }
        });


    });




$(document).on('keypress', '#addonselorderqty', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});


//LightBox Modal//

var lightBoxModal = document.getElementById("modalLightbox");

var imglightcontent = document.getElementById("imgcontent");
//var spanclose=document.getElementsByClassName("close")[0];

$(document).on('click', '.imglightbox', function (e) {
    e.preventDefault();

    debugger;

    var counter = $(this).closest('div').data('id');

    //console.log(counter);

    lightBoxModal.style.display = "block";
    currentSlide(counter);


});


$(document).on('click', '.menuimg-close', function (e) {
    e.preventDefault();

    //console.log($tblBookMenu);

    lightBoxModal.style.display = "none";


});


var slideIndex = 1;

function showSlides(n) {

    //debugger;

    var i;
    var slides = document.getElementsByClassName("slides");
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");

    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }

    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";

    dots[slideIndex - 1].className += " active";
    captionText.innerHTML = dots[slideIndex - 1].alt;
}

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}


// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}


InsertImages(imglightcontent, $tblBookMenu);

function InsertImages(element, object) {

    debugger;
    var imagesdemo = "";
    let i = 0;

    object.forEach(function (obj) {
        //console.log(obj);
        i += 1;
        var nophoto = 'no-Image.jpeg';

        var imagefilename = obj.menuImageFilename !== null ? obj.menuImageFilename : nophoto;

        var imagescontainer = '<div class="slides">' +
            '<img src=/Content/UploadedImages/' + imagefilename + ' style="width:100%;">' +
            '</div>';

        imagesdemo += '<div class="column"><img class="demo" src=/Content/UploadedImages/' + imagefilename + ' alt="' + obj.menu_name + '" onclick="currentSlide(' + i + ')"></div>';




        element.insertAdjacentHTML('beforeend', imagescontainer);

    });


    var positioner = '<a class="menuimg-prev" onclick="plusSlides(-1)">&#10094;</a>' +
        '<a class="menuimg-next" onclick="plusSlides(1)">&#10095;</a>';

    var caption = '<div class="caption-container"><p id="caption"></p> </div>';

    element.insertAdjacentHTML('beforeend', positioner);
    element.insertAdjacentHTML('beforeend', caption);

    element.insertAdjacentHTML('beforeend', imagesdemo);


}




