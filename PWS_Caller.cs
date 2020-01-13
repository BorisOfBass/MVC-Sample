using ScreeningTesterWebSite.Models;
using ScreeningTesterWebSite.PartnerWebService;
using System;
using System.ServiceModel;

namespace ScreeningTesterWebSite
{
    public class PWS_Caller : IFadvWebServiceCaller
    {
        #region Global Information
        public String UID { get; set; }
        public String PW { get; set; }
        public String EnvironmentCode { get; set; }
        #endregion End of Global Information

        /// <summary>
        /// Constructor with required parameters. Could be refactored into a factory.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pw"></param>
        /// <param name="environmentCode">
        /// Valid Values are D (Dev), Q (QA), U (UAT) and P (Production)
        /// </param>
        public PWS_Caller(String userid, String pw, String environmentCode)
        {
            UID = userid;
            PW = pw;
            if (String.IsNullOrEmpty(environmentCode) || !("PU~PP~PQ~PD~").Contains(environmentCode + "~"))
            {
                environmentCode = "PU";
            }
            EnvironmentCode = environmentCode;
        }

        public InitializeQuestionnaireOutput InitializeQuestionnaire(InputNewApplicant newApplicantIn)
        {
            // Get client and point to the correct environment
            PartnerWebServiceSoapClient pwsClient = GetPartnerWebServiceSoapClient();
            InitializeQuestionnaireOutput retOutput = new InitializeQuestionnaireOutput() { ReturnedErrorText = "" };
            newApplicantIn.UserName = this.UID;
            newApplicantIn.UserPW = this.PW;

            // Get Authentication 
            Authentication auth = null;
            try
            {
                auth = GetPWS_Auth();
            }
            catch (Exception ex1)
            {
                retOutput.ReturnedErrorText = ex1.Message;
            }

            if (retOutput.ReturnedErrorText == "")
            {
                String xmlText = null;
                try
                {
                    newApplicantIn.MethodName = "InitializeQuestionnaire";
                    String xmlFormatted = newApplicantIn.ToXml;
                    xmlText = pwsClient.InitializeQuestionnaire(auth, xmlFormatted);
                    retOutput = new InitializeQuestionnaireOutput(xmlText);
                }
                catch (Exception ex2)
                {
                    retOutput.ReturnedErrorText = ex2.Message;
                }
            }
            retOutput.TransferCommonProperties(newApplicantIn);

            return retOutput;
        }

        public GetLocationByIncomingPhoneNumberOutput GetCompanyByPhoneNumber(InputGetLocationByIncomingPhoneNumber getLocIn)
        {
            throw new NotImplementedException();
        }

        public GetLocationByIncomingPhoneNumberOutput GetCompanyByCode(InputGetCompanyByCode getLocIn)
        {
            throw new NotImplementedException();
        }

        public NewApplicantOutput NewApplicant(InputNewApplicant newApplicantIn)
        {
            // Get client and point to the correct environment
            PartnerWebServiceSoapClient pwsClient = GetPartnerWebServiceSoapClient();
            NewApplicantOutput retOutput = new NewApplicantOutput() { ReturnedErrorText = "" };

            // Get Authentication 
            Authentication auth = null;
            try
            {
                auth = GetPWS_Auth();
            }
            catch (Exception ex1)
            {
                retOutput.ReturnedErrorText = ex1.Message;
            }

            if (retOutput.ReturnedErrorText == "")
            {
                String xmlText = null;
                try
                {
                    newApplicantIn.MethodName = "NewApplicant";
                    String xmlFormatted = newApplicantIn.ToXml;
                    xmlText = pwsClient.NewApplicant(auth, xmlFormatted);
                    retOutput = new NewApplicantOutput(xmlText);
                }
                catch (Exception ex2)
                {
                    retOutput.ReturnedErrorText = ex2.Message;
                }
            }
            if (retOutput.questions == null || retOutput.questions.Count == 0)
            {
                retOutput.QuestionFlowDescription = "End of Questions";
            }
            else
            {
                retOutput.QuestionFlowDescription = "First Questions Presented";
            }
            retOutput.TransferCommonProperties(newApplicantIn);
            retOutput.MethodName = newApplicantIn.MethodName;

            return retOutput;
        }

        public IVRNewApplicantOutput IVRNewApplicant(InputIVRNewApplicant newApplicantIn)
        {
            throw new NotImplementedException();
        }

        public NewApplicantOutput PostAnswers(InputPostAnswers postAnswersIn)
        {
            // Get client and point to the correct environment
            PartnerWebServiceSoapClient pwsClient = GetPartnerWebServiceSoapClient();
            NewApplicantOutput retOutput = new NewApplicantOutput() { ReturnedErrorText = "" };

            // Get Authentication 
            Authentication auth = null;
            try
            {
                auth = GetPWS_Auth();
            }
            catch (Exception ex1)
            {
                retOutput.ReturnedErrorText = ex1.Message;
            }

            if (retOutput.ReturnedErrorText == "")
            {
                String xmlText = null;
                try
                {
                    String xmlFormatted = postAnswersIn.ToXml;
                    xmlText = pwsClient.PostAnswers(auth, xmlFormatted);
                    retOutput = new NewApplicantOutput(xmlText);
                }
                catch (Exception ex2)
                {
                    retOutput.ReturnedErrorText = ex2.Message;
                }
            }
            if (retOutput.questions == null || retOutput.questions.Count == 0)
            {
                retOutput.QuestionFlowDescription = "End of Questions";
            }
            else
            {
                retOutput.QuestionFlowDescription = "Next Questions Presented";
            }
            retOutput.MethodName = "PostAnswers";
            retOutput.TransferCommonProperties(postAnswersIn);

            return retOutput;
        }

        public IVRNewApplicantOutput IVRPostAnswers(InputIVRPostAnswers postAnswersIn)
        {
            throw new NotImplementedException();
        }

        public HireApplicantsOutput HireApplicants(InputHireApplicants hireApplicantsIn)
        {
            // Get client and point to the correct environment
            PartnerWebServiceSoapClient pwsClient = GetPartnerWebServiceSoapClient();
            HireApplicantsOutput retOutput = new HireApplicantsOutput() { ReturnedErrorText = "" };

            // Get Authentication 
            Authentication auth = null;
            try
            {
                auth = GetPWS_Auth();
            }
            catch (Exception ex1)
            {
                retOutput.ReturnedErrorText = ex1.Message;
            }

            if (retOutput.ReturnedErrorText == "")
            {
                String xmlText = null;
                try
                {
                    String xmlFormatted = hireApplicantsIn.ToXml;
                    xmlText = pwsClient.HireApplicants(auth, xmlFormatted);
                    retOutput = new HireApplicantsOutput(xmlText);
                }
                catch (Exception ex2)
                {
                    retOutput.ReturnedErrorText = ex2.Message;
                }
            }
            retOutput.TransferCommonProperties(hireApplicantsIn);
            return retOutput;
        }

        public NewApplicantOutput GetLastQuestions(InputGetLastQuestions getLastQuestionsIn)
        {
            throw new NotImplementedException();
        }

        public NewApplicantOutput GetLastPageOfQuestions(InputGetLastQuestions getLastQuestionsIn)
        {
            throw new NotImplementedException();
        }

        public GetApplicantStatusOutput GetApplicantStatus(InputGetLastQuestions getApplicantStatusIn)
        {
            throw new NotImplementedException();
        }

        private Authentication GetPWS_Auth()
        {
            return new Authentication()
            {
                username = UID,
                password = PW
            };
        }

        private PartnerWebServiceSoapClient GetPartnerWebServiceSoapClient()
        {
            // Get client and point to the correct environment
            PartnerWebServiceSoapClient pwsClient = new PartnerWebServiceSoapClient();

            if (this.EnvironmentCode == "PP")        // PRODUCTION
            {
                pwsClient.Endpoint.Address = new EndpointAddress(@"https://www.jobcredits.com/services/JobCredits/PartnerWebService.asmx");
            }
            else if (this.EnvironmentCode == "PU")   // UAT
            {
                pwsClient.Endpoint.Address = new EndpointAddress(@"https://test.jobcredits.com/services/JobCredits/PartnerWebService.asmx");
            }
            else if (this.EnvironmentCode == "PD")   // Dev
            {
                // This worked for me locally. Not sure if port # is always the same
                pwsClient.Endpoint.Address = new EndpointAddress(@"http://localhost:4706/PartnerWebService.asmx");
            }
            else if (this.EnvironmentCode == "PQ")  // QA
            {
                pwsClient.Endpoint.Address = new EndpointAddress(@"https://qajobcredits.fadv.com/services/JobCredits/PartnerWebService.asmx");
            }
            return pwsClient;
        }
    }
}