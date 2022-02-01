using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Elmah;

namespace SBOSysTacV2.HtmlHelperClass
{
    public class UserPermissionAuthorized:AuthorizeAttribute
    {
        private readonly UserPermessionLevelEnum[] allowedPermissionLevel;

        public UserPermissionAuthorized(params UserPermessionLevelEnum[] permessionLevel)
        {
            this.allowedPermissionLevel = permessionLevel;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool _isauthorize = false;

            IList<UserPermessionLevelEnum> theApprovedPermissionLevelList = GetLoggedUserPermissionLevel();

            foreach (UserPermessionLevelEnum permessionLevel in allowedPermissionLevel)
            {
                if (theApprovedPermissionLevelList.Any(a => a == permessionLevel) == true)
                {
                    _isauthorize = true;
                }
            }

            return _isauthorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var urlHelper=new UrlHelper(filterContext.RequestContext);
                   // filterContext.HttpContext.Response.StatusCode =(int)HttpStatusCode.Forbidden;
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new JsonResult()
                    {
                        Data = new
                        {
                            Error="You Are Not Authorized to Access this Operation",
                            LogOnUrl=urlHelper.Action("UnauthorizedAccess","Home")
                            
                        },JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "UnauthorizedAccess" }));
                }

            }
            else // not authnticated  redirect to login
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private IList<UserPermessionLevelEnum> GetLoggedUserPermissionLevel()
        {
            var _LoggedUserRoles = new List<UserPermessionLevelEnum>();

            //theApprovedRoles.Add(UserPermessionLevelEnum.superadmin);
            //theApprovedRoles.Add(UserPermessionLevelEnum.admin);
       

          //  ClaimsIdentity userClaimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
           // var claims = userClaimsIdentity.Claims;
            //var roleClaimType = userClaimsIdentity.RoleClaimType;
          //  var roles = claims.Where(c => c.Type == roleClaimType).ToList();

            var roles = ((ClaimsIdentity) HttpContext.Current.User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();


            _LoggedUserRoles = roles.Select(x => Enum.Parse(typeof(UserPermessionLevelEnum), x))
                .Cast<UserPermessionLevelEnum>().ToList();

            return _LoggedUserRoles;
        }

    }

    public enum UserPermessionLevelEnum
    {
        superadmin,
        admin,
        user,
        cashier
    }

}