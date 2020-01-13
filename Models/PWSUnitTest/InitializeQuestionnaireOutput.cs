using Newtonsoft.Json;
using System;
using System.Xml;

namespace ScreeningTesterWebSite.Models
{
    public class InitializeQuestionnaireOutput : ModelBase
    {
        public InitializeQuestionnaireOutput()
        {
        }

        public InitializeQuestionnaireOutput(String xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            this.ReturnedErrorText = null;

            XmlNode optOutTextNode = null;
            XmlNode applicantNode = null;
            XmlNode normalizedAddressNode = null;
            XmlNode errorNode = null;
            if (xmlDoc.HasChildNodes)
            {
                XmlNode initializequestionnaireReturnNode = null;
                if (xmlDoc.ChildNodes[0].Name == "initializequestionnairereturn")
                {
                    initializequestionnaireReturnNode = xmlDoc.ChildNodes[0];
                }
                else if (applicantNode.ChildNodes[0].Name == "error")
                {
                    errorNode = applicantNode.ChildNodes[0];
                }

                if (initializequestionnaireReturnNode != null)
                {
                    if (initializequestionnaireReturnNode.HasChildNodes) 
                    {
                        if (initializequestionnaireReturnNode.ChildNodes.Count == 2)
                        {
                            optOutTextNode = initializequestionnaireReturnNode.ChildNodes[0];
                            applicantNode = initializequestionnaireReturnNode.ChildNodes[1];
                            if (applicantNode.HasChildNodes && applicantNode.ChildNodes[0].ChildNodes.Count == 4)
                            {
                                normalizedAddressNode = applicantNode.ChildNodes[0];
                                this.address = normalizedAddressNode.ChildNodes[0].ChildNodes[0].Value;
                                this.city = normalizedAddressNode.ChildNodes[1].ChildNodes[0].Value;
                                this.state = normalizedAddressNode.ChildNodes[2].ChildNodes[0].Value;
                                this.zip = normalizedAddressNode.ChildNodes[3].ChildNodes[0].Value;
                            }
                        }
                        else if (initializequestionnaireReturnNode.ChildNodes.Count == 1)
                        {
                            errorNode = initializequestionnaireReturnNode.ChildNodes[0];
                        }
                    }
                    if (optOutTextNode?.ChildNodes.Count > 0)
                    {
                        this.optouttext = optOutTextNode.ChildNodes[0].Value;
                    }
                }
            }
            if (errorNode == null && normalizedAddressNode == null)
            {
                this.ReturnedErrorText = "Returned XML not in expected format.";
                return;
            }

            if (applicantNode?.Attributes["companyid"] != null)
            {
                this.CoID = applicantNode.Attributes["companyid"].Value;
            }
            if (applicantNode?.Attributes["externalid"] != null)
            {
                this.CandidateID = applicantNode.Attributes["externalid"].Value;
            }
            if (errorNode != null)
            {
                this.ReturnedErrorText = errorNode.ChildNodes[0].InnerText.Trim();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="jsonVer">Always blank string</param>
        public InitializeQuestionnaireOutput(String responseString, String jsonVer)
        {
            if (responseString.Contains("error"))
            {
                this.ReturnedErrorText = "Error returned from Web Service";
            }
            else
            {
                InitializeQuestionnaireOutput.ResponseInitializeQuestionnaire objResultOutput;
                if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                {
                    objResultOutput = JsonConvert.DeserializeObject<ResponseInitializeQuestionnaire>(responseString);
                }
                else
                {
                    objResultOutput =
                       (InitializeQuestionnaireOutput.ResponseInitializeQuestionnaire)
                       UtilityFunctions.XmlDeserializeFromString(responseString, typeof(ResponseInitializeQuestionnaire));
                }
                this.optouttext = objResultOutput.optouttext;
                this.address = objResultOutput.normalizedaddress.address;
                this.city = objResultOutput.normalizedaddress.city;
                this.state = objResultOutput.normalizedaddress.state;
                this.zip = objResultOutput.normalizedaddress.zip;

                // These are no longer sent back in result set 
                // this.CoID = "";
                //this.CandidateID = "";
            } 
        }

        public String optouttext { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zip { get; set; }
        public String countrycode { get; set; }

        // Output classes
        [Serializable()]
        public class ResponseInitializeQuestionnaire
        {
            [System.Xml.Serialization.XmlElementAttribute("optouttext")]
            public string optouttext { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("normalizedaddress")]
            public Normalizedaddress normalizedaddress { get; set; }
        }

        public class Normalizedaddress
        {
            [System.Xml.Serialization.XmlElementAttribute("address")]
            public string address { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("city")]
            public string city { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("state")]
            public string state { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("zip")]
            public string zip { get; set; }
        }
    }
}