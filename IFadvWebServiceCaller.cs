using ScreeningTesterWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreeningTesterWebSite
{
    public interface IFadvWebServiceCaller
    {
        String UID { get; set; }
        String PW { get; set; }
        String EnvironmentCode { get; set; }

        InitializeQuestionnaireOutput InitializeQuestionnaire(InputNewApplicant newApplicantIn);
        GetLocationByIncomingPhoneNumberOutput GetCompanyByPhoneNumber(InputGetLocationByIncomingPhoneNumber getLocIn);
        GetLocationByIncomingPhoneNumberOutput GetCompanyByCode(InputGetCompanyByCode getLocIn);
        NewApplicantOutput NewApplicant(InputNewApplicant newApplicantIn);
        IVRNewApplicantOutput IVRNewApplicant(InputIVRNewApplicant newApplicantIn);
        NewApplicantOutput PostAnswers(InputPostAnswers postAnswersIn);
        IVRNewApplicantOutput IVRPostAnswers(InputIVRPostAnswers postAnswersIn);
        HireApplicantsOutput HireApplicants(InputHireApplicants hireApplicantsIn);
        NewApplicantOutput GetLastQuestions(InputGetLastQuestions getLastQuestionsIn);
        NewApplicantOutput GetLastPageOfQuestions(InputGetLastQuestions getLastQuestionsIn);
        GetApplicantStatusOutput GetApplicantStatus(InputGetLastQuestions getApplicantStatusIn);
    }
}
