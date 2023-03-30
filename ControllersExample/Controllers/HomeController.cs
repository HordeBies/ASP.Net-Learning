using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;
using System.Net.Mime;
using Microsoft.Net.Http.Headers;

namespace ControllersExample.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        [Route("/home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return Content(
            //    content: "Hello HomeController-Index" , 
            //    contentType: "text/plain");
            return Content(
                content: "Hello HomeController-Index",
                contentType: "text/plain");
        }
        [Route("about")]
        public ContentResult About()
        {
            return Content(
                content: "<h1>Hello</h1> <h2>HomeController-About</h2>",
                contentType: "text/html");
        }
        [Route("/contact/{mobile:regex(^\\d{{3}}$)}")]
        public ContentResult Contact()
        {
            return Content(
                content:"Hello HomeController-Contact",
                contentType:"text/plain");
        }
        [Route("/person")]
        public JsonResult Person()
        {
            return Json(new Person() {Id = Guid.NewGuid(), FullName = "Bies", Age = 13 });
        }
        [Route("/virtual-file")]
        public VirtualFileResult VirtualSampleFileDownload()
        {
            return File("/sample.pdf", "application/pdf");
        }
        [Route("/physical-file")]
        public PhysicalFileResult PhysicalSampleFileDownload()
        {
            return PhysicalFile(@"D:\Okul\Lecture9_Linux.pdf", "application/pdf");
        }
        [Route("/content-file")]
        public FileContentResult SampleFileContentDownload()
        {
            var buffer = System.IO.File.ReadAllBytes(@"D:\Okul\Lecture9_Linux.pdf");
            return File(buffer, "application/pdf");
        }
        [Route("/action")] //We will use IActionResult as return type
        public IActionResult Test()
        {
            return Content("content", "text/plain");
        }
    }
}
