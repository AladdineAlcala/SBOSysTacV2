var _g_viewType = "package_cardview";

$(document).ready(function () {



    $('span.packageview >i').removeClass('text-blue');
    $('.icon-cardview').addClass('text-blue');
   

    $('input[name="radfilter"]').on('change', function (e) {

        e.preventDefault();
        e.stopPropagation();

        /*    debugger;*/

        $('#spinn-loader').show();


        var selected = $(this).attr('id');
        var _filter="";

        switch (selected) {

            case 'rad_wedd':
            _filter = 'wedding';
                break;

            case 'rad_premier':
                _filter = 'premier';
                break;

            case 'rad_vip':
                _filter = 'vip';
                break;
            case 'radregular':
                _filter = 'regular';
                break;

            case 'rad_packmeal':
                _filter = 'pm';
                break;

            default:
                _filter = 'all';
        }

      /*  console.log(_g_viewType);*/

        $.ajax({
            type: 'Get',
            url: PackageUrl.url_loadPackageCard,
            contentType: 'application/html;charset=utf8',
            data: { packagetype: _filter, _viewType: _g_viewType},
            datatype: 'html',
            cache: false,
            success: function (result) {
                $('#page_container').html(result);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error on retrieving record!', 'Please try again', 'error');
            }


        }).done(function () {

            setTimeout(function () {

                $('#spinn-loader').hide();

            }, 1000);

            
        });



    });

    //cancel booking click event

    $('#canceladdnewpackage').on('click',
        function (e) {

            e.preventDefault();

            Swal.fire({
                title: "Cancel New Package",
                text: "Yes..Cancel this package",
                type: "info"

            });


            setTimeout(function () {

                //debugger;

               // window.location.href = PackageUrl.url_index;
                window.location.href = "/Packages/Index";
                //  window.history.back();
                // $('#spinn-loader').hide();
            }, 500);


        });



    $('.packagetype').each(function(index,item) {
        $(item).click(function() {

            var vl = item.value;

            if (vl === 'vip') {

                $('#mincounthead').val(0);
                $('#mincounthead').prop('readonly',true);
            } else {
                $('#mincounthead').val(100);
                $('#mincounthead').prop('readonly',false);
            }
            //console.log(vl);
        });
    });


    $('#isActivePackage').on('change',function(e) {
        e.preventDefault();

        var packageid = $('#hidden_packageId').val();
        var _value = "";

            if ($(this).is(':checked')) {
                _value = "checked";
            } else {
                _value = "unchecked";
            //    alert('unchecked');
            }
            execActivePackage(_value, packageid);
        });

    
    function execActivePackage(value,packageId) {

        var stat = new Boolean(false);


        if (value === 'checked') {
            stat = true;
        }


        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Package Status..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed Operation!'

        }).then((result) => {

            if (result.value) {

                $.ajax({
                    type: 'POST',
                    url: PackageUrl.url_packageStatus,
                    data: { bolStat: stat, pId: packageId },
                    datatype: 'json',
                    cache: false,
                    success: function (data) {
                        if (data.success) {

                            Swal.fire({
                                title: "Success",
                                text: "It was succesfully Updated!",
                                type: "success"
                                //timer: "5000"
                            });


                            //reload partialview packageArea

                            $('#packagelocationTable').load(data.url);
                         

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                        if (xhr.status === 403) {

                            var response = $.parseJSON(xhr.responseText);

                            //  console.log(response);
                           // window.location = response.LogOnUrl;
                            Swal.fire({
                                title: "UnAuthorized Access",
                                text: data.Error,
                                type: "error"
                               
                            });

                        } else {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                       
                    }
                });


            }
        });



    }


    $.ajaxSetup({ cache: false });

    $('#btnAddPackage').on('click', function (e) {

      //  e.preventDefault();
        e.stopPropagation();

       // alert('asdasd');
        debugger;

        $('#newpackageform').load(this.href,
            function () {
                
                reparseform();
            });

    });


    function reparseform() {
        $("#new_packageForm").removeData("validator");
        $("#new_packageForm").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse('#new_packageForm');
        //console.log('sadasd');

    }


    //search package
    //$('#search_package').on('click', function() {


   

    //});


});

function formatRepo(item) {
    var markup = "<table class='item-result'><tr>";
    if (item.text !== undefined) {
        markup += "<div class='item-name' data-jtext=" + item.text + " data-jid=" + item.id + ">" + item.text + "</div>";
    }
    markup += "</td></tr></table>";
    return markup;
}

function formatRepoSelection(item) {
    return item.text;
}


$(document).on('click','#btn_savePackages', function (e) {

    e.preventDefault();

   var form = $(this).closest('form');

   Swal.fire({
       title: "Are You Sure ?",
       text: "Confirm Saving Packages..",
       type: "question",
       showCancelButton: true,
       confirmButtonColor: '#3085d6',
       cancelButtonColor: '#d33',
       confirmButtonText: 'Yes, Save it!'
    
   }).then((result) => {

       if (result.value) {

           var formUrl = $('#new_packageForm').attr('action');

           $.validator.unobtrusive.parse(form);

           form.validate();


           if (form.valid()) {

             //  console.log('valid form');
               //debugger;


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
                                   text: "It was succesfully Added!",
                                    type: "success",
                                    timer: "5000"
                           });

                           $('#newpackageform').load(data.url);

                         

                       }
                   },
                   error: function (xhr, ajaxOptions, thrownError) {
                       Swal.fire('Error adding record!', 'Please try again', 'error');
                   }
               });

           } else {
               $.each(form.validate().errorList, function (key, value) {
                   $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                   $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                   $errorSpan.show();
               });
           }


       }


   });//end swal

});



//form PackageBody Save Method
$(document).on('click', '#btn_savePackagedetails', function(e) {
    e.preventDefault();
    e.stopPropagation();

    var form = $(this).closest('form');


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Package Details..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!',
        //closeOnConfirm: true, closeOnCancel: true
    }).then((result) => {

        if (result.value) {

            var formUrl = $('#form_packagebody').attr('action');

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


                            // window.location.href = PackageUrl.url_details.replace("pId", data.packageId);

                            Swal.fire({
                                    title: "Success",
                                    text: "It was succesfully Added!",
                                    type: "success"
                             

                                });


                            setTimeout(function () {

                                window.location.href = PackageUrl.url_details.replace("pId", data.packageId);

                                // $('#spinn-loader').hide();
                            }, 500);



                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire("Error adding record!", "Please try again", "error");
                    }
                });

            } else {
                $.each(form.validate().errorList, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                    $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                    $errorSpan.show();
                });
            }

        }
    });

});///and remove


$(document).on('click','#btn_cancelPackagedetails',function(e) {

    e.stopPropagation();
    e.preventDefault();

    var form = $(this).closest('form');

    //form.submit();
    var formUrl = $('#form_packagebody').attr('action');
    //console.log(formUrl);

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

                    setTimeout(function () {

                        window.location.href = PackageUrl.url_details.replace("pId", data.packageId);

                        // $('#spinn-loader').hide();
                    }, 500);

                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error adding record!', 'Please try again', 'error');
            }
        });

    } else {
        $.each(form.validate().errorList, function (key, value) {
            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
            $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
            $errorSpan.show();
        });
    }



});


function PrivateCheck() {
    $(document).on('click','#chkExtended',function(e) {

        e.stopPropagation();


        if ($('[id*=chkExtended]').is(':checked')) {


            $('[id*=extAmount]').removeAttr('disabled');
            $('[id*=extAmount]').focus();
        }

        //} else {
            
        //    $('[id*=extAmount]').attr('disabled', 'disabled');

        //}


      
    });
}

$(document).on('click', '#btnSavelocation', function (e)
{
    e.preventDefault();

   
    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Saving Location Details..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'
        //closeOnConfirm: true, closeOnCancel: true
    }).then((result) => {

        if (result.value) {

            var formUrl = $('#packagelocationform').attr('action');

            var form = $('[id*=packagelocationform]');

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
                                    text: "It was succesfully Added!",
                                    type: "success"
                            

                                });

                            location.href = PackageUrl.url_details.replace("pId", data.packageId);
                            $('[id*=modal-addpackagecoverage]').modal('hide');

                       

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Saving record Failed!', 'Please try again', 'error');
                    }
                });

            } else {
                $.each(form.validate().errorList, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                    $errorSpan.html("<span style='color:yellow'>" + value.message + "</span>");
                    $errorSpan.show();
                });
            }

        }
    });

});

$(document).on('click','#package_remove',function(e) {

        e.preventDefault();

        var _packageId = $(this).data('id');

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Removing Package Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Remove it!'
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                $.ajax({

                    type: 'Post',
                    url: PackageUrl.url_remove,
                    ajaxasync: true,
                    data: { packageId: _packageId },
                    success: function (data) {

                        if (data.success) {

                            Swal.fire({
                                title: "Success",
                                text: "Package was succesfully remove!",
                                type: "success"
                               
                            });

                            setTimeout(function () {

                                window.location.href = PackageUrl.url_index;

                                // $('#spinn-loader').hide();
                            }, 500);

                           
                        }
                        else {

                            Swal.fire({
                                title: "Unable to Remove Package " + data.package_name,
                                text: "Package has current bookings!",
                                type: "warning"
                            });

                        }


                      

                    }
                    ,
                    error: function (xhr, status, errorThrown) {

                        // alert(xhr.status);

                        if (xhr.status === 403) {


                            var response = $.parseJSON(xhr.responseText);

                            //  console.log(response);
                            window.location = response.LogOnUrl;


                        }
                    }

                });

            }

        });//end then


});


$(document).on('click','#package_edit',function(e) {

    e.preventDefault();
    e.stopPropagation();

    var _packageId = $(this).data('id');

    window.location.href = PackageUrl.url_modifypackage.replace("pId", _packageId);

});


$(document).on('click', '#btn_modifyMainPackage', function (e) {

    e.preventDefault();

    
    var form = $(this).closest('form');


        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Packages Modification..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!'
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#modify_packageMain').attr('action');

                form.removeData("validator");
                form.removeData("unobtrusiveValidation");

                $.validator.unobtrusive.parse(form);
                form.validate();
                

                    if (form.valid()) {

                        //console.log('valid form');

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
                                            text: "It was succesfully Updated!",
                                            type: "success"
                       

                                        });

                                    setTimeout(function () {

                                        $('#packageform').load(data.url);

                                        // $('#spinn-loader').hide();
                                    }, 1000);



                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error on modifying record!', 'Please try again', 'error');
                            }
                        });

                    } else {
                        $.each(form.validate().errorList, function (key, value) {
                            $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                            $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                            $errorSpan.show();
                        });
                    }


            }
        });



});//modify package end


$(document).on('click','#btn_updatePackagedetails',function(e) {

    e.preventDefault();

    var form = $(this).closest('form');

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating Packages..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!'
        
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#form_modifypackagebody').attr('action');

                form.removeData("validator");
                form.removeData("unobtrusiveValidation");

                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    //console.log('valid form');

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
                                        text: "It was succesfully Updated!",
                                        type: "success"
                               

                                    });

                                setTimeout(function () {

                                    window.location.href = PackageUrl.url_details.replace("pId", data.packageId);

                                    // $('#spinn-loader').hide();
                                }, 500);


                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }
                    });


                } else {
                    $.each(form.validate().errorList, function (key, value) {
                        $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                        $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                        $errorSpan.show();
                    });
                }
            }
        });

 
});

$(document).on('click','#btn_skip',function(e) {

    e.preventDefault();
    e.stopPropagation();

    $('#packageform').load(PackageUrl.url_modifyPackageBody.replace("pId", $(this).data('id')));


});


$(document).on('click', '#btnCancelmodifyPackage', function (e) {

    e.preventDefault();

        Swal.fire({
            title: "Cancel Operation",
            text: "Cancel This Operation!",
            type: "Info"
        });

        setTimeout(function () {

            window.location.href = PackageUrl.url_index;

            // $('#spinn-loader').hide();
        }, 500);


});


$(document).on('click', '#btn_removelocPackage',function(e)
{
    e.preventDefault();
    var p_areaId = $(this).data('id');

            Swal.fire({
                title: "Are You Sure ?",
                text: "Confirm removing this location..",
                type: "question",
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Remove it!'
                //closeOnConfirm: true, closeOnCancel: true
            }).then((result) => {

                if (result.value) {

                    $.ajax({

                        type: 'Post',
                        url: PackageUrl.url_removePackageAreaCoverage,
                        ajaxasync: true,
                        data: { pAreaNo: p_areaId },
                        success: function (data) {

                            if (data.success) {

                                Swal.fire({
                                    title: "Success",
                                    text: "Package was succesfully remove!",
                                    type: "success"
                                       });

                            }

                            setTimeout(function () {

                                $('#packagelocationTable').load(data.url);

                                // $('#spinn-loader').hide();
                            }, 500);



                        }

                        ,
                        error: function (xhr, status, errorThrown) {

                            // alert(xhr.status);

                            if (xhr.status === 403) {


                                var response = $.parseJSON(xhr.responseText);

                                //  console.log(response);
                               // window.location = response.LogOnUrl;
                                Swal.fire({
                                    title: "Unable to process ",
                                    text: response.Error,
                                    type: "error"
                                });

                            }
                        }


                    });


                }

            });//end then

   

});



$(document).on('click','#addpackageLocation', function (e) {

    e.preventDefault();

  

    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    //delete existing modal
    $('.modal').replaceWith('');

    //get modal


    $.ajax({
        //'/Packages/AddPackageCoverage

        url: PackageUrl.url_addPackageAreaCoverage,
        type: 'Get',
        contentType: "application/json; charset=utf-8",
        data: { packageId: $(this).data('id') },
        cache: false,
        success: function (data) {

            var modal = $(data);
            $('body').append(modal);

            reparseform_packagelocation();

            $('#select2Area').select2({

                dropdownParent: $('#modal-addpackagecoverage'),

                placeholder: "Search Location",
                minimumInputLength: 2,  // minimumInputLength for sending ajax request to server
                width: 'resolve',
                ajax: {
                    //url: '/Packages/GetAreas/',
                    url: PackageUrl.url_searchPackageLocation,
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        var queryParameters = {
                            query: params.term
                        }

                        return queryParameters;
                    },
                    processResults: function (data) {
                        return { results: data.areaList }
                    }
                },

                formatResult: formatRepo,
                formatSelection: formatRepoSelection

            }).on("select2:select", function (e) {
                var selectedId = "";
                var selected_element = $(e.currentTarget);

                selectedId = selected_element.val();

                $('#hidden_AreaLocationId').val(selectedId);
            });

            modal.modal({
                backdrop: 'static',
                keyboard: false
            });

        }
    });

});


function reparseform_packagelocation() {
    $("#packagelocationform").removeData("validator");
    $("#packagelocationform").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse('#packagelocationform');
    //console.log('sadasd');

}


$(document).on('click', '#btn_modifyPackageloc', function (e) {

    e.preventDefault();

    var package_Id = $('#hiddenpackageId').val();

    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    //delete existing modal
    $('.modal').replaceWith('');

    //get modal


    $.ajax({
        //'/Packages/AddPackageCoverage

        url: PackageUrl.url_modifyPackageAreaCoverage,
        type: 'Get',
        contentType: "application/json; charset=utf-8",
        data: { packageId: package_Id, p_area_No: $(this).data('id') },
        cache: false,
        success: function (data) {

            var modal = $(data);
            $('body').append(modal);

            reparseform_packagelocation();

            $('#select2Area').select2({

                dropdownParent: $('#modal-addpackagecoverage'),

                placeholder: "Search Location",
                minimumInputLength: 2,  // minimumInputLength for sending ajax request to server
                width: 'resolve',
                ajax: {
                    //url: '/Packages/GetAreas/',
                    url: PackageUrl.url_searchPackageLocation,
                    dataType: 'json',
                    delay: 250,
                    data: function (params) {
                        var queryParameters = {
                            query: params.term
                        }

                        return queryParameters;
                    },
                    processResults: function (data) {
                        return { results: data.areaList }
                    }
                },

                formatResult: formatRepo,
                formatSelection: formatRepoSelection

            }).on("select2:select", function (e) {
                var selectedId = "";
                var selected_element = $(e.currentTarget);

                selectedId = selected_element.val();

                $('#hidden_AreaLocationId').val(selectedId);
            });

            modal.modal({
                backdrop: 'static',
                keyboard: false
            });

        }
    });

});


$(document).on('click', '#btnmodifylocation', function (e) {
    e.preventDefault();


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Update Location Details..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Save it!'
        //closeOnConfirm: true, closeOnCancel: true
    }).then((result) => {

        if (result.value) {

            var formUrl = $('#modifypackagelocationform').attr('action');

            var form = $('[id*=modifypackagelocationform]');

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
                                text: "It was succesfully Updated!",
                                type: "success"


                            });

                            location.href = PackageUrl.url_details.replace("pId", data.packageId);
                            $('[id*=modal-addpackagecoverage]').modal('hide');



                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Saving record Failed!', 'Please try again', 'error');
                    }
                });

            } else {
                $.each(form.validate().errorList, function (key, value) {
                    $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                    $errorSpan.html("<span style='color:yellow'>" + value.message + "</span>");
                    $errorSpan.show();
                });
            }

        }
    });

});

$(document).on('keypress', '#extAmount', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});


$(document).on('keypress', '#packageAmount', function (event) {

    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57) && (event.which !== 48)) {
        event.preventDefault();
    }
});

$(document).on('click', 'i#selectedcourse', function (e) {

    e.preventDefault();

    var _courseid = $(this).attr("data-id");

    var _packageId = $('#hidden_packageId').val();
    var thiscourse = $(this);

    if ($(this).hasClass("hasselected")) {
 
        //alert('remove seleted');

                Swal.fire({
                    title: "Are You Sure ?",
                    text: "Confirm removing this course in the list..",
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
                            url: PackageUrl.url_removeCoursefromPackagebody,
                            ajaxasync: true,
                            dataType: 'json',
                            data: { packageId: _packageId, courseId: _courseid },
                            //contentType: 'application/json; charset=utf-8',
                            cache: false,
                            success: function (data) {

                                if (data.success === false) {

                                                Swal.fire({
                                                    title: 'Unable to process!',
                                                    text: 'Unable to remove ,course is in use for active boking' ,
                                                    type: 'error'


                                                });


                                    } else {

                                    thiscourse.removeClass("fa-check-square-o");
                                    thiscourse.removeClass("hasselected");
                                    thiscourse.addClass("fa-square-o");
                                      
                                                setTimeout(function () {


                                                    $('#menulist').load(data.url);

                                                    // $('#spinn-loader').hide();
                                                }, 500);

                                                Swal.fire({
                                                    title: "Success",
                                                    text: "It was succesfully removed!",
                                                    type: "success"


                                                });


                                    }//end else

                                
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


    } else {

       // alert('check');
     

                Swal.fire({
                    title: "Are You Sure ?",
                    text: "Confirm Adding Course ..",
                    type: "question",
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, Save it!'

                }).then((result) => {

                    if (result.value) {

                        $.ajax({
                            type: "post",
                            url: PackageUrl.url_addcoursetoPackagebody,
                            ajaxasync: true,
                            data: { packageId: _packageId, courseId: _courseid },
                            cache: false,
                            success: function (data) {

                                if (data.is_recordexist) {

                                    Swal.fire({
                                        title: 'Unable to add course!',
                                        text: "Course is already in the list !",
                                        type: 'error'


                                    });



                                } else {

                                    thiscourse.removeClass("fa-square-o");
                                    thiscourse.addClass("fa-check-square-o hasselected");

                                            setTimeout(function () {
                                                $('#menulist').load(data.url);
                                            }, 500);


                                            Swal.fire({
                                                title: "Success",
                                                text: "It was succesfully added!",
                                                type: "success"


                                            });//end time out

                                }

                            }//succcess

                        });//end ajax


                    }//end if result

                });// end then

    }
});

$(document).on('click', 'span.packageview', function(e) {

    e.preventDefault();

    $('#spinn-loader').show();

    var action = $(this).attr('id');

    $('.packageview >i').removeClass('text-blue');

    $(this).children('i').addClass('text-blue');
    _g_viewType = action;

    $.ajax({
            url: PackageUrl.url_loadPackageCard,
            type: "GET",
            dataType: "html",
            data: { _viewType:action },
            cache: false,
            success: function (data) {
                //Fill div with results
                $("#packageDetails").html(data);
            },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);

                    setTimeout(function () {

                        $('#spinn-loader').hide();

                    }, 1000);
                }
    }).done(function () {

                setTimeout(function () {

                    $('#spinn-loader').hide();

                }, 1000);

                });

});

