$(document).ready(function () {



    $('#updateSecurity').on('click',function(e) {
        e.preventDefault();

        var form = $(this).closest('form');

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Updating User Security..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Update it!'

        }).then((result) => {

            if (result.value) {

                var formUrl = $('#updateFormUser').attr('action');
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

                                //window.location.href = customerUrl.url_customerIndex;
                                $('#usersecurity').load(data.url);

                            }
                            else {

                                Swal.fire('Failed', data.errmsg, 'error');
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error updating user security!', 'Please try again', 'error');
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


        e.stopPropagation();

    });


});
