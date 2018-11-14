using System;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class BaseController : Controller
    {

        public T Execute<T>(Func<T> delegateFunction)
        {
            return delegateFunction();
        }

        public IActionResult ExecuteI<T>(Func<T> delegateFunction)
        {
            try
            {
                return Ok(delegateFunction.Invoke());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public void Execute(Action delegateFunction)
        {
            delegateFunction();
        }
    }
}
