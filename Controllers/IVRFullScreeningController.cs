using ScreeningTesterWebSite.Models;
using System.Web.Mvc;

namespace ScreeningTesterWebSite.Controllers
{
    public class IVRFullScreeningController : Controller
    {
        public ActionResult CompanyIDAndLOIDLookupReturn(string coID, string loID, string envCode, string languageid, string block, string section)
        {
            InputGetCompanyByCode inputGetLocation = new InputGetCompanyByCode()
            {
                CoID = coID,
                LoID = loID,
                EnvironCode = envCode,
                LanguageID = languageid,
                UserName = StringUtil.Decrypt(block),
                UserPW = StringUtil.Decrypt(section)
            };

            inputGetLocation.SetDefaults();

            return View(inputGetLocation);
        }

        public ActionResult InitialQuestionsPage(InputGetCompanyByCode inputGetLocation)
        {
            if (ModelState.IsValid)
            {
                InputIVRNewApplicant inputIVRNewAppl = new InputIVRNewApplicant(inputGetLocation);

                GetLocationByIncomingPhoneNumberOutput getLocOut
                    = UtilityFunctions.GetWebService_Caller(inputGetLocation)
                    .GetCompanyByCode(inputGetLocation);

                inputIVRNewAppl.CoID = getLocOut.companyid.ToString();
                inputIVRNewAppl.CandidateID = getLocOut.CandidateID;

                inputIVRNewAppl.action = getLocOut?.action ?? "none";
                if (inputIVRNewAppl.action == "RedirectToCSR")
                {
                    inputIVRNewAppl.redirecturl = UtilityFunctions.IVRReplaceRedirectURL(getLocOut?.redirecturl ?? "", inputIVRNewAppl, true);
                }
                else
                {
                    inputIVRNewAppl.redirecturl = "";
                }
                inputIVRNewAppl.recordingfile = UtilityFunctions.IVRReplaceRecordingFile( (getLocOut?.recordingfile ?? "")
                    , inputIVRNewAppl, inputIVRNewAppl.action);

                return View(inputIVRNewAppl);
            }
            return View();
        }
        
        public ActionResult ApplicantInitializePage(InputIVRNewApplicant inputGetLocation)
        {
            if (ModelState.IsValid)
            {
                IVRNewApplicantOutput newAppOut
                   = UtilityFunctions.GetWebService_Caller(inputGetLocation)
                   .IVRNewApplicant(inputGetLocation);

                newAppOut.redirecturl = UtilityFunctions.IVRReplaceRedirectURL(newAppOut?.redirecturl ?? "", newAppOut);
                newAppOut.recordingfile = UtilityFunctions.IVRReplaceRecordingFile((newAppOut?.recordingfile ?? "")
                    , newAppOut, newAppOut.action);
                if (newAppOut?.question != null)
                {
                    newAppOut.question.questionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.questionrecording ?? ""),newAppOut, "Question" );
                    newAppOut.question.instructionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.instructionrecording ?? ""),newAppOut, "Instruction");
                    newAppOut.question.validationrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.validationrecording ?? ""), newAppOut, "Validation");
                }

                return View(newAppOut);
            }
            return View();
        }

        public ActionResult IVRFirstQuestionsPage(IVRNewApplicantOutput inNewApplOut)
        {
            if (inNewApplOut?.question == null)
            {
                if (TempData["InputPageModel"] == null)
                {
                    return View();
                }
                else
                {
                    inNewApplOut = TempData["InputPageModel"] as IVRNewApplicantOutput;
                }
            }
            if (ModelState.IsValid)
            {
                InputIVRPostAnswers inputPostAnsPrepared = new InputIVRPostAnswers(inNewApplOut).PrepareAnsweredQuestions();

                IVRNewApplicantOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputPostAnsPrepared)
                    .IVRPostAnswers(inputPostAnsPrepared);

                postAnswersOut.redirecturl = UtilityFunctions.IVRReplaceRedirectURL(postAnswersOut?.redirecturl ?? "", postAnswersOut);
                postAnswersOut.recordingfile = UtilityFunctions.IVRReplaceRecordingFile((postAnswersOut?.recordingfile ?? "")
                    , postAnswersOut, postAnswersOut.action);

                if (postAnswersOut?.question != null)
                {
                    postAnswersOut.question.questionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.questionrecording ?? ""), postAnswersOut, "Question");
                    postAnswersOut.question.instructionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.instructionrecording ?? ""), postAnswersOut, "Instruction");
                    postAnswersOut.question.validationrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.validationrecording ?? ""), postAnswersOut, "Validation");
                }

                return View(postAnswersOut);
            }
            return View();
        }

        public ActionResult IVRNextQuestionsPage(IVRNewApplicantOutput inNewApplOut)
        {
            if (inNewApplOut?.question == null)
            {
                if (TempData["InputPageModel"] == null)
                {
                    return View();
                }
                else
                {
                    inNewApplOut = TempData["InputPageModel"] as IVRNewApplicantOutput;
                }
            }

            if (ModelState.IsValid)
            {
                InputIVRPostAnswers inputPostAnsPrepared = new InputIVRPostAnswers(inNewApplOut).PrepareAnsweredQuestions();

                IVRNewApplicantOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputPostAnsPrepared)
                    .IVRPostAnswers(inputPostAnsPrepared);

                // Work-around for testing
                postAnswersOut.redirecturl = UtilityFunctions.IVRReplaceRedirectURL(postAnswersOut?.redirecturl ?? "", postAnswersOut);
                postAnswersOut.recordingfile = UtilityFunctions.IVRReplaceRecordingFile( (postAnswersOut?.recordingfile ?? "")
                    , postAnswersOut, postAnswersOut.action);
                if (postAnswersOut?.question != null)
                {
                    postAnswersOut.question.questionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.questionrecording ?? ""), postAnswersOut, "Question");
                    postAnswersOut.question.instructionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.instructionrecording ?? ""), postAnswersOut, "Instruction");
                    postAnswersOut.question.validationrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (postAnswersOut?.question?.validationrecording ?? ""), postAnswersOut, "Validation");
                }

                return View(postAnswersOut);
            }
            return View();
        }
        public ActionResult IVRFirstQuestionsPage_route(IVRNewApplicantOutput inNewApplOut, string nextbutton)
        {
            TempData["InputPageModel"] = inNewApplOut;
            return RedirectToAction("IVRFirstQuestionsPage");
        }
        public ActionResult IVRNextQuestionsPage_route(IVRNewApplicantOutput inNewApplOut, string nextbutton)
        {
            TempData["InputPageModel"] = inNewApplOut;
            return RedirectToAction("IVRNextQuestionsPage");
        }
    }
}