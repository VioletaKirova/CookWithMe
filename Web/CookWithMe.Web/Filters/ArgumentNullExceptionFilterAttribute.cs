namespace CookWithMe.Web.Filters
{
    using System;
    using System.Collections.Generic;

    using CookWithMe.Common;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public class ArgumentNullExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        private readonly ITempDataDictionaryFactory tempDataDictionaryFactory;

        public ArgumentNullExceptionFilterAttribute(ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            this.tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentNullException)
            {
                this.tempDataDictionaryFactory
                    .GetTempData(context.HttpContext)
                    .Add("ErrorParams", new Dictionary<string, string>
                    {
                        ["RequestId"] = context.HttpContext.TraceIdentifier,
                        ["RequestPath"] = context.HttpContext.Request.Path,
                    });

                context.Result = new RedirectResult($"/Home/Error?statusCode={StatusCodes.NotFound}");
            }
        }
    }
}
