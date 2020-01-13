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
    public class IVRWebService_Caller : IFadvWebServiceCaller
    {
        #region Global Information
        public String UID { get; set; }
        public String PW { get; set; }
        public String EnvironmentCode { get; set; }
        #endregion End of Global Information

        protected HttpClient client;

        public IVRWebService_Caller(String userid, String pw, String environmentCode)
        {
            UID = userid;
            PW = pw;
            if (String.IsNullOrEmpty(environmentCode) || !("ID~IQ~IU~IP~").Contains(environmentCode + "~"))
            {
                environmentCode = "ID"; // Default to Local
            }
            EnvironmentCode = environmentCode;
            
            this.client = new HttpClient();
            String endPointURL = GetEndPointURL();

            client.BaseAddress = new Uri(endPointURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/" + UtilityFunctions.GetWebAPIInputType().ToLower()));
        }
        private String GetEndPointURL()
        {
            String retVal = ""; //"Unable to determine Endpoint";

            if (this.EnvironmentCode == "ID") // IVR Web API Development
            {
                retVal = @"http://localhost:33523/";
            }
            else if (this.EnvironmentCode == "IQ") // IVR Web API QA
            {
                retVal = @"http://qajobcredits.fadv.com/services/WebAPIIVR/";
            }
            else if (this.EnvironmentCode == "IU") // IVR Web API UAT
            {
                retVal = @"https://test.jobcredits.com/services/WebAPIIVR/";
            }
            else if (this.EnvironmentCode == "IP") // IVR Web API Production
            {
                retVal = @"https://www.jobcredits.com/services/WebAPIIVR/";
            }
            return retVal;
        }

        #region IFadvWebServiceCaller implementation
        public GetLocationByIncomingPhoneNumberOutput GetCompanyByPhoneNumber(InputGetLocationByIncomingPhoneNumber getLocIn)
        {
            GetLocationByIncomingPhoneNumberOutput retOutput = new GetLocationByIncomingPhoneNumberOutput() { ReturnedErrorText = "" };

            getLocIn.UserName = this.UID;
            getLocIn.UserPW = this.PW;

            InputGetLocationByIncomingPhoneNumber.GetLocationByIncomingPhoneNumberCompactObject inputParms
                = getLocIn.GetSerializableObject();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/GetCompanyByPhoneNumber/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputGetLocationByIncomingPhoneNumber.GetLocationByIncomingPhoneNumberCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputGetLocationByIncomingPhoneNumber.GetLocationByIncomingPhoneNumberCompactObject>
                        ("api/IVR/1.0.0/GetCompanyByPhoneNumber", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK 
                    && passedBack != null && passedBack != String.Empty);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetCompanyByPhoneNumber call: " + passedBack, "INFO");

                retOutput = new GetLocationByIncomingPhoneNumberOutput(passedBack, "");
                retOutput.EnvironCode = getLocIn.EnvironCode;
                retOutput.CandidateID = getLocIn.CandidateID;
                retOutput.UserName = getLocIn.UserName;
                retOutput.UserPW = getLocIn.UserPW;
            }
            else
            {
                if (passedBack == null || passedBack == String.Empty)
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + 
                        "GetCompanyByPhoneNumber Web Service Call did not return an Error Status, but returned an empty result.";
                }
                else if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + ExtractErrorMessage(passedBack);
                }
                retOutput.TransferCommonProperties(getLocIn);

                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetCompanyByPhoneNumber call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            
            return retOutput;
        }

        public GetLocationByIncomingPhoneNumberOutput GetCompanyByCode(InputGetCompanyByCode getLocIn)
        {
            GetLocationByIncomingPhoneNumberOutput retOutput = new GetLocationByIncomingPhoneNumberOutput() { ReturnedErrorText = "" };

            getLocIn.UserName = this.UID;
            getLocIn.UserPW = this.PW;

            InputGetCompanyByCode.GetCompanyByCodeCompactObject inputParms
                = getLocIn.GetSerializableObject_();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/GetCompanyByCode/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputGetCompanyByCode.GetCompanyByCodeCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputGetCompanyByCode.GetCompanyByCodeCompactObject>
                        ("api/IVR/1.0.0/GetCompanyByCode", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK
                    && passedBack != null && passedBack != String.Empty);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetCompanyByCode call: " + passedBack, "INFO");

                retOutput = new GetLocationByIncomingPhoneNumberOutput(passedBack, "");
                retOutput.EnvironCode = getLocIn.EnvironCode;
                retOutput.CandidateID = getLocIn.CandidateID;
                retOutput.UserName = getLocIn.UserName;
                retOutput.UserPW = getLocIn.UserPW;
            }
            else
            {
                if (passedBack == null || passedBack == String.Empty)
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + 
                        "GetCompanyByCode Web Service Call did not return an Error Status, but returned an empty result.";
                }
                else if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + ExtractErrorMessage(passedBack);
                }
                retOutput.TransferCommonProperties(getLocIn);

                // Log response
                UtilityFunctions.AddLogEntry("Response returned from GetCompanyByCode call: " + retOutput.ReturnedErrorText, "ERROR");
            }

            return retOutput;
        }

        public InitializeQuestionnaireOutput InitializeQuestionnaire(InputNewApplicant newApplicantIn)
        {
            throw new NotImplementedException();
        }

        public NewApplicantOutput NewApplicant(InputNewApplicant newApplicantIn)
        {
            throw new NotImplementedException();
        }

        public IVRNewApplicantOutput IVRNewApplicant(InputIVRNewApplicant newApplicantIn)
        {
            IVRNewApplicantOutput retOutput = new IVRNewApplicantOutput() { ReturnedErrorText = "" };

            InputIVRNewApplicant ivrNewApplicantIn = new InputIVRNewApplicant()
            {
                UserName = this.UID,
                UserPW = this.PW,
                CandidateID = newApplicantIn.CandidateID,
                CoID = newApplicantIn.CoID,
                EmID = newApplicantIn.EmID,
                EnvironCode = newApplicantIn.EnvironCode,
                LoID = newApplicantIn.LoID,
                ssn = newApplicantIn.ssn,
                zip = newApplicantIn.zip,
                under40 = newApplicantIn.under40,
                birthdate = newApplicantIn.birthdate,
            };
            
            InputIVRNewApplicant.NewApplicantCompactObject inputParms
                = ivrNewApplicantIn.GetSerializableObject_NewApplicant();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/NewApplicant/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputIVRNewApplicant.NewApplicantCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputIVRNewApplicant.NewApplicantCompactObject>
                        ("api/IVR/1.0.0/NewApplicant", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK
                    && passedBack != null && passedBack != String.Empty);
            }
            catch (Exception ex)
            {
                postGood = false;
                retOutput.ReturnedErrorText = ex.Message;
            }

            if (postGood)
            {
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from IVRNewApplicant call: " + passedBack, "INFO");

                retOutput = new IVRNewApplicantOutput(passedBack, "NewApplicant"); 
            }
            else
            {
                if (passedBack == null || passedBack == String.Empty)
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + 
                        "IVRNewApplicant Web Service Call did not return an Error Status, but returned an empty result.";
                }
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from IVRNewApplicant call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            retOutput.TransferCommonProperties(newApplicantIn);

            return retOutput;
        }

        public NewApplicantOutput PostAnswers(InputPostAnswers postAnswersIn)
        {
            throw new NotImplementedException();
        }

        public IVRNewApplicantOutput IVRPostAnswers(InputIVRPostAnswers postAnswersIn)
        {
            IVRNewApplicantOutput retOutput = new IVRNewApplicantOutput() { ReturnedErrorText = "" };

            postAnswersIn.UserName = this.UID;
            postAnswersIn.UserPW = this.PW;

            InputIVRPostAnswers.PostAnswersIVRCompactObject inputParms
                = postAnswersIn.GetSerializableObject_IVR();

            HttpResponseMessage response = new HttpResponseMessage();
            Boolean postGood = true;
            String passedBack = "";

            // Log what was called and sent
            String log1 = @"1.0.0/PostAnswers/ called with " + UtilityFunctions.GetWebAPIInputType() + ": " +
                returnObjectAsCurrentFormatType(typeof(InputPostAnswers.PostAnswersIVRCompactObject), inputParms);
            UtilityFunctions.AddLogEntry(log1, "INFO");

            try
            {
                response =
                    client.PostAsJsonAsync<InputIVRPostAnswers.PostAnswersIVRCompactObject>
                        ("api/IVR/1.0.0/PostAnswers", inputParms)
                        .Result;

                passedBack = response.Content.ReadAsStringAsync().Result;

                postGood = (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.OK
                    && passedBack != null && passedBack != String.Empty);
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

                retOutput = new IVRNewApplicantOutput(passedBack, "");
            }
            else
            {
                if (passedBack == null || passedBack == String.Empty)
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + 
                        "IVRPostAnswers Web Service Call did not return an Error Status, but returned an empty result.";
                }
                if (String.IsNullOrEmpty(retOutput.ReturnedErrorText))
                {
                    retOutput.ReturnedErrorText = "EndPoint: " + GetEndPointURL() + ", " + ExtractErrorMessage(passedBack);
                }
                // Log response
                UtilityFunctions.AddLogEntry("Response returned from PostAnswers call: " + retOutput.ReturnedErrorText, "ERROR");
            }
            retOutput.TransferCommonProperties(postAnswersIn);

            return retOutput;
        }
         
        public HireApplicantsOutput HireApplicants(InputHireApplicants hireApplicantsIn)
        {
            throw new NotImplementedException();
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
        #endregion

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
    }
}