using Newtonsoft.Json;
using ScreeningTesterWebSite.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

namespace ScreeningTesterWebSite
{
    public class WebAPI_Caller : IFadvWebServiceCaller
    {
        #region Global Information
        public String UID { get; set; }
        public String PW { get; set; }
        public String EnvironmentCode { get; set; }
        #endregion End of Global Information

        protected HttpClient client;

        public WebAPI_Caller(String userid, String pw, String environmentCode)
        {
            UID = userid;
            PW = pw;
            if (String.IsNullOrEmpty(environmentCode) || !("AD~AQ~AU~AP").Contains(environmentCode + "~"))
            {
                environmentCode = "AQ"; // Default to QA
            }
            EnvironmentCode = environmentCode;

            this.client = new HttpClient();
            if (this.EnvironmentCode == "AD") // Standard Web API Development
            {
                client.BaseAddress = new Uri("http://localhost:33523/");   // Bob's Dev area. Not sure if this will work on other locals.
            }
            else if (this.EnvironmentCode == "AQ")  // Standard Web API QA
            {
                client.BaseAddress = new Uri("http://apacblr01web34q:99/services/WebAPIJobCreditsPartner/");
            }
            else if (this.EnvironmentCode == "AU") // Standard Web API UAT
            {
                client.BaseAddress = new Uri("https://test.jobcredits.com/services/WebAPIJobCreditsPartner/");
            }
            else if (this.EnvironmentCode == "AP") // Standard Web API Production
            {
                client.BaseAddress = new Uri("https://www.jobcredits.com/services/WebAPIJobCreditsPartner/");
            }
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/" + UtilityFunctions.GetWebAPIInputType().ToLower()));
        }

        #region IFadvWebServiceCaller implementation
        public InitializeQuestionnaireOutput InitializeQuestionnaire(InputNewApplicant newApplicantIn)
        {
            InitializeQuestionnaireOutput retOutput = new InitializeQuestionnaireOutput() { ReturnedErrorText = "" };

            newApplicantIn.UserName = this.UID;
            newApplicantIn.UserPW = this.PW;

            InputNewApplicant.InitializeQuestionnaireCompactObject inputParms
                = newApplicantIn.GetSerializableObject_InitializeQuestionnaire();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";
            try
            {
                response =
                    client.PostAsJsonAsync<InputNewApplicant.InitializeQuestionnaireCompactObject>
                        ("api/JobCreditsPartner/1.0.0/InitializeQuestionnaire", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                retOutput = new InitializeQuestionnaireOutput(passedBack, "");
            }
            else
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
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
            NewApplicantOutput retOutput = new NewApplicantOutput() { ReturnedErrorText = "" };

            newApplicantIn.UserName = this.UID;
            newApplicantIn.UserPW = this.PW;

            InputNewApplicant.NewApplicantCompactObject inputParms
                = newApplicantIn.GetSerializableObject_NewApplicant();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/NewApplicant/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputNewApplicant.NewApplicantCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputNewApplicant.NewApplicantCompactObject>
                        ("api/JobCreditsPartner/1.0.0/NewApplicant", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from NewApplicant call: " + passedBack, "INFO");

                retOutput = new NewApplicantOutput(passedBack, newApplicantIn.MethodName, newApplicantIn);
            }
            else
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from NewApplicant call: " + retOutput.ReturnedErrorText, "ERROR");
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
            NewApplicantOutput retOutput = new NewApplicantOutput() { ReturnedErrorText = "" };

            postAnswersIn.UserName = this.UID;
            postAnswersIn.UserPW = this.PW;

            InputPostAnswers.PostAnswersCompactObject inputParms
                = postAnswersIn.GetSerializableObject();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/PostAnswers/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputHireApplicants.HireApplicantsCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputPostAnswers.PostAnswersCompactObject>
                        ("api/JobCreditsPartner/1.0.0/PostAnswers", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from PostAnswers call: " + passedBack, "INFO");

                retOutput = new NewApplicantOutput(passedBack, "PostAnswers", postAnswersIn);
            }
            else 
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from PostAnswers call: " + retOutput.ReturnedErrorText, "ERROR");
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
            HireApplicantsOutput retOutput = new HireApplicantsOutput() { ReturnedErrorText = "" };
            hireApplicantsIn.UserName = this.UID;
            hireApplicantsIn.UserPW = this.PW;

            InputHireApplicants.HireApplicantsCompactObject inputParms
                = hireApplicantsIn.GetSerializableObject();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/HireApplicants/ called withn " + UtilityFunctions.GetWebAPIInputType() + ": " +
                //returnObjectsAsXML(typeof(InputHireApplicants.HireApplicantsCompactObject), inputParms);
                returnObjectAsJSON(typeof(InputHireApplicants.HireApplicantsCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputHireApplicants.HireApplicantsCompactObject>
                        ("api/JobCreditsPartner/1.0.0/HireApplicants", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from HireApplicants call: " + passedBack, "INFO");

                retOutput = new HireApplicantsOutput(passedBack, "");
            }
            else
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from HireApplicants call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            retOutput.TransferCommonProperties(hireApplicantsIn);

            return retOutput;
        }

        public NewApplicantOutput GetLastQuestions(InputGetLastQuestions getLastQuestionsIn)
        {
            return GetLastQuestionsCalls(getLastQuestionsIn, "GetLastQuestions");
        }

        public NewApplicantOutput GetLastPageOfQuestions(InputGetLastQuestions getLastQuestionsIn)
        {
            return GetLastQuestionsCalls(getLastQuestionsIn, "GetLastPageOfQuestions");
        }

        public GetApplicantStatusOutput GetApplicantStatus(InputGetLastQuestions getApplicantStatusIn)
        {
            GetApplicantStatusOutput retOutput = new GetApplicantStatusOutput() { ReturnedErrorText = "" };

            getApplicantStatusIn.UserName = this.UID;
            getApplicantStatusIn.UserPW = this.PW;

            InputGetLastQuestions.GetLastQuestionsCompactObject inputParms
                = getApplicantStatusIn.GetSerializableObject();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            try
            {
                response =
                    client.PostAsJsonAsync<InputGetLastQuestions.GetLastQuestionsCompactObject>
                        ("api/JobCreditsPartner/1.0.0/GetApplicantStatus", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetApplicantStatus call: " + passedBack, "INFO");

                retOutput = new GetApplicantStatusOutput(passedBack, "");
            }
            else
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetApplicantStatus call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            retOutput.TransferCommonProperties(getApplicantStatusIn);

            return retOutput;
        }
        #endregion

        private NewApplicantOutput GetLastQuestionsCalls(InputGetLastQuestions getLastQuestionsIn, String endPointName)
        {
            NewApplicantOutput retOutput = new NewApplicantOutput() { ReturnedErrorText = "" };

            getLastQuestionsIn.UserName = this.UID;
            getLastQuestionsIn.UserPW = this.PW;

            InputGetLastQuestions.GetLastQuestionsCompactObject inputParms
                = getLastQuestionsIn.GetSerializableObject();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = "1.0.0/" + endPointName + " called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsJSON(typeof(InputGetLastQuestions.GetLastQuestionsCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputGetLastQuestions.GetLastQuestionsCompactObject>
                        ("api/JobCreditsPartner/1.0.0/" + endPointName, inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from " + endPointName + " call: " + passedBack, "INFO");

                retOutput = new NewApplicantOutput(passedBack, "PostAnswers", getLastQuestionsIn);
            }
            else
            {
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from " + endPointName + " call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            retOutput.TransferCommonProperties(getLastQuestionsIn);

            return retOutput;
        }

        private String returnObjectAsCurrentFormatType(Type objType, Object objInstantiation)
        {
            try
            {
                if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                {
                    return returnObjectAsJSON(objType, objInstantiation);
                }
                else
                {
                    return returnObjectsAsXML(objType, objInstantiation);
                }
            }
            catch (Exception ex) // swallow error
            {
                return "Error in serialization: " + ex.Message;
            }
        }
        private String returnObjectAsJSON(Type objType, Object objInstantiation)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(objType);
            MemoryStream stream1 = new MemoryStream();
            ser.WriteObject(stream1, objInstantiation);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);

            return sr.ReadToEnd(); ;
        }
        private String returnObjectsAsXML(Type objType, Object objInstantiation)
        {
            XmlMediaTypeFormatter formatter = new XmlMediaTypeFormatter();
            MemoryStream stream1 = new MemoryStream();
            StreamContent content = new StreamContent(stream1);
            /// Serialize the object.
            formatter.WriteToStreamAsync(objType, objInstantiation, stream1, content, null).Wait();
            // Read the serialized string.
            stream1.Position = 0;

            return content.ReadAsStringAsync().Result; 
        }

        private String ExtractErrorMessage(String responseString)
        {
            WebAPI_Caller.ErrorResponse objResultOutput;
            try
            {
                objResultOutput = JsonConvert.DeserializeObject<WebAPI_Caller.ErrorResponse>(responseString);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
            if (objResultOutput == null || String.IsNullOrEmpty(objResultOutput.message))
            {
                // Return generic message
                return "Web Service Error";
            }
            else
            {
                return objResultOutput.message;
            }
        }

        public class ErrorResponse
        {
            public String code { get; set; }
            public String message { get; set; }
        }
    }
}