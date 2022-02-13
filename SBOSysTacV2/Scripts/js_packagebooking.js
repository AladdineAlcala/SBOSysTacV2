
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

function BookmenuAddSuccess(data) {

    debugger;

            if (data.isRecordExist === false) {

                        Swal.fire({
                            title: "Success",
                            text: "Menu was successfully added!",
                            type: "success"


                        });

                loadUrl(data.packageType, data.url);

     
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


        loadUrl(data.packageType,data.url);

        $('#txtselectedmenu').val(" ");
        $('#modal-searchPackageBooking').modal('hide');
        $selectedObj = null;
        $selectedId = null;
    }
}

function loadUrl(ptype,url) {

    setTimeout(function () {

/*        console.log(ptype);*/

        if (ptype != "sd") {

            $('#bookmenus').load(url);

        } else {

            $('#booking_details').load(url);
        }


    }, 1000);
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

        var trow = $(this).closest('tr');

        var id = trow.children('td:eq(0)').attr('data-id');

            $('#hiddenmenuId').val(id);

            $selectedId = id;

            $('#txtselectedmenu').val(trow.children('td:eq(1)').html());

        var data = $('#tbl-maincourse').DataTable().row(trow).data();
        var price = data['price'];

        if (!isNaN(parseFloat(price))) {

            $('#txtSprice').val(currencyFormat(price));
        } else {
            $('#txtSprice').val(currencyFormat(0));
        }
            

    } else {
            $('#hiddenmenuId').val(" ");
            $('#txtselectedmenu').val(" ");
            $selectedId = "";
            /*$('#txtserving').val(' ');*/
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

/*    debugger;*/

    var elem = $(this).closest('li').find('div[id=course]');

    var courseId = elem.attr('data-id');

   

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlSearchpackagebooking,   //  <<<<<=========      urlSearchpackagebookingbybookNo: "@Url.Action("GetListofCourse", "Bookings")",
        data: { transactionId: $(this).closest('div.tools').data('transid'), courseId: courseId, no_of_pax: $('#booking_no_of_pax').val() },
        contentType: 'application/html;charset=utf8',
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


/* This function update menus on packmeals / catering packages transaction */
$(document).on('click', '#menu_change', function (e) {
    e.preventDefault();


    var elem = $(this).closest('li').find('div[id=course]');
    var selectedcourseId = elem.attr('data-id');
    var selectedmenuId = $(this).attr("data-menuid");

    modifySelectedBookingMenu(selectedcourseId, selectedmenuId)

});

//  <<<<<=========    =====================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>

//----------------      PACKAGE MENU > MENU CHANGE OPTION  FUNCTION  -------------------------------------------------- 
var modifySelectedBookingMenu = function(selcourseId,selmenuId) {


    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlSearchpackagebookingbybookNo,     //  urlSearchpackagebookingbybookNo: "@Url.Action("GetListofCourseforChange", "Bookings")",
        contentType: 'application/html;charset=utf8',
        data: { bookmenuNo: selmenuId},
        datatype: 'html',
        cache: false,
        success: function (result) {

            //var modal = $('#modal-change_menu');

            //modal.find('#modal_changemenu_content').html(result);

            var modal = $('#modal-searchPackageBooking');

            modal.find('#modalPackagecontent').html(result);


            RegisterAjaxFormEventsModify();

            LoadDataTabletoModal(selcourseId);


            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');

        }, error: function (xhr, ajaxOptions, thrownError) {

            Swal.fire('Error adding record!', 'Please try again', 'error');
        }



    });

}

function LoadDataTabletoModal(_courseId) {
   
    if ($.fn.dataTable.isDataTable('#tbl-maincourse')) {

        // console.log('DataTable');
        $('#tbl-maincourse').DataTable().destroy();
        $('#tbl-maincourse tbody').empty();

    }

    

    //var courseid = _courseId;

    $tblMainCourse = $('#tbl-maincourse').DataTable(
        {
            bLengthChange: false,
            select: {
                style: 'os',
                //style: 'multi',
                selector: 'td:first-child'
            },
            //"dom": "<'row'<'col-sm-6'B><'col-sm-6'f>>" +
            "dom": "<'row'<'col-sm-12'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",


            "ajax":
            {
                "url":PackageBookingUrl.urlMenuList, //==============>>>>>>  "@Url.Action("LoadListMenus", "Bookings")",  <<<<<<<<<<<<<<<<<<<<<
                "data": { courseId: _courseId },
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
                ,
                {
                    'targets': [3],
                    "data": "price",
                    "visible": false

                }



            ],
            createdRow: function (row, data) {

                $(row).attr('data-id', data.course_id);
                $(row).find("td:eq(0)").attr('data-id', data.menuId);
            }
           

        });

}



/* This function will load when snacks and drinks transaction is selected */

function Load_SelectedMenusForPackage(_packageId) {

  /*  console.log(_packageId);*/

     if ($.fn.dataTable.isDataTable('#tbl-maincourse')) {
 
         // console.log('DataTable');
         $('#tbl-maincourse').DataTable().destroy();
         $('#tbl-maincourse tbody').empty();
 
     }

     $tblCourse = $('#tbl-maincourse').DataTable(
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
 
 
             "ajax":
             {
                 "url": PackageBookingUrl.urlCheckMenusByPackage,    //==============>>>>>>  "@Url.Action("GetPackageMenusperTransaction", "Bookings")"",  <<<<<<<<<<<<<<<<<<<<<
                 "data": { packageId: _packageId },
                 "type": "Get",
                 "datatype": "json"
                 //"dataSrc": function (response) {

                 //    console.log(response);
                 //}
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
                         'targets':[1],
                         'autowidth': true, 
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
                         'targets': [2],
                         'autowidth': true,
                         "data": "course"
                     },
                     {
                         'targets': [3],
                         "data": "price",
                         "visible": false

                     }
 
                 ],
             createdRow: function (row, data, indice) {
                 $(row).attr('data-id', data.course_id);
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
        url: PackageBookingUrl.urlAddOnsInformation, /*"@Url.Action("AddOnsInformation", "Bookings")",*/
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

//-------------------------------------------------------------------------------------------------------------------------------
//===============================================================================================================================
//=======================================  For Snack and Drinks Operation =======================================================
//==============================================================================================================================

//start

//---------------------- Add Snacks and Drinks ---------------------------------------------------------------------------

$(document).on('click', '#add_snacks_and_drinks', function (e) {

    e.preventDefault();

 /*   alert($('#booking_no_of_pax').val());*/

    var packageId = $('#pId').val();
    var _courseId = 0;

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlSearchpackagebooking,   //  <<<<<=========      urlSearchpackagebookingbybookNo: "@Url.Action("GetListofCourse", "Bookings")",\
        data: { transactionId: $(this).attr('data-id'), courseId: _courseId, no_of_pax: $('#booking_no_of_pax').val()},
        datatype: 'html',
        contentType: 'application/html;charset=utf8',
        cache: false,
        success: function (result) {

            var modal = $('#modal-searchPackageBooking');

            modal.find('#modalPackagecontent').html(result);

            //var insertopt = 0;
            RegisterAjaxFormEvents();


            Load_SelectedMenusForPackage(packageId);


            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');

        }, error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error adding record!', 'Please try again', 'error');
        }



    });


});


//---------------------- Modify Snacks and Drinks ---------------------------------------------------------------------------

$(document).on('click', '#menu_change_snacks_drinks', function(event) {

    event.preventDefault();

    var parenttr = $(this).closest('tr');
    var elem = $(this).closest('td');
    var selectedMenuId = elem.attr('data-id');
    var selectedcourseid = parenttr.attr('data-id');

    modifySelectedBookingMenu(selectedcourseid, selectedMenuId);

});



//---------------------- Remove Snacks and Drinks ---------------------------------------------------------------------------

$(document).on('click', '#menu_remove_snacks_drinks', function (event) {
    event.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Adding Payment..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Transaction..'

    }).then((result) => {

        if (result.value) {
            
            $('#spinn-loader').show();

            $.ajax({
                type: 'POST',
                url: PackageBookingUrl.urlBookingRemoveSnacksandDrinks,  // "@Url.Action("RemoveSnackandDrinks", "Bookings")",
                ajaxasync: true,
                data: { id: $(this).closest('td').attr('data-id') },
                cache: false,
                success: function (data) {

                    if (data.success) {

                        Swal.fire({
                            title: "Success",
                            text: "Data succesfully Added!",
                            type: "success"

                        });

                        console.log(data.url);

                        setTimeout(function () {

                            $('#booking_details').load(data.url);

                            $('#spinn-loader').hide();
                        }, 1000);

                      

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire('Error adding record!', 'Please try again', 'error');

                    $('#spinn-loader').hide();
                }
            });



        }



    });


});

//end

//========================================================================================================================
//------------------------------------------------------------------------------------------------------------------------

$(document).on('click', '#addon_upgrades', function (e) {

    e.preventDefault();

    // urlAddOnsUpgrades: "@Url.Action("Get_AddonsandUpgrades", "Bookings")",

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlAddOnsUpgrades,
        contentType: 'application/html;charset=utf8',
        data: { bookaddonNo: $(this).data('id') },
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
                                            url: PackageBookingUrl.urlGetListAddonsbyCat,   // urlGetListAddonsbyCat: "@Url.Action("Get_AddonsandUpgradesByCat", "Bookings")",
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
                                                     'data': "addonId",
                                                     'searchable': false,
                                                     'orderable': false,
                                                     'className': 'dt-body-center text-center',
                                                     render: function (data, type, row) {
                                                         var addonNo = data;
                                                        /* console.log(data);*/
                                                         return '<button class="btn bg-olive btn-flat btn-sm" type="button" id="btn-selectaddon"  data-id="' + addonNo + '"> <i class="fa fa-check-square-o"></i> Select </button>';

                                                     }

                                                 }

                                               
                                             ]
                            
                                }
                                );//end table

    });


$(document).on('click', '#btn_saveaddOns', function (e) {
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

                ///Bookings/SaveAddons

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

    var bookaddonNo = $(this).attr("data-id");

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


                //Check Selected Addon if has addon Details 



                $.ajax({

                    type: "post",
                    url: PackageBookingUrl.urlRemoveBookingAddOns,
                    ajaxasync: true,
                    data: { addonNo: bookaddonNo },
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            Swal.fire({
                                title: "Success",
                                text: "It was succesfully removed!",
                                type: "success"
                            });


                            $('#addons').load(data.url);


                            //setTimeout(function () { $("#modal-Addons").modal('hide') }, 300);
                        }
                        else {
                            Swal.fire({
                                title: 'Unable to remove record... !',
                                text: data.message,
                                type: 'error'
                            });
                        }

                    }
                });

            }

        });//end then
        
        
  
});

//========================= MODIFY ADDONS DETAILS =====================================================


$(document).on('click', '#addon-detail_Modify', function (e) {
    e.preventDefault();




    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlModifyBookingAddOnDetail,  /*"@Url.Action("ModifyAddonDetail", "Bookings")",*/
        contentType: 'application/html;charset=utf8',
        data: { selAddonDetailId: $(this).closest('tr').attr('data-id')},
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


//============================= UPDATE ADDONS DETAIL ==================================================

$(document).on('click', '#btn_updateselctedaddondetail', function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating Addons..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!',
        //closeOnConfirm: true, closeOnCancel: true
    }).then((result) => {

        if (result.value) {

            ///Bookings/UpdateSelectedAddonDetail

            var formUrl = $('#selectedaddonformmodify').attr('action');

            var form = $('[id*=selectedaddonformmodify]');

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
                                text: "It was succesfully Updated",
                                type: "success"


                            });

                            //$('#addonDescription').val("");

                            $('#addonsdetails').load(data.url[0]);

                            $('#addons').load(data.url[1]);



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



//========================= REMOVE ADDONDETAILS ========================================================

$(document).on('click', '#addon-detail_Remove', function (e) {
    e.preventDefault();

    var __targetElem = $(this).closest('tr');
    var __addondetailId = __targetElem.attr('data-id');
    var __transId = $('#hiddenTransId').val();


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
                 url: PackageBookingUrl.urlRemoveBookingAddOnDetail,
                 ajaxasync: true,
                 data: { addondetailId: __addondetailId, trans_id: __transId},
                 cache: false,
                 success: function (data) {
 
                     if (data.success) {

                         Swal.fire({
                             title: "Success",
                             text: "It was succesfully removed!",
                             type: "success"
                         });

                         setTimeout(function () {

                             console.log(data.url[1]);
                             $('#addons').load(data.url[1]);

                         }, 1000);

                         __targetElem.remove();
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





$(document).on('click', '#btn_canceladdon,#btn_cancelselectedaddondetail', function (e) {
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

        var selectedaddon_id = $(this).attr("data-id");
        var addon_no = $('#bookaddon_No').val();

         //alert(addonId);   
        $('#modal-Addons').modal('hide');


            $('#modal-Addons').on('hidden.bs.modal', function() {
                            // Load up a new modal...
                        
                                $.ajax({
                                    type: 'Get',
                                    url: PackageBookingUrl.urlGetSelectedAddons,    /* @Url.Action("GetSelectedAddons", "Bookings")"*/
                                    contentType: 'application/html;charset=utf8',
                                    data: { selectedaddonId: selectedaddon_id, addonNo: addon_no},
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

                var formUrl = $('#selectedaddonform').attr('action');     /*Bookings/SaveSelectedAddon*/

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


                              //  console.log(data.url[0]);

                                $('#addonsdetails').load(data.url[0]);

                                $('#addons').load(data.url[1]);

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
                        success: function(data) {
                            if (data.success) {

                                Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully saved!",
                                    type: "success"


                                });

                                //$('#addonDescription').val("");

                                $('#addons').load(data.url);
                               
                            } else {
                                Swal.fire({
                                    title: "Failed",
                                    text: "Add on selected already in the list",
                                    type: "error"


                                });


                             


                            }

                            $('#modal-modifyAddons').modal('hide');
                        },
                        error: function(xhr, ajaxOptions, thrownError) {
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


$(document).on('click', '#addon_otherCharges', function(e) {

    e.preventDefault();

        $.ajax({
            type: 'Get',
            url: PackageBookingUrl.urlAddBookingOtherCharge,
            contentType: 'application/html;charset=utf8',
            data: { transactionId:$(this).attr('data-id')},
            datatype: 'html',
            cache: false,
            success: function (result) {

                var modal = $('#modal-BookingOtherCharges');

                modal.find('#modal-BookingOthercharge-content').html(result);

                modal.modal({
                    backdrop: 'static',
                    keyboard: false
                }, 'show');
            }

        });

});


$(document).on('click', '#btn_registerNewCharge', function(event) {

    event.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Adding Miscellaneous Charge..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Transaction..'

        }).then((result) => {

            if (result.value) {


                var formUrl = $('#otherchargesForm').attr('action');
                var form = $('[id*=otherchargesForm]');

                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    $('#spinn-loader').show();

                    $.ajax({
                        type: 'POST',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                $('#booking_details').load(data.url);


                                setTimeout(function () {
                                 
                                    Swal.fire({
                                        title: "Success",
                                        text: "Data succesfully Added!",
                                        type: "success"

                                    });
 
                                    $('#spinn-loader').hide();

                                }, 500);


                                $('#modal-BookingOtherCharges').modal('hide');
                               
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');

                            $('#spinn-loader').hide();
                        }
                    });

                }

            }

        });

});




$(document).on('click', '#modify_otherCharge', function(event) {

    event.preventDefault();

    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlAddBookingModifyOtherCharge,
        contentType: 'application/html;charset=utf8',
        data: { id: $(this).closest('td').attr('data-id')},
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-BookingOtherCharges');

            modal.find('#modal-BookingOthercharge-content').html(result);

            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
        }

    });


});



$(document).on('click', '#btn_RegisterUpdatedCharge', function (event) {

    event.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating Miscellaneous Charge..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Transaction..'

    }).then((result) => {

        if (result.value) {


            var formUrl = $('#otherchargesForm').attr('action');
            var form = $('[id*=otherchargesForm]');

            $.validator.unobtrusive.parse(form);
            form.validate();

            if (form.valid()) {

                $('#spinn-loader').show();

                $.ajax({
                    type: 'POST',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            $('#booking_details').load(data.url);


                            setTimeout(function () {

                                Swal.fire({
                                    title: "Success",
                                    text: "Data succesfully Updated!",
                                    type: "success"

                                });

                                $('#spinn-loader').hide();

                            }, 500);


                            $('#modal-BookingOtherCharges').modal('hide');

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');

                        $('#spinn-loader').hide();
                    }
                });

            }

        }

    });

});

$(document).on('click', '#remove_otherCharge', function (event) {

    event.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing Miscellaneous Charge..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed Transaction..'

        }).then((result) => {

            if (result.value) {

            $('#spinn-loader').show();

            $.ajax({
                type: 'POST',
                url: PackageBookingUrl.urlAddBookingRemoveOtherCharge,
                ajaxasync: true,
                data: {id:$(this).closest('td').attr('data-id')},
                cache: false,
                success: function (data) {

                    if (data.success) {

                        $('#booking_details').load(data.url);

                        setTimeout(function () {

                            Swal.fire({
                                title: "Success",
                                text: "Data succesfully Added!",
                                type: "success"

                            });

                           

                            $('#spinn-loader').hide();

                        }, 500);

                        $('#modal-BookingOtherCharges').modal('hide');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire('Error adding record!', 'Please try again', 'error');

                    $('#spinn-loader').hide();
                }
            });

        }
    });

});


$(document).on('click', '#printContract', function(e) {
    e.preventDefault();


    $.ajax({
        type: 'Get',
        url: PackageBookingUrl.urlBookingPrintContractForm,
        contentType: 'application/html;charset=utf8',
        data: { transId: $(this).attr('data-id') },
        datatype: 'html',
        cache: false,
        success: function (result) {

            console.log(result);

            var modal = $('#modal-printForm');

            modal.find('#modal-content_printForm').html(result);

            modal.modal({
                backdrop: 'static',
                keyboard: false
            }, 'show');
        }

    });




});





$(document).on('click', 'a.printcontractOption', function (e) {

    e.preventDefault();
    e.stopPropagation();

    var transid = $('#hiddenid').val();

    var selopt = $('input[type="radio"][name="printopt"]:checked').val();

    var url = $(this).attr('href');

    window.location.href = url + '?transId=' + transid + '&selprintopt=' + selopt;

    //window.open(url + '?transId=' + transid + '&selprintopt=' + selopt);

    // window.location.href = url;
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

    /*debugger;*/

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

function currencyFormat(num) {
    return (num
        .toFixed(2)
       /* .replace('.', ',')*/
        .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
    );
}
/*InsertImages(imglightcontent, $tblBookMenu);*/

//function InsertImages(element, object) {

//   /* debugger;*/
//    var imagesdemo = "";
//    let i = 0;

//    object.forEach(function (obj) {
//        //console.log(obj);
//        i += 1;
//        var nophoto = 'no-Image.jpeg';

//        var imagefilename = obj.menuImageFilename !== null ? obj.menuImageFilename : nophoto;

//        var imagescontainer = '<div class="slides">' +
//            '<img src=/Content/UploadedImages/' + imagefilename + ' style="width:100%;">' +
//            '</div>';

//        imagesdemo += '<div class="column"><img class="demo" src=/Content/UploadedImages/' + imagefilename + ' alt="' + obj.menu_name + '" onclick="currentSlide(' + i + ')"></div>';




//        element.insertAdjacentHTML('beforeend', imagescontainer);

//    });


//    var positioner = '<a class="menuimg-prev" onclick="plusSlides(-1)">&#10094;</a>' +
//        '<a class="menuimg-next" onclick="plusSlides(1)">&#10095;</a>';

//    var caption = '<div class="caption-container"><p id="caption"></p> </div>';

//    element.insertAdjacentHTML('beforeend', positioner);
//    element.insertAdjacentHTML('beforeend', caption);

//    element.insertAdjacentHTML('beforeend', imagesdemo);


//}




