using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Data;
using net_il_mio_fotoalbum.Models;
using System.Security.Claims;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            if (loggedProfile == null)
                return RedirectToAction("Create", "Profile");

            List<Message> messages = AdminManager.GetProfilesMessages(loggedProfile.ProfileId);

            return View("/Views/Admin/Messages/Index.cshtml", messages);
        }

        [Route("/Admin/Message/{id}")]
        public IActionResult Show(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);
            if (loggedProfile == null)
                return RedirectToAction("Create", "Profile");

            Message message = AdminManager.GetMessageById(id, true);

            if (message.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                return View("/Views/Admin/Messages/Show.cshtml", message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Profile loggedProfile = AdminManager.GetProfileByUserId(userId);

            if (loggedProfile == null)
                return RedirectToAction("Create", "Profile");

            Message message = AdminManager.GetMessageById(id);

            if (message.ProfileId == loggedProfile.ProfileId || User.IsInRole("Admin"))
            {
                AdminManager.DeleteMessage(id);
            }

            return RedirectToAction("Index");
        }
    }
}
