$(document).ready(function() {


    //Date for the calendar events (dummy data)
    var to_day = new Date();

    loadCalendarEvents(calendarEventsUrl.calUrl_AllEvents);


    function loadCalendarEvents(url) {
        $('#spinn-loader').show();

        $('#event_calendar').fullCalendar('destroy');

        $('#event_calendar').fullCalendar({
            contentHeight: 400,
            aspectRatio: 1.8,
            defaultDate: to_day,
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay',
                height: 650
            },
            buttonText: {
                today: 'today',
                month: 'month',
                week: 'week',
                day: 'day'
            },
            defaultView: 'month',
            editable: 'false',
            timezone: 'local',
            eventRender: function(eventObj, $el) {

                $el.popover({
                    title: eventObj.title,
                    content: eventObj.description,
                    trigger: 'hover',
                    placement: 'top',
                    container: 'body'
                });
            },

            events: function(start, end, timezone, callback) {

                $.ajax({
                    url: url,
                    type: 'GET',
                    dataType: 'JSON',

                    success: function(result) {

                        var events = [];

                        var eventdate = "";


                        result.forEach(function (evt) {

                            var txtcolor = '';
                            var backcolor = '';

                            var d = moment.utc(evt.StartDateTime).format('MM/DD/YYYY');
                            eventdate = new Date(d);



                            if (evt.booktype === 'ins') {
                                backcolor = '#7a6bf4';
                                txtcolor = '#F7F9F9';
                            } else if (evt.booktype === 'out') {
                                backcolor = '#58b460';
                                txtcolor = '#F7F9F9';
                            } else {
                                
                            }

                            if (to_day.getTime() < eventdate.getTime()) {
                                console.log('equals' + to_day.getTime());

                                txtcolor = '#FFFFFF';

                            } else if (eventdate.getTime() === to_day.getTime()) {
                                //  console.log('equals');
                                backcolor = '#0000FF';
                                txtcolor = '#FFFFFF';
                            } else {
                                //   console.log('date schedule finish');
                                backcolor = '#FF0000';
                                txtcolor = '#F7F9F9';
                            }


                            events.push({
                                id: evt.EventId,
                                title: evt.EventDescription,
                                description: evt.EventName,
                                start: moment.utc(evt.StartDateTime),
                                backgroundColor: backcolor,
                                textColor: txtcolor


                            });
                        });

                        //console.log(events);
                        callback(events);

                       

                    },
                    error: function(jqXhr, textStatus, errorThrown) {
                        Swal.fire('Error when fetching data:', +textStatus + "- " + errorThrown, 'error');
                    }


                }).done(function () {

                    $('#spinn-loader').hide();
                });

                //end call ajax events
            },
            dayClick: function(date, jsevent, view) {

                var datefiltered = moment.utc(date).format('MM/DD/YYYY');

                $.ajax({
                    type: 'Get',
                    url: calendarEventsUrl.calUrl_dayEventUrl,
                    contentType: 'application/html;charset=utf8',
                    data: { eventdate: datefiltered },
                    datatype: 'html',
                    cache: false,
                    success: function(result) {

                        // console.log(result);

                        $('#eventsList').html(result);
                    },
                    error: function(xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error on retrieving record!', 'Please try again', 'error');
                    }


                });


            } //----- end dayclick event
        });

    }


    $("input:radio").on('change',
        function() {

            var selectedVal = $(this).val();
            var url = "";

            if (selectedVal === "book") {

                //console.log('book');

                url = calendarEventsUrl.calUrl_Book_Events;


                loadCalendarEvents(url);

            } else if (selectedVal === "reserve") {
                //console.log('reserve');
                url = calendarEventsUrl.calUrl_Reservation_Events;
                loadCalendarEvents(url);


            } else {
                //console.log('all');

                url = calendarEventsUrl.calUrl_AllEvents;
                loadCalendarEvents(url);


            }


            //  alert(selectedVal);


        });

    //var currerntday = new Date();
    //console.log(currerntday);

    //load todays events

    $.ajax({
        type: 'Get',
        url: calendarEventsUrl.calUrl_GetEventsToday,
        contentType: 'application/html;charset=utf8',
        //data: { eventdate: to_day },
        datatype: 'html',
        cache: false,
        success: function(result) {

            //console.log(result);

            $('#eventsList').html(result);
        },
        error: function(xhr, ajaxOptions, thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    });



         
});

$(document).on('click','td#bookingevent', function (e) {

    window.location.href = calendarEventsUrl.calUrl_GetEventBookingDetails.replace("book_id", $(this).attr('data-id'));

});