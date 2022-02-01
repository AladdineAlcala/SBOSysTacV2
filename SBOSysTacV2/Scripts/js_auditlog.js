var $table_logs;

$(document).ready(function () {

    if ($.fn.DataTable.isDataTable('#table_logs')) {

        $('#table_logs').dataTable().fnDestroy();
        $('#table_logs').dataTable().empty();

    }


    $.fn.dataTable.moment = function (format, locale) {

        var types = $.fn.dataTable.ext.type;

        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ? 'moment-' + format : null;
        });

        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    }


    $table_logs = $('#table_logs').DataTable({
        
        "serverSide": false,
        "processing": true,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },
        //"bInfo": false,
        ////"dom": '<"top"B>rt<"bottom"lp><"clear">',

        //"dom": "<'#menutop.row'<'col-sm-6'B>>" +
        //    "<'row'<'col-sm-12'tr>>" +
        //    "<'#menubottom.row'<'col-sm-5'l><'col-sm-7'p>>",

        "pagingType": "full_numbers",

        "ajax":
        {
            "url": auditLog.url_auditlogList,
            "type": "GET",
            "datatype": "json"
        },
    
        "columnDefs":
        [
           
            {
                'autowidth': true, 'targets': 0,
                "data": "dateLog",
                "type": "date",
                "render": function (d) {
                    //return moment(d).format('MM/DD/YYYY');
                  
                  return moment(d).format("MMM-DD-YYYY hh:mm: A");
                    
                }

            }
            ,
            {
                'autowidth': true, 'targets': 1, "data": "username"

            }
            ,
            {
                'autowidth': true, 'targets': 2, "data": "audit_operation"

            }
            ,
            {
                'autowidth': true, 'targets': 3, "data": "tablename"

            }
            ,
            {
                'autowidth': true, 'targets': 4, "data": "audit_Data"

            }
        ]
        ,
        "order":[[0,'desc']]
        ,

        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-audit-id', data.logId);
        }

    });

});