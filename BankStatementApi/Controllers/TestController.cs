using Microsoft.AspNetCore.Mvc;

namespace BankStatementApi.Controllers
{
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "test";
        }
    }
}