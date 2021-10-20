$(document).ready(function() {

    //Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD-MMM-YYYY", true).isValid();
    }
    var dateNow = new Date();
    //init datetimepcker
    $('#dateFrom').datetimepicker(
        {

            //format: "DD-MMM-YYYY",
            //defaultDate: dateNow

            //maxDate: moment(),
            allowInputToggle: true,
            enabledHours: false,
            locale: moment().local('en'),
            format: 'MM-DD-YYYY',
            defaultDate: dateNow

        });

    //init datetimepcker
    $('#dateTo').datetimepicker(
        {

            allowInputToggle: true,
            enabledHours: false,
            locale: moment().local('en'),
            format: 'MM-DD-YYYY',
            defaultDate: dateNow

        });


    $.fn.dataTable.moment = function (format, locale) {

        var types = $.fn.dataTable.ext.type;

        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ? 'moment-' + format : null;
        });

        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    }

    $('a.printopt').on('click', function (e) {

        e.preventDefault();
        e.stopPropagation();


        var filterdatefrom =$('#dateFrom').val();
        var filterdateTo = $('#dateTo').val();
       // filterdatefrom = moment(filterdatefrom, "YYYY-MM-DD");

        //console.log(filterdatefrom);

        var url = $(this).attr('href');

        window.location.href = url + '?datefrom=' + filterdatefrom + '&dateto=' + filterdateTo;

        // window.location.href = url;
    });


    
    $('a.printcateringReport').on('click', function (e) {

        e.preventDefault();
        e.stopPropagation();


        var filterdatefrom =$('#dateFrom').val();
        var filterdateTo = $('#dateTo').val();
        var unsetchk=false;

        var unset = document.getElementById("chk_unsettled");

         if (unset.checked) {
             unsetchk = true;
         }
         

    

        var url = $(this).attr('href');

        window.location.href = url + '?datefrom=' + filterdatefrom + '&dateto=' + filterdateTo + '&w_unsettle=' + unsetchk;

        // window.location.href = url;
    });


});