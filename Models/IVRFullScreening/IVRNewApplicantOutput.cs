using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScreeningTesterWebSite.Models
{
    public class IVRNewApplicantOutput : ModelBase
    {
        public IVRNewApplicantOutput()
        {
        }

        //public IVRNewApplicantOutput(String xmlString)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xmlString);

        //    this.ReturnedErrorText = null;

        //    XmlNode optOutTextNode = null;
        //    XmlNode applicantNode = null;
        //    XmlNode normalizedAddressNode = null;
        //    XmlNode errorNode = null;
        //    if (xmlDoc.HasChildNodes)
        //    {
        //        XmlNode initializequestionnaireReturnNode = null;
        //        if (xmlDoc.ChildNodes[0].Name == "initializequestionnairereturn")
        //        {
        //            initializequestionnaireReturnNode = xmlDoc.ChildNodes[0];
        //        }
        //        else if (applicantNode.ChildNodes[0].Name == "error")
        //        {
        //            errorNode = applicantNode.ChildNodes[0];
        //        }

        //        if (initializequestionnaireReturnNode != null)
        //        {
        //            if (initializequestionnaireReturnNode.HasChildNodes)
        //            {
        //                if (initializequestionnaireReturnNode.ChildNodes.Count == 2)
        //                {
        //                    optOutTextNode = initializequestionnaireReturnNode.ChildNodes[0];
        //                    applicantNode = initializequestionnaireReturnNode.ChildNodes[1];
        //                    if (applicantNode.HasChildNodes && applicantNode.ChildNodes[0].ChildNodes.Count == 4)
        //                    {
        //                        normalizedAddressNode = applicantNode.ChildNodes[0];
        //                        this.address = normalizedAddressNode.ChildNodes[0].ChildNodes[0].Value;
        //                        this.city = normalizedAddressNode.ChildNodes[1].ChildNodes[0].Value;
        //                        this.state = normalizedAddressNode.ChildNodes[2].ChildNodes[0].Value;
        //                        this.zip = normalizedAddressNode.ChildNodes[3].ChildNodes[0].Value;
        //                    }
        //                }
        //                else if (initializequestionnaireReturnNode.ChildNodes.Count == 1)
        //                {
        //                    errorNode = initializequestionnaireReturnNode.ChildNodes[0];
        //                }
        //            }
        //            if (optOutTextNode?.ChildNodes.Count > 0)
        //            {
        //                this.optouttext = optOutTextNode.ChildNodes[0].Value;
        //            }
        //        }
        //    }
        //    if (errorNode == null && normalizedAddressNode == null)
        //    {
        //        this.ReturnedErrorText = "Returned XML not in expected format.";
        //        return;
        //    }

        //    if (applicantNode?.Attributes["companyid"] != null)
        //    {
        //        this.CoID = applicantNode.Attributes["companyid"].Value;
        //    }
        //    if (applicantNode?.Attributes["externalid"] != null)
        //    {
        //        this.CandidateID = applicantNode.Attributes["externalid"].Value;
        //    }
        //    if (errorNode != null)
        //    {
        //        this.ReturnedErrorText = errorNode.ChildNodes[0].InnerText.Trim();
        //    }
        //}


        public IVRNewApplicantOutput(String responseString, String jsonVer)
        {
            if (responseString.Contains("Error") && !responseString.Contains("Valid"))
            {
                this.ReturnedErrorText = "Error returned from Web Service";
            }
            else
            {
                ResponseIVRNewApplicant objCommonResultOutput;
                if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                {
                    objCommonResultOutput = JsonConvert.DeserializeObject<ResponseIVRNewApplicant>(responseString);
                }
                else
                {
                    objCommonResultOutput =
                       (ResponseIVRNewApplicant)
                       UtilityFunctions.XmlDeserializeFromString(responseString, typeof(ResponseIVRNewApplicant));
                }

                this.applicantid = objCommonResultOutput.applicant.applicantid;
                this.action = objCommonResultOutput.applicant?.action ?? "none";
                if (objCommonResultOutput.applicant?.question != null)
                {
                    this.question = GetQuestionFromResponse(objCommonResultOutput.applicant.question);
                }
                this.redirecturl = objCommonResultOutput.applicant?.redirecturl ?? "none";
                this.recordingfile = objCommonResultOutput.applicant?.recordingfile ?? "none";
            }
        }

        public Int32 applicantid { get; set; }
        public String action { get; set; }
        public IVRReturnQuestion question { get; set; }
        public String redirecturl { get; set; }
        public String recordingfile { get; set; }

        public class IVRReturnQuestion
        {
            public Int32 questionid { get; set; }
            public String answertype { get; set; }
            public Int32 answersize { get; set; }
            public String answervalue { get; set; }
            public String questiontext { get; set; }
            public String questionrecording { get; set; }
            public String instructionrecording { get; set; }
            public String validationrecording { get; set; }
            public String answerrecordingfilename { get; set; }
            public String answerrecordingfiledirectory { get; set; }
        }
        private IVRReturnQuestion GetQuestionFromResponse(IVRReturnQuestion respQuest)
        {
            IVRReturnQuestion retQuestion = new IVRReturnQuestion()
            {
                questionid = respQuest.questionid,
                answertype = respQuest.answertype,
                answersize = respQuest.answersize,
                questiontext = respQuest.questiontext,
                questionrecording = respQuest?.questionrecording ?? "",
                instructionrecording = respQuest?.instructionrecording ?? "",
                validationrecording = respQuest?.validationrecording ?? "",
                answerrecordingfiledirectory = respQuest?.answerrecordingfiledirectory ?? "",
                answerrecordingfilename = respQuest?.answerrecordingfilename ?? ""
            };
            return retQuestion;
        }

        // Output classes
        public class ResponseIVRNewApplicant
        {
            public IVRNewApplicantApplicant applicant { get; set; }
        }

        public class IVRNewApplicantApplicant
        {
            public Int32 applicantid { get; set; }
            public String action { get; set; }
            public IVRReturnQuestion question { get; set; }
            public String redirecturl { get; set; }
            public String recordingfile { get; set; }
        }
    }
}