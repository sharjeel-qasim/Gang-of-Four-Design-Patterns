using GoFDesignPatterns.Creational;
using Microsoft.AspNetCore.Mvc;

namespace GoFDesignPatterns.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreationalPatternsController : ControllerBase
    {

        /// <summary>
        /// The Singleton design pattern is used when you want to ensure that a class has only one instance and 
        /// provides a global point of access to that instance. Common use cases are configuration management, logging etc
        /// </summary>
        /// <returns></returns>
        [HttpGet("singleton")]
        public IActionResult Singleton()
        {
            // Get the singleton instance.
            var instance1 = SingletonDesignPattern.GetInstance();
            var instance2 = SingletonDesignPattern.GetInstance();

            bool isBothInstanceEqual = ReferenceEquals(instance1, instance2);

            return new JsonResult(new { isBothInstanceEqual });
        }
    }
}