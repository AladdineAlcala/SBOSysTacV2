using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBOSysTacV2.HtmlHelperClass
{
    public static class JsonExtractorHelper
    {
        public static string GetJson(string jsonstring)
        {
            string auditData = "";

            JObject data = (JObject)JsonConvert.DeserializeObject(jsonstring);

            string action = (string)data["Action"];

            var table = data["Table"];
            var primaryKey = data["PrimaryKey"].First;


            if (action == "Update")
            {
                var changes = data["Changes"].Children();



                auditData += table + " table updated ;" + " Primary key value: " + primaryKey + "</br>";


                foreach (var change in changes)
                {


                    //var originalValue =change.Children().Contains("OriginalValue") ? change["NewValue"].Value<string>():change["OriginalValue"].Value<string>();

                    try
                    {
                        var columnName = change["ColumnName"].Value<string>();

                        var newValue = change["NewValue"].Value<string>();

                        JToken token = change["OriginalValue"];

                        string originalValue = token != null? change["OriginalValue"].Value<string>() : string.Empty;

                        //if (token != null)
                        //{
                        //    originalValue = change["OriginalValue"].Value<string>();
                        //}


                        if (originalValue != newValue)
                        {
                            var changecolumn = "( " + columnName + " : " + " Original Value :" + originalValue +
                                               "; New Value : " + newValue + ")" + "</br>";

                            auditData += changecolumn;
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }


                    

                }

            }
            else if (action == "Insert")
            {

                var columnValues = data["ColumnValues"];

                var newdata = "New record inserted to " + table + " Primary Key Value: " + primaryKey + "</br>" + " data " + columnValues;

                auditData += newdata;
            }
            else if (action == "Delete")
            {

                var columnValues = data["ColumnValues"];

                var newdata = "Record deleted " + table + " Primary Key Value: " + primaryKey + "</br>" + " data " + columnValues;

                auditData += newdata;
            }

            return auditData;
        }



        public static IEnumerable<BookingHistoryLogViewModel> BookingLogsHistory(IList<AuditLog> auditlog, int transId)
        {
            List<BookingHistoryLogViewModel> bookingHistoryLogs = new List<BookingHistoryLogViewModel>();



            foreach (var audit in auditlog)
            {
                string auditData = "";

                JObject data = (JObject)JsonConvert.DeserializeObject(audit.AuditData);

                var table = data["Table"];


                int key = data["PrimaryKey"]["trn_Id"].Value<int>();
                DateTime datelog = (DateTime)audit.EventDateUTC;
                string operation = data["Action"].Value<string>();
                int x = 1;


                var bookhistory = new BookingHistoryLogViewModel();

                bookhistory.TransId = key;
                bookhistory.Log_operation = operation;
                bookhistory.Logdate = datelog;

                if (operation == "Update")
                {
                    var changes = data["Changes"].Children();

                    foreach (var c in changes)
                    {

                        var columnName = c["ColumnName"].Value<string>();
                        //var originalValue =change.Children().Contains("OriginalValue") ? change["NewValue"].Value<string>():change["OriginalValue"].Value<string>();
                        var newValue = c["NewValue"].Value<string>();


                        JToken token = c["OriginalValue"];

                        string originalValue = string.Empty;

                        if (token != null)
                        {
                            originalValue = c["OriginalValue"].Value<string>();
                        }


                        if (originalValue != newValue)
                        {
                            auditData = "( " + columnName + " : " + " Original Value :" + originalValue +
                                               "; New Value : " + newValue + ")";


                            bookhistory.BookingEventList.Add(new BookingEvents
                            {
                                eventNo = x,
                                transId = key,
                                eventDetails = auditData
                            });

                        }


                        x += 1;
                    }
                }
                else if (operation == "Insert")
                {

                    var columnValues = data["ColumnValues"];

                    auditData = "New record inserted to " + table + " Primary Key Value: " + key + " data " + columnValues;


                    bookhistory.BookingEventList.Add(new BookingEvents
                    {
                        eventNo = x,
                        transId = key,
                        eventDetails = auditData
                    });

                }
                else if (operation == "Delete")
                {

                    var columnValues = data["ColumnValues"];

                    auditData = "Record deleted " + table + " Primary Key Value: " + key + " data " + columnValues;

                    bookhistory.BookingEventList.Add(new BookingEvents
                    {
                        eventNo = x,
                        transId = key,
                        eventDetails = auditData
                    });
                }



                bookingHistoryLogs.Add(bookhistory);
            }



            return bookingHistoryLogs.Where(x => x.TransId == transId).OrderByDescending(t => t.Logdate);
        }
    }
}