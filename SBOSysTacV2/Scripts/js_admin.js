$(document).ready(function() {

    //$('#btn-searchAll').on('click',function(e) {

    //        e.preventDefault();

    //        var form = $(this).closest('form');
    //        var formUrl = $('#globalsearchkey').attr('action');

    //    //Swal.fire('Sorry', 'Module is still on working process', 'info');

    //        $.ajax({
    //            type: 'POST',
    //            url: formUrl,
    //            data: form.serialize(),
    //            datatype: 'json',
    //            cache: false,
    //            success: function (data) {
    //            if (data.success) {

    //                //alert(data.success);

                  


    //            }
    //            else {

    //                Swal.fire('Failed', data.message, 'error');
    //            }
    //        },
    //        error: function (xhr, ajaxOptions, thrownError) {
    //            Swal.fire('Error adding record!', 'Please try again', 'error');
    //        }
    //    });



    //});

    $('#btnProfile').on('click',function(e) {
        e.preventDefault();

        var loguserid = $(this).attr('data-userid');

        //console.log(selecteduserid);

        window.location.href = UserlProfile.url_userProfile.replace("userid", loguserid);


    });

    //$('#updateSecurity').on('click',function(e) {
    //    e.preventDefault();

    //    alert('asdasd');

    //});


});

$(document).on('click','#updateSecurity',function(e) {
    e.preventDefault();

    alert('asdasd');

});