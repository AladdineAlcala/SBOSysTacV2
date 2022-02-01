using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using MvcBreadCrumbs;
using SBOSysTacV2.Models;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Web.Configuration;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
  //  [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
    [Authorize]
    public class CustomersController : Controller
    {
        private PegasusEntities _dbcontext;

        private CustomerViewModel cusviewmodel=new CustomerViewModel();
        // GET: Customers
        //[BreadCrumb(Clear = true,Label = "Customers")]

        public CustomersController()
        {
            _dbcontext = new PegasusEntities();
        }


        public ActionResult Index()
        {
            ViewBag.FormTitle = "Customers Index";

            return View();
        }

     
        public ActionResult LoadCustomers()
        {
            _dbcontext.Configuration.ProxyCreationEnabled = false;

            var listcustCustomers = _dbcontext.Customers.ToList();

            return Json(new {data = listcustCustomers}, JsonRequestBehavior.AllowGet);
        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        [HttpGet]
        public ActionResult CreateCustomer()
        {
            ViewBag.FormTitle = "Create New Customer";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer(CustomerDetailsViewModel newcusViewmodel)
        {
            newcusViewmodel.datereg = Convert.ToDateTime(DateTime.Today);

            bool isSuccess = false;

            if (ModelState.IsValid)
            {

                
                string _lastname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newcusViewmodel.lastname);
                string _firstname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newcusViewmodel.firstname);

                string _middle = string.Empty;

                if (newcusViewmodel.middle != null)
                {
                    _middle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newcusViewmodel.middle);
                }
                else
                {
                    _middle = newcusViewmodel.middle;
                }

                string _address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newcusViewmodel.address);

                //verify if customer has already record

                var isRecordAlreadyExist = _dbcontext.Customers.Any(x => x.lastname == _lastname && x.firstname == _firstname);

                if (isRecordAlreadyExist)
                {

                    return Json(
                        new {success=isSuccess, message = "Unable to save record ;\n Possible for duplicate entry."},JsonRequestBehavior.AllowGet);

                }
                else
                {
                    try
                    {
                        var newcustomer = new Customer()
                        {
                            c_Id = Convert.ToInt32(newcusViewmodel.c_Id),
                            lastname = _lastname,
                            firstname = _firstname,
                            middle = _middle,
                            address = _address,
                            contact1 = newcusViewmodel.contact1,
                            contact2 = newcusViewmodel.contact2,
                            datereg = newcusViewmodel.datereg,
                            company = newcusViewmodel.company

                        };

                        _dbcontext.Customers.Add(newcustomer);
                        _dbcontext.SaveChanges();

                        isSuccess = true;

                        return Json(new {success=isSuccess }, JsonRequestBehavior.AllowGet);

                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine(
                                "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }


                        throw;
                    }
                }


            }


            ViewBag.FormTitle = "Create New Customer";
            return View(newcusViewmodel);
        }

       
        public JsonResult GetCustomers(string query)
        {
            List<CustomerViewModel> customerList;
            try
            {
             customerList = cusviewmodel.getCustomer().ToList();

                customerList = customerList.Where(c => c.fullname.Contains(query)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        //Remove Customer
        [HttpPost]
        public ActionResult RemoveCustomer(int customerId)
        {

            Customer cusdelete=new Customer();
            TransRecievablesViewModel tr=new TransRecievablesViewModel();
            bool success = false;
            try
            {
                //get customer in db
             

                // check if has balance

                //var customertrans = tr.GetAllRecievables().Where(x => x.cusId == cusdelete.c_Id)
                //    .Any(b => b.balance > 0);

                var hasBookings = _dbcontext.Bookings.Any(x => x.c_Id == customerId);

                if (!hasBookings)
                {
                    cusdelete = _dbcontext.Customers.Find(customerId);
                    if (cusdelete != null) _dbcontext.Customers.Remove(cusdelete);
                    _dbcontext.SaveChanges();

                    success = true;
                }
           

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new {success=success}, JsonRequestBehavior.AllowGet);
        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult ModifyCustomer(int customerId)
        {

           Customer customer=new Customer();

            customer = _dbcontext.Customers.Find(customerId);

            var cusdetails = new CustomerDetailsViewModel()
            {
                c_Id = Convert.ToInt32(customer.c_Id),
                lastname = customer.lastname,
                firstname = customer.firstname,
                middle = customer.middle,
                contact1 = customer.contact1,
                contact2 = customer.contact2,
                datereg = customer.datereg,
                company = customer.company,
                address = customer.address


            };
           

            return View(cusdetails);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyCustomer(CustomerDetailsViewModel modifiedcustomer)
        {

            //newcusViewmodel.datereg = Convert.ToDateTime(DateTime.Today);
            bool isSuccess = false;

            if (ModelState.IsValid)
            {

                string _lastname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modifiedcustomer.lastname);
                string _firstname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modifiedcustomer.firstname);
                string _middle = string.Empty;
                if (modifiedcustomer.middle !=null)
                {
                    _middle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modifiedcustomer.middle);
                }
                else
                {
                    _middle = modifiedcustomer.middle;
                }

                DateTime modifdate = DateTime.Now;

                if (modifiedcustomer.datereg != null)
                {
                    modifdate = (DateTime) modifiedcustomer.datereg;
                }

                string _address = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(modifiedcustomer.address);


                try
                {
                    //var customer = new Customer()
                    //{
                    //    c_Id = Convert.ToInt32(modifiedcustomer.c_Id),
                    //    lastname = _lastname,
                    //    firstname = _firstname,
                    //    middle = _middle,
                    //    contact1 = modifiedcustomer.contact1,
                    //    contact2 = modifiedcustomer.contact2,
                    //    datereg = modifdate,
                    //    company = modifiedcustomer.company,
                    //    address = _address

                    //};

                   var customer= _dbcontext.Customers.FirstOrDefault(c => c.c_Id == modifiedcustomer.c_Id);

                    if (customer!=null)
                    {

                        customer.lastname = _lastname;
                        customer.firstname = _firstname;
                        customer.middle = _middle;
                        customer.contact1 = modifiedcustomer.contact1;
                        customer.contact2 = modifiedcustomer.contact2;
                        customer.datereg = modifdate;
                        customer.company = modifiedcustomer.company;
                        customer.address = _address;


                    }



                    //_dbcontext.Customers.Attach(customer);
                    //_dbcontext.Entry(customer).State = EntityState.Modified;


                    _dbcontext.SaveChanges();

                    isSuccess = true;

                    //return RedirectToAction("Index");
                    return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);


                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }


                    throw;
                }
            }


            ViewBag.FormTitle = "Modify Customer";
            return View(modifiedcustomer);

        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }
    }
}