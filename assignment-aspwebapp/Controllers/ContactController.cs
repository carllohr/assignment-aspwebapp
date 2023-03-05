using assignment_aspwebapp.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace assignment_aspwebapp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index(string ReturnUrl = null!)
        {
            var form = new ContactForm { ReturnUrl = ReturnUrl ?? Url.Content("~/")};
            return View(form);
        }
        [HttpPost]
        public IActionResult Index(ContactForm form)
        {
            return View(form);
        }
    }
}
