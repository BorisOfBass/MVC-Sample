using ScreeningTesterWebSite.Models;
using System.Web.Mvc;

namespace ScreeningTesterWebSite.Controllers
{
    public class PWSFullScreeningController : Controller
    {
        #region Step 1 - The Welcome Page
        /// <summary>
        /// Step 1 - The Welcome Page
        /// Given the Global information from the front page, returns 
        /// an InputNewApplicant object 
        /// </summary>
        /// <param name="inputFrontPage"></param>
        /// <returns></returns>
        public ActionResult WelcomePage(InputFrontPage inputFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputNewApplicant inputInitQuest = new InputNewApplicant();

                inputInitQuest.TransferCommonProperties(inputFrontPage);
                inputInitQuest.SetDefaults();

                inputInitQuest.MethodName = "InitializeQuestionnaire";
                inputInitQuest.ResultsTargetPage = "AddressConfirmation";
                inputInitQuest.ResultsTargetController = "PWSFullScreening";

                return View(inputInitQuest);
            }
            return View();
        }
        /// <summary>
        /// Given paramerterized Global information, returns an InputNewApplication object
        /// </summary>
        /// <param name="coID"></param>
        /// <param name="loID"></param>
        /// <param name="envCode"></param>
        /// <returns></returns>
        public ActionResult WelcomePageReturn(string coID, string loID, string envCode, string languageid, string block, string section)
        {
            InputNewApplicant inputInitQuest = new InputNewApplicant()
            {
                CoID = coID,
                LoID = loID,
                EnvironCode = envCode,
                LanguageID = languageid,
                UserName = StringUtil.Decrypt(block),
                UserPW = StringUtil.Decrypt(section)
            };

            inputInitQuest.SetDefaults();

            inputInitQuest.MethodName = "InitializeQuestionnaire";
            inputInitQuest.ResultsTargetPage = "AddressConfirmation";
            inputInitQuest.ResultsTargetController = "PWSFullScreening";

            return View(inputInitQuest);
        }
        #endregion End of Step 1 - The Welcome Page

        #region Step 2 - Address Confirmation
        /// <summary>
        /// Given a InputNewApplicant object, returns an AddressConfirmationOutput object, which 
        /// is an extended InitializeQuestionnaireOutput object
        /// </summary>
        /// <param name="newApplicantIn"></param>
        /// <returns></returns>
        public ActionResult AddressConfirmation(InputNewApplicant newApplicantIn)
        {
            if (ModelState.IsValid)
            {
                newApplicantIn.PrepareForInput();

                InitializeQuestionnaireOutput initQuestOut
                    = UtilityFunctions.GetWebService_Caller(newApplicantIn)
                    .InitializeQuestionnaire(newApplicantIn);

                AddressConfirmationOutput addressConfOut = new AddressConfirmationOutput(initQuestOut, newApplicantIn);

                return View(addressConfOut);
            }
            return View();
        }
        #endregion End of Step 2 - Address Confirmation

        #region Steps 3 - N  Presenting Questions
        /// <summary>
        /// Given an AddressConfirmationOutput object, returns
        /// a NewApplicantOutput object
        /// </summary>
        /// <param name="intQuestOutput"></param>
        /// <returns></returns>
        public ActionResult FirstQuestionsPage(AddressConfirmationOutput intQuestOutput)
        {
            if (ModelState.IsValid)
            {
                InputNewApplicant newApplicantIn = new InputNewApplicant(intQuestOutput);
                
                NewApplicantOutput newApplOut
                    = UtilityFunctions.GetWebService_Caller(newApplicantIn)
                    .NewApplicant(newApplicantIn);

                return View(newApplOut);
            }
            return View();
        }
        /// <summary>
        /// Given a NewApplicantOutput object, will process the entered answers
        /// and return another NewApplicantOutput object in return.
        /// </summary>
        /// <param name="inNewApplOut"></param>
        /// <returns></returns>
        public ActionResult NextQuestionsPage(NewApplicantOutput inNewApplOut)
        {
            if (inNewApplOut?.questions == null)
            {
                if (TempData["InputPageModel"] == null)
                {
                    return View();
                }
                else
                {
                    inNewApplOut = TempData["InputPageModel"] as NewApplicantOutput;
                }
            }

            if (ModelState.IsValid)
            {
                InputPostAnswers inputPostAnsPrepared = new InputPostAnswers(inNewApplOut);

                NewApplicantOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputPostAnsPrepared)
                    .PostAnswers(inputPostAnsPrepared);

                return View(postAnswersOut);
            }
            return View();
        }
        /// <summary>
        /// Given a NewApplicantOutput object, will pass back the result of NextQuestionsPage. 
        /// When calling the NextQuestionsPage from itself, it will use the Model in a 
        /// state it was in before changes were done.
        /// </summary>
        /// <param name="inNewApplOut"></param>
        /// <returns></returns>
        public ActionResult PreviousQuestionsPage(NewApplicantOutput inNewApplOut)
        {
            if (TempData["InputPageModel"] == null)
            {
                return View();
            }
            else
            {
                inNewApplOut = TempData["InputPageModel"] as NewApplicantOutput;
                InputGetLastQuestions inGetLastQuestion = new InputGetLastQuestions(inNewApplOut);

                if (ModelState.IsValid)
                {
                    NewApplicantOutput getLastQuestionOut
                        = UtilityFunctions.GetWebService_Caller(inGetLastQuestion)
                        .GetLastQuestions(inGetLastQuestion);

                    return View(getLastQuestionOut);
                }
            }
            return View();
        }

        public ActionResult NextQuestionsPage_route(NewApplicantOutput inNewApplOut, string nextbutton, string prevbutton)
        {
            TempData["InputPageModel"] = inNewApplOut;
            if (!string.IsNullOrEmpty(nextbutton))
            {
                return RedirectToAction("NextQuestionsPage");
            }
            else
            {          
                return RedirectToAction("PreviousQuestionsPage");
            }
        }
        #endregion  End of Steps 3 - N  Presenting Questions
    }
}