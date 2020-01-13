using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ScreeningTesterWebSite.Models
{
    public class HireApplicantsOutput : ModelBase
    {
        public HireApplicantsOutput()
        {
        }
        public HireApplicantsOutput(String xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            credits = new List<credit>();
            statuses = new List<status>();

            this.ReturnedErrorText = null;

            XmlNode applicantNode = null;
            XmlNode creditsNode = null;
            XmlNode formsNode = null;
            XmlNode statusNode = null;
            XmlNode errorNode = null;
            if (xmlDoc.HasChildNodes)
            {
                XmlNode newapplicantReturnNode = xmlDoc.ChildNodes[0];
                if (newapplicantReturnNode.HasChildNodes)
                {
                    applicantNode = newapplicantReturnNode.ChildNodes[0];
                    if (applicantNode.HasChildNodes)
                    {
                        for (int i = 0; i < applicantNode.ChildNodes.Count; i++)
                        {
                            if (applicantNode.ChildNodes[i].Name == "credits")
                            {
                                creditsNode = applicantNode.ChildNodes[i];
                            }
                            else if (applicantNode.ChildNodes[i].Name == "forms")
                            {
                                formsNode = applicantNode.ChildNodes[i];
                            }
                            else if (applicantNode.ChildNodes[i].Name == "status")
                            {
                                statusNode = applicantNode.ChildNodes[i];
                            }
                            else if (applicantNode.ChildNodes[i].Name == "error")
                            {
                                errorNode = applicantNode.ChildNodes[i];
                            }
                        }
                    }
                }
            }
            if (errorNode == null && creditsNode == null && formsNode == null && statusNode == null)
            {
                this.ReturnedErrorText = "Returned XML not in expected format.";
                return;
            }
            
            if (applicantNode?.Attributes["companyid"] != null)
            {
                this.CoID = applicantNode.Attributes["companyid"].Value;
            }
            if (applicantNode?.Attributes["id"] != null)
            {
                this.EmID = applicantNode.Attributes["id"].Value;
            }
            if (applicantNode?.Attributes["externalid"] != null)
            {
                this.CandidateID = applicantNode.Attributes["externalid"].Value;
            }
            if (errorNode != null)
            {
                this.ReturnedErrorText = errorNode.ChildNodes[0].InnerText.Trim();
            }
            if (creditsNode != null)
            {
                for (int i = 0; i < creditsNode.ChildNodes.Count; i++)
                {
                    credits.Add(new credit()
                    {
                        credittype = creditsNode.ChildNodes[i].Name,
                        qualified = creditsNode.ChildNodes[i].Attributes[0].Value
                    });
                }
            }
            if (formsNode != null)
            {
                // TO DO
            }
            if (statusNode != null)
            {
                for (int i = 0; i < statusNode.ChildNodes.Count; i++)
                {
                    statuses.Add(new status()
                    {
                        processstep = statusNode.ChildNodes[i].Name,
                        statuscode = statusNode.ChildNodes[i].ChildNodes[0].Value
                    });
                }
            }
        }
        public HireApplicantsOutput(String jsonString, String jsonVer)
        {
            credits = new List<credit>();
            statuses = new List<status>();
            ReturnedErrorText = null;

            HireApplicantsRootobject objResultOutput =
                       JsonConvert.DeserializeObject<HireApplicantsRootobject>(jsonString);

            if (objResultOutput.screeningresults?.credits != null)
            {
                this.credits.Add(
                    new credit()
                    {
                        credittype = "Federal",
                        qualified = (objResultOutput.screeningresults?.credits.federal.qualified == true ? "Yes" : "No")
                    });
                this.credits.Add(
                    new credit()
                    {
                        credittype = "State",
                        qualified = (objResultOutput.screeningresults?.credits.state.qualified == true ? "Yes" : "No")
                    });
            }
            if (objResultOutput.screeningresults?.forms != null)
            {
                // TO DO , add forms to the underlying class and pass around, display etc.
            }
            if (objResultOutput.screeningresults?.status != null)
            {
                this.statuses.Add(
                    new status()
                    {
                        processstep = "Screening",
                        statuscode = objResultOutput.screeningresults?.status.screening
                    });
            }
        }

        public List<credit> credits { get; set; }
        public List<status> statuses { get; set; }
               
        // Output classes
        public class HireApplicantsRootobject
        {
            public NewApplicantOutput.ScreeningResultsContainer screeningresults { get; set; }
        }
    }
}