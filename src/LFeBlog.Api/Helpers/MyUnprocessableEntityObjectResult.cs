using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LFeBlog.Web.Core.Helpers
{
    public class MyUnprocessableEntityObjectResult:UnprocessableEntityObjectResult
    {
        public MyUnprocessableEntityObjectResult(ModelStateDictionary modelState) : base(modelState)
        {

            if (modelState==null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = 42;

        }
    }
}