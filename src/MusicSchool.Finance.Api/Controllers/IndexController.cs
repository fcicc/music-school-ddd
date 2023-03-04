using Microsoft.AspNetCore.Mvc;

namespace MusicSchool.Finance.Api.Controllers;

[Controller]
[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class IndexController : Controller
{
    [HttpGet("")]
    public ActionResult GetIndex()
    {
        return Redirect("/swagger");
    }
}
