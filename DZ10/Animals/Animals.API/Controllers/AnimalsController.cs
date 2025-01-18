using Animals.API.Filters;
using Animals.API.Interfaces;
using Animals.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Controllers;

[ApiController]
[Route("api/animals")]
public class AnimalsController : ControllerBase
{
    [HttpGet("test1")]
    [LogFilter]
    public ActionResult TestEndpoint()
    {
        return Ok("test");
    }

    [HttpGet("test2")]
    public ActionResult TestEndpoint2()
    {
        return Ok("test");
    }

    [HttpGet("test3")]
    [HttpLogFilter]
    public ActionResult TestEndpoint3()
    {
        return Ok("Test response");
    }

    [HttpGet("test4")]
    [LogFilter]
    public ActionResult TestEndpoint4()
    {
        throw new ArgumentException("text argument exception");
        return Ok("test");
    }

    [HttpGet("test5")]
    public ActionResult TestEndpoint5()
    {
        throw new UnauthorizedAccessException("test unauth");
        return Ok("test");
    }

    //DZ-10
    [HttpGet("test6")]
    [ExecutionTimeFilter]
    public ActionResult TestEndpoint6()
    {
        Thread.Sleep(1000); // Симуляція затримки
        return Ok("Test response");
    }

}