using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ScreeningTesterWebSite.Models
{
    public class GetApplicantStatusOutput : ModelBase
    {
        public GetApplicantStatusOutput()
        {
        }
        public GetApplicantStatusOutput(String xmlString)
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
                    else
                    {
                        if (applicantNode.Name == "error")
                        {
                            errorNode = applicantNode;
                        }
                    }
                }
            }
            if (errorNode == null && creditsNode == null && formsNode == null && statusNode == null)
            {
                this.ReturnedErrorText = "Returned XML not in expected format.";
                return;
            }

            if (applicantNode?.Attributes["externalid"] != null)
            {
                this.CandidateID = applicantNode.Attributes["externalid"].Value;
            }
            if (applicantNode?.Attributes["companyid"] != null)
            {
                this.CoID = applicantNode.Attributes["companyid"].Value;
            }
            if (applicantNode?.Attributes["id"] != null)
            {
                this.EmID = applicantNode.Attributes["id"].Value;
            }
            if (errorNode != null)
            {
                this.ReturnedErrorText = errorNode.ChildNodes[0].InnerText.Trim();
            }

            if (creditsNode != null)
            {
                for (int i = 0; i < creditsNode.ChildNodes.Count; i++)
                {
                    credits.Add(new ScreeningTesterWebSite.Models.credit()
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

        public GetApplicantStatusOutput(String responseString, String responseVer)
        {
            if (responseString.Contains("Error"))
            {
                // Should not be able to get here, but just in case it snuck by...
                this.ReturnedErrorText = "Error returned from Web Service";
            }
            else
            {
                this.statuses = new List<status>();
                this.credits = new List<credit>();
                this.forms = new List<NewApplicantOutput.PostAnswerForm>();

                GetApplicantStatusRootobject objResultOutput;
                if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                {
                    objResultOutput = JsonConvert.DeserializeObject<GetApplicantStatusRootobject>(responseString);
                }
                else
                {
                    objResultOutput =
                       (GetApplicantStatusRootobject)
                       UtilityFunctions.XmlDeserializeFromString(responseString, typeof(GetApplicantStatusRootobject));
                }

                if (objResultOutput.screeningresults?.credits != null)
                {
                    this.credits.Add(
                        new credit()
                        {
                            credittype = "Federal",
                            qualified = (objResultOutput.screeningresults.credits.federal.qualified == true ? "Yes" : "No")
                        });
                    this.credits.Add(
                        new credit()
                        {
                            credittype = "State",
                            qualified = (objResultOutput.screeningresults.credits.state.qualified == true ? "Yes" : "No")
                        });
                }
                if (objResultOutput.screeningresults?.forms != null)
                {
                    for (int i = 0; i < objResultOutput.screeningresults.forms.Count; i++)
                    {
                        this.forms.Add(
                            new NewApplicantOutput.PostAnswerForm()
                            {
                                formid = objResultOutput.screeningresults.forms[i].formid,
                                formname = objResultOutput.screeningresults.forms[i].formname
                            });
                    }
                }
                if (objResultOutput.screeningresults?.status != null)
                {
                    this.statuses.Add(
                        new status()
                        {
                            processstep = "Screening",
                            statuscode = objResultOutput.screeningresults.status.screening
                        });
                }
            }
        }

        public List<credit> credits { get; set; }
        public List<status> statuses { get; set; }
        public List<NewApplicantOutput.PostAnswerForm> forms { get; set; }

        // Output classes
        public class GetApplicantStatusRootobject
        {
            public NewApplicantOutput.ScreeningResultsContainer screeningresults { get; set; }
        }
    }
}