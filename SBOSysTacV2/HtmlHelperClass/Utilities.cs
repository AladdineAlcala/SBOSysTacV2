using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Hosting;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;

namespace SBOSysTacV2.HtmlHelperClass
{
    public static class Utilities
    {

        public static string ActiveForm;
        public static PackageBodyViewModel PackageBodyModel=new PackageBodyViewModel();

        public static string MenusCode_Generator()
        {
            int id = 0;

            var dbEntities = new PegasusEntities();

            try
            {

                var seriesParam = new SqlParameter
                {
                    ParameterName = "series",
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output
                };
                var seriesId = dbEntities.Database.SqlQuery<int>("exec Generate_MenuCode @series out", seriesParam).FirstOrDefault();

                //var seriesId = dbEntities.Database.SqlQuery<int>("exec Generate_PmtCode @series out", seriesParam).FirstOrDefault();

                id = Convert.ToInt32(seriesId);

            }
            catch (NullReferenceException)
            {

                id = id + 1;
            }

            catch (FormatException)
            {
                id = id + 1;
            }

            return (string.Format("{0:0000}",id));

        }

        public static string EventSlip_Generator()
        {
            int id = 0;

            var dbEntities = new PegasusEntities();

            try
            {

                var seriesParam = new SqlParameter
                {
                    ParameterName = "series",
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output
                };
                var seriesId = dbEntities.Database.SqlQuery<int>("exec Generate_EventSlip @series out", seriesParam).FirstOrDefault();

                id = Convert.ToInt32(seriesId);

            }
            catch (NullReferenceException)
            {

                id = id + 1;
            }

            catch (FormatException)
            {
                id = id + 1;
            }

            return (string.Format("{0:0000}", id));

        }


        public static string Generate_PaymentId()
        {
            var dbcontext = new PegasusEntities();

            int id = 0;
            try
            {
                var seriesParameter = new SqlParameter()
                {
                    ParameterName = "series",
                    DbType = DbType.Int32,
                    Direction = ParameterDirection.Output


                };

                var seriesId = dbcontext.Database.SqlQuery<int>("exec Generate_PmtCode @series out", seriesParameter).FirstOrDefault();

                id = Convert.ToInt32(seriesId);


            }
            catch (NullReferenceException)
            {

                id = id + 1;
            }

            catch (FormatException)
            {
                id = id + 1;
            }

            return String.Format("{0:0000000}",id);
        }

      


            public static string getfullname(string last, string first, string middle)
            {
                string conCat = " ";

                if (!string.IsNullOrEmpty(last))
                {
                    conCat += last + " ,";
                }
                if (!string.IsNullOrEmpty(first))
                {
                    conCat += first + " ";
                }

                if (!string.IsNullOrEmpty(middle))
                {
                    conCat += middle + " ";
                }

                return conCat.Trim();
            }

            public static string getfullname_nonreverse(string last, string first, string middle)
            {
                string conCat = " ";

           
                if (!string.IsNullOrEmpty(first))
                {
                    conCat += first.Trim() + " ";
                }
                if (!string.IsNullOrEmpty(middle))
                {
                    conCat += middle.Trim() + ". ";
                }

                if (!string.IsNullOrEmpty(last))
                {
                    conCat += last.Trim() + " ";
                }
            

                return conCat.Trim();
            }

        public static void AddCssClass(this WebControl control, string cssclass)
        {
            List<string> classes;

            if (!string.IsNullOrWhiteSpace(control.CssClass))
            {
                classes = control.CssClass.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!classes.Contains(cssclass))
                {
                    classes.Add(cssclass);
                }
            }
            else
            {
                classes=new List<string> {cssclass};
            }
            control.CssClass = string.Join(" ", classes.ToArray());
        }

        public static int GetTotalNoofCoursePerTransaction(int trId,int courseId)
        {
            int count = 0;


            return count;
        }

        public static string DBGateway()
        {

            ConnectionStringSettings dbconnString = ConfigurationManager.ConnectionStrings["PegasusEntities2"];

            return dbconnString.ConnectionString;

        }

        public static string ReportPath(string repName)
        {
          
            return HostingEnvironment.MapPath(string.Format("~/Reports/{0}.rpt", repName));

        }

        public static string capfirstletter(string wordstring)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(wordstring.ToLower());
        }
    }
}   