using ASP.NETCore.Models;
using Microsoft.AspNetCore.Mvc;

public class LoginFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = new LoginViewModel();
        return View(model);
    }
}
