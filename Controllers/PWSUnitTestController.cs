using ScreeningTesterWebSite.Models;
using System.Web.Mvc;

namespace ScreeningTesterWebSite.Controllers
{
    public class PWSUnitTestController : Controller
    {
        public ActionResult PWSUnitTestPage(InputFrontPage inputFrontPage)
        {
            if (ModelState.IsValid)
            {
                return View(inputFrontPage);
            }
            return View();
        }

        public ActionResult PWSUnitTestPageReturn(string coID, string loID, string envCode, string languageid, string block, string section)
        {
            InputFrontPage retObj = new InputFrontPage()
            {
                CoID = coID,
                LoID = loID,
                EnvironCode = envCode,
                LanguageID = languageid, 
                UserName = StringUtil.Decrypt(block),
                UserPW = StringUtil.Decrypt(section)
            };
            return View(retObj);
        }

        public ActionResult InitializeQuestionnaireSetUp(InputFrontPage inFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputNewApplicant inputInitQuest = new InputNewApplicant();

                inputInitQuest.TransferCommonProperties(inFrontPage);
                inputInitQuest.SetDefaults();

                inputInitQuest.MethodName = "InitializeQuestionnaire";
                inputInitQuest.ResultsTargetPage = "InitializeQuestionnaireResults";
                inputInitQuest.ResultsTargetController = "PWSUnitTest";

                return View(inputInitQuest);
            }
            return View();
        }
        public ActionResult InitializeQuestionnaireResults(InputNewApplicant newApplicantIn)
        {
            if (ModelState.IsValid)
            {
                newApplicantIn.PrepareForInput();

                InitializeQuestionnaireOutput initQuestOut 
                    = UtilityFunctions.GetWebService_Caller(newApplicantIn)
                    .InitializeQuestionnaire(newApplicantIn);

                return View(initQuestOut);
            }
            return View();
        }

        public ActionResult NewApplicantSetUp(InputFrontPage inFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputNewApplicant inputInitQuest = new InputNewApplicant();

                inputInitQuest.TransferCommonProperties(inFrontPage);
                inputInitQuest.SetDefaults();

                inputInitQuest.MethodName = "NewApplicant"; 
                inputInitQuest.ResultsTargetPage = "NewApplicantResults";
                inputInitQuest.ResultsTargetController = "PWSUnitTest";

                return View(inputInitQuest);
            }
            return View();
        }
        public ActionResult NewApplicantResults(InputNewApplicant newApplicantIn)
        {
            if (ModelState.IsValid)
            {
                newApplicantIn.PrepareForInput();

                NewApplicantOutput newApplOut 
                    = UtilityFunctions.GetWebService_Caller(newApplicantIn)
                    .NewApplicant(newApplicantIn);

                return View(newApplOut);
            }
            return View();
        }

        public ActionResult PostAnswersSetUp(InputPostAnswers inputPostAnswers)
        {
            if (ModelState.IsValid)
            {
                inputPostAnswers.SetDefaults();
                return View(inputPostAnswers);
            }
            return View();
        }
        public ActionResult PostAnswersResults(InputPostAnswers inputPostAns)
        {
            if (ModelState.IsValid)
            {
                InputPostAnswers inputPostAnsPrepared = inputPostAns.PrepareAnsweredQuestions();

                NewApplicantOutput postAnswersOut 
                    = UtilityFunctions.GetWebService_Caller(inputPostAnsPrepared)
                    .PostAnswers(inputPostAnsPrepared);

                return View(postAnswersOut);
            }
            return View();
        }

        public ActionResult HireApplicantsSetUp(InputFrontPage inFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputHireApplicants inputHireApplicants = new InputHireApplicants();
                inputHireApplicants.TransferCommonProperties(inFrontPage);
                inputHireApplicants.SetDefaults();

                return View(inputHireApplicants);
            }
            return View();
        }
        public ActionResult HireApplicantsResults(InputHireApplicants inputHireApplicant)
        {
            if (ModelState.IsValid)
            {
                inputHireApplicant.PrepareForInput();

                HireApplicantsOutput hireApplicantOut
                    = UtilityFunctions.GetWebService_Caller(inputHireApplicant)
                    .HireApplicants(inputHireApplicant);

                return View(hireApplicantOut);
            }
            return View();
        }

        public ActionResult GetLastQuestionsSetUp(InputGetLastQuestions inputGetLastQuestions)
        {
            if (ModelState.IsValid)
            {
                inputGetLastQuestions.SetDefaults();
                inputGetLastQuestions.MethodName = "GetLastQuestionsResults";

                return View(inputGetLastQuestions);
            }
            return View();
        }
        
        public ActionResult GetLastQuestionsResults(InputGetLastQuestions inputGetLastQuest)
        {
            if (ModelState.IsValid)
            {
                NewApplicantOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputGetLastQuest)
                    .GetLastQuestions(inputGetLastQuest);

                return View(postAnswersOut);
            }
            return View();
        }

        public ActionResult GetLastPageOfQuestionsSetUp(InputGetLastQuestions inputGetLastQuestions)
        {
            if (ModelState.IsValid)
            {
                inputGetLastQuestions.SetDefaults();
                inputGetLastQuestions.MethodName = "GetLastPageOfQuestionsResults";

                return View(inputGetLastQuestions);
            }
            return View();
        }

        public ActionResult GetLastPageOfQuestionsResults(InputGetLastQuestions inputGetLastQuest)
        {
            if (ModelState.IsValid)
            {
                NewApplicantOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputGetLastQuest)
                    .GetLastPageOfQuestions(inputGetLastQuest);

                return View(postAnswersOut);
            }
            return View();
        }

        public ActionResult GetApplicantStatusSetUp(InputGetLastQuestions inputGetApplicantStatus)
        {
            if (ModelState.IsValid)
            {
                inputGetApplicantStatus.SetDefaults();
                inputGetApplicantStatus.MethodName = "GetApplicantStatusResults";

                return View(inputGetApplicantStatus);
            }
            return View();
        }

        public ActionResult GetApplicantStatusResults(InputGetLastQuestions inputGetApplicantStatus)
        {
            if (ModelState.IsValid)
            {
                GetApplicantStatusOutput postAnswersOut
                    = UtilityFunctions.GetWebService_Caller(inputGetApplicantStatus)
                    .GetApplicantStatus(inputGetApplicantStatus);

                return View(postAnswersOut);
            }
            return View();
        }

        public ActionResult GetCompanyByPhoneNumberSetUp(InputFrontPage inputFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputGetLocationByIncomingPhoneNumber inputGetLocation = new InputGetLocationByIncomingPhoneNumber();
                inputGetLocation.TransferCommonProperties(inputFrontPage);
                inputGetLocation.SetDefaults();

                return View(inputGetLocation);
            }
            return View();
        }
        public ActionResult GetCompanyByPhoneNumberResults(InputGetLocationByIncomingPhoneNumber inputGetLocation)
        {
            if (ModelState.IsValid)
            {
                InputIVRNewApplicant inputIVRNewAppl = new InputIVRNewApplicant(inputGetLocation);

                GetLocationByIncomingPhoneNumberOutput getLocOut
                    = UtilityFunctions.GetWebService_Caller(inputGetLocation)
                    .GetCompanyByPhoneNumber(inputGetLocation);

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
                inputIVRNewAppl.recordingfile = UtilityFunctions.IVRReplaceRecordingFile((getLocOut?.recordingfile ?? "")
                    , inputIVRNewAppl, inputIVRNewAppl.action);

                return View(inputIVRNewAppl);
            }
            return View();
        }

        public ActionResult GetCompanyByCodeSetUp(InputFrontPage inputFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputGetCompanyByCode inputGetLocation = new InputGetCompanyByCode();
                inputGetLocation.TransferCommonProperties(inputFrontPage);
                inputGetLocation.SetDefaults();

                return View(inputGetLocation);
            }
            return View();
        }

        public ActionResult GetCompanyByCodeResults(InputGetCompanyByCode inputGetLocation)
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
                inputIVRNewAppl.recordingfile = UtilityFunctions.IVRReplaceRecordingFile((getLocOut?.recordingfile ?? "")
                    , inputIVRNewAppl, inputIVRNewAppl.action);

                return View(inputIVRNewAppl);
            }
            return View();
        }

            public ActionResult IVRNewApplicantSetUp(InputFrontPage inFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputIVRNewApplicant inputInitQuest = new InputIVRNewApplicant();

                inputInitQuest.TransferCommonProperties(inFrontPage);
                inputInitQuest.SetDefaults();

                return View(inputInitQuest);
            }
            return View();
        }

        public ActionResult IVRNewApplicantResults(InputIVRNewApplicant inputGetLocation)
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
                        (newAppOut?.question?.questionrecording ?? ""), newAppOut, "Question");
                    newAppOut.question.instructionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.instructionrecording ?? ""), newAppOut, "Instruction");
                    newAppOut.question.validationrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.validationrecording ?? ""), newAppOut, "Validation");
                }
                return View(newAppOut);
            }
            return View();
        }

        public ActionResult IVRPostAnswersSetUp(InputFrontPage inFrontPage)
        {
            if (ModelState.IsValid)
            {
                InputIVRPostAnswers inputInitQuest = new InputIVRPostAnswers();

                inputInitQuest.TransferCommonProperties(inFrontPage);
                inputInitQuest.SetDefaults();

                return View(inputInitQuest);
            }
            return View();
        }

        public ActionResult IVRPostAnswersResults(InputIVRPostAnswers inputGetLocation)
        {
            if (ModelState.IsValid)
            {
                inputGetLocation = inputGetLocation.PrepareAnsweredQuestions();
                IVRNewApplicantOutput newAppOut
                   = UtilityFunctions.GetWebService_Caller(inputGetLocation)
                   .IVRPostAnswers(inputGetLocation);

                // Work-around for testing
                newAppOut.redirecturl = UtilityFunctions.IVRReplaceRedirectURL(newAppOut?.redirecturl ?? "", newAppOut);
                newAppOut.recordingfile = UtilityFunctions.IVRReplaceRecordingFile((newAppOut?.recordingfile ?? "")
                    , newAppOut, newAppOut.action);
                if (newAppOut?.question != null)
                {
                    newAppOut.question.questionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.questionrecording ?? ""), newAppOut, "Question");
                    newAppOut.question.instructionrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.instructionrecording ?? ""), newAppOut, "Instruction");
                    newAppOut.question.validationrecording = UtilityFunctions.IVRReplaceRecordingFile(
                        (newAppOut?.question?.validationrecording ?? ""), newAppOut, "Validation");
                }

                return View(newAppOut);
            }
            return View();
        }
    }
}