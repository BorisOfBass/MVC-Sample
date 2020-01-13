using ScreeningTesterWebSite.Models;
using System.Web.Mvc;

namespace ScreeningTesterWebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            InputFrontPage inFrontPage = new InputFrontPage();
            return View(inFrontPage);
        }

        public ActionResult HomePage(InputFrontPage passedinFrontPage)
        {
            InputFrontPage inputFrontPage = new InputFrontPage();
            inputFrontPage.TransferCommonProperties(passedinFrontPage);
            inputFrontPage.SetDefaults();
            return View(inputFrontPage);
        }

        public ActionResult BackToHomePage(string coID, string loID, string envCode, string languageid, string block, string section)
        {
            return View(new InputFrontPage()
            {
                CoID = coID,
                LoID = loID,
                EnvironCode = envCode,
                LanguageID = languageid,
                UserName = StringUtil.Decrypt(block),
                UserPW = StringUtil.Decrypt(section)
            });
        }

        public ActionResult NavigateFromHomePage(InputFrontPage inputFrontPage, string unitmode, string fullmode, string ivrmode)
        {
            if (!string.IsNullOrEmpty(ivrmode))  // If ActionLink named "ivrmode" is clicked
            {
                return RedirectToAction(
                    actionName: "CompanyIDAndLOIDLookupReturn",
                    controllerName: "IVRFullScreening",
                    routeValues: new
                    {
                        coID = inputFrontPage.CoID,
                        loID = inputFrontPage.LoID,
                        envCode = inputFrontPage.EnvironCode,
                        languageid = inputFrontPage.LanguageID,
                        block = inputFrontPage.EncryptedUserName,
                        section = inputFrontPage.EncryptedUserPW
                    });
            }
            if (!string.IsNullOrEmpty(unitmode))  // If ActionLink named "unitmode" is clicked
            {
                return RedirectToAction(
                    actionName: "PWSUnitTestPageReturn",
                    controllerName: "PWSUnitTest",
                    routeValues: new
                    {
                        coID = inputFrontPage.CoID,
                        loID = inputFrontPage.LoID,
                        envCode = inputFrontPage.EnvironCode,
                        languageid = inputFrontPage.LanguageID,
                        block = inputFrontPage.EncryptedUserName,
                        section = inputFrontPage.EncryptedUserPW
                    });
            }
            if (!string.IsNullOrEmpty(fullmode))  // If ActionLink named "fullmode" is clicked
            {
                return RedirectToAction(
                    actionName: "WelcomePageReturn",
                    controllerName: "PWSFullScreening",
                    routeValues: new
                    {
                        coID = inputFrontPage.CoID,
                        loID = inputFrontPage.LoID,
                        envCode = inputFrontPage.EnvironCode,
                        languageid = inputFrontPage.LanguageID,
                        block = inputFrontPage.EncryptedUserName,
                        section = inputFrontPage.EncryptedUserPW
                    });
            }
            return View();
        }
    }
}