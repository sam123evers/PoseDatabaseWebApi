using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PoseDatabaseWebApi.CustomActionResults
{
    public class ActionResultException : ActionResult
    {
        private readonly Exception _exception;
        public ActionResultException(Exception exception)
        {
            _exception = exception;
        }
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            //context gives you access to http request & response data
            var httpResponse = context.HttpContext.Response;

            httpResponse.Headers.Add("Exception-type", _exception.GetType().Name);

            await httpResponse.WriteAsync(_exception.Message);
        }
    }
}