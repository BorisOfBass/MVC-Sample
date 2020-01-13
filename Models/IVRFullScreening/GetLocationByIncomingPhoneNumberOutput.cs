using Newtonsoft.Json;
using System;

namespace ScreeningTesterWebSite.Models
{
    public class GetLocationByIncomingPhoneNumberOutput : ModelBase
    {
        public GetLocationByIncomingPhoneNumberOutput()
        {
        }

        //public GetLocationByReferenceNumberOutput(String xmlString)
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


        public GetLocationByIncomingPhoneNumberOutput(String responseString, String jsonVer)
        {
            if (responseString.Contains("error"))
            {
                this.ReturnedErrorText = "Error returned from Web Service";
            }
            else
            {
                GetLocationApplicant objResultOutput;
                if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                {
                    objResultOutput = JsonConvert.DeserializeObject<GetLocationApplicant>(responseString);
                }
                else
                {
                    objResultOutput =
                       (GetLocationApplicant)
                       UtilityFunctions.XmlDeserializeFromString(responseString, typeof(GetLocationApplicant));
                }
                this.companyid = objResultOutput.companyid;
                this.CoID = this.companyid.ToString();
                this.action = objResultOutput.action;
                this.recordingfile = objResultOutput.recordingfile;
            }
        }

        public Int32 companyid { get; set; }
        public String action { get; set; }
        public String redirecturl { get; set; }
        public String recordingfile { get; set; }

        // Output classes
        public class ResponseGetLocationByIncomingPhoneNumber
        {
            public GetLocationApplicant applicant { get; set; }
        }

        public class GetLocationApplicant
        {
            public Int32 companyid { get; set; }
            public String action { get; set; }
            public String redirecturl { get; set; }
            public String recordingfile { get; set; }
        }
    }
}