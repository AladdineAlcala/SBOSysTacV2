using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace SBOSysTacV2.HtmlHelperClass
{
    public static class ModelStateExt
    {
        public static JsonResult Json_validation(this System.Web.ModelBinding.ModelStateDictionary state)
        {
            return new JsonResult
            {
                Data = new
                {
                    Tag = "ValidationError",
                    State = from e in state
                        where e.Value.Errors.Count > 0
                        select new
                        {
                            Name = e.Key,
                            Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                                .Concat(e.Value.Errors.Where(x => x.Exception != null).Select(x => x.Exception.Message))
                        }
                }
            };
        }

    }
}