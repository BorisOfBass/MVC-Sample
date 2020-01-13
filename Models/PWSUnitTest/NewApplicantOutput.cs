using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ScreeningTesterWebSite.Models
{
    public class NewApplicantOutput : ModelBase
    {
        public NewApplicantOutput()
        {
        }
        public NewApplicantOutput(String xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            questions = new List<question>();
            credits = new List<credit>();
            statuses = new List<status>();

            this.ReturnedErrorText = null;

            XmlNode applicantNode = null;
            XmlNode questionsNode = null;
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
                            if (applicantNode.ChildNodes[i].Name == "questions")
                            {
                                questionsNode = applicantNode.ChildNodes[i];
                            }
                            else if (applicantNode.ChildNodes[i].Name == "credits")
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
            if (errorNode == null && questionsNode == null && creditsNode == null && formsNode == null && statusNode == null)
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

            if (questionsNode != null)
            {
                for (int i = 0; i < questionsNode.ChildNodes.Count; i++)
                {
                    question quest = new question()
                    {
                        order = questionsNode.ChildNodes[i].Attributes["order"].Value,
                        id = questionsNode.ChildNodes[i].Attributes["id"].Value,
                        answertype = questionsNode.ChildNodes[i].Attributes["answertype"].Value,
                        answersize = questionsNode.ChildNodes[i].Attributes["answersize"].Value
                    };
                    if (questionsNode.ChildNodes[i].HasChildNodes)
                    {
                        quest.text = questionsNode.ChildNodes[i].ChildNodes[0].Value;
                    }
                    else
                    {
                        quest.text = "UNABLE TO RETRIEVE QUESTION TEXT.";
                    }
                    quest.answervalue = "";
                    questions.Add(quest);
                }
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

        public NewApplicantOutput(String responseString, String methodName, IModelBase inputClass)
        {
            if (responseString.Contains("Error"))
            {
                // Should not be able to get here, but just in case it snuck by...
                this.ReturnedErrorText = "Error returned from Web Service";
            }
            else
            {
                this.TransferCommonProperties(inputClass);

                this.questions = new List<question>();
                this.statuses = new List<status>();
                this.credits = new List<credit>();
                this.forms = new List<PostAnswerForm>();
                // Get Response and convert to common type
                PostAnswerRootobject objCommonResultOutput = new PostAnswerRootobject();
                if (methodName == "PostAnswers")
                {
                    PostAnswerRootobject objResultOutputPA = new PostAnswerRootobject();
                    if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                    {
                        objResultOutputPA = JsonConvert.DeserializeObject<PostAnswerRootobject>(responseString);
                    }
                    else
                    {
                        objResultOutputPA =
                           (PostAnswerRootobject)
                           UtilityFunctions.XmlDeserializeFromString(responseString, typeof(PostAnswerRootobject));
                    }
                    objCommonResultOutput = objResultOutputPA;
                }
                else
                {
                    NewApplicantRootobject objResultOutputNA = new NewApplicantRootobject();
                    if (UtilityFunctions.GetWebAPIInputType().ToLower() == "json")
                    {
                        objResultOutputNA = JsonConvert.DeserializeObject<NewApplicantRootobject>(responseString);
                    }
                    else
                    {
                        objResultOutputNA =
                           (NewApplicantRootobject)
                           UtilityFunctions.XmlDeserializeFromString(responseString, typeof(NewApplicantRootobject));
                    }
                    this.EmID = objResultOutputNA.applicant.applicantid.ToString();
                    objCommonResultOutput = new PostAnswerRootobject()
                    {
                        applicant = new PostAnswerContainer()
                        {
                            questions = objResultOutputNA.applicant.questions,
                            screeningresults = objResultOutputNA.applicant.screeningresults
                        }
                    };
                }
                // Go through returned collections, filling this object
                if (objCommonResultOutput.applicant?.questions != null && objCommonResultOutput.applicant.questions.Count > 0)
                {
                    Int32 maxQuestions = objCommonResultOutput.applicant.questions.Count;
                    if (this.UserName == "PAFadvUser")
                    {
                        maxQuestions = 1;
                    }
                    for (int i = 0; i < maxQuestions; i++)
                    {
                        this.questions.Add(
                            GetQuestionFromResponse(objCommonResultOutput.applicant.questions[i]));
                    }
                }
                if (objCommonResultOutput.applicant?.screeningresults?.credits != null)
                {
                    if (objCommonResultOutput.applicant?.screeningresults.credits.federal != null)
                    {
                        this.credits.Add(
                            new credit()
                            {
                                credittype = "Federal",
                                qualified = (objCommonResultOutput.applicant?.screeningresults.credits.federal.qualified == true ? "Yes" : "No")
                            });
                    }
                    if (objCommonResultOutput.applicant?.screeningresults.credits.state != null)
                    {
                        this.credits.Add(
                            new credit()
                            {
                                credittype = "State",
                                qualified = (objCommonResultOutput.applicant?.screeningresults.credits.state.qualified == true ? "Yes" : "No")
                            });
                    }
                }
                if (objCommonResultOutput.applicant?.screeningresults?.forms != null)
                {
                    for (int i = 0; i < objCommonResultOutput.applicant.screeningresults.forms.Count; i++)
                    {
                        this.forms.Add(
                            new NewApplicantOutput.PostAnswerForm()
                            {
                                formid = objCommonResultOutput.applicant.screeningresults.forms[i].formid,
                                formname = objCommonResultOutput.applicant.screeningresults.forms[i].formname
                            });
                    }
                }
                if (objCommonResultOutput.applicant?.screeningresults?.status != null)
                {
                    this.statuses.Add(
                        new status()
                        {
                            processstep = "Screening",
                            statuscode = objCommonResultOutput.applicant.screeningresults.status.screening
                        });
                }
            }
        }

        public String MethodName { get; set; }
        public List<question> questions { get; set; }
        public List<credit> credits { get; set; }
        public List<status> statuses { get; set; }
        public List<PostAnswerForm> forms { get; set; }
        public String QuestionFlowDescription { get; set; }

        private question GetQuestionFromResponse(ReturnQuestion respQuest)
        {
            question retQuestion = new question()
            {
                id = respQuest.questionid.ToString(),
                answertype = respQuest.answertype,
                answersize = respQuest.answersize.ToString(),
                text = respQuest.questiontext,
                isrequired = respQuest.isrequired,
                canskip = respQuest.canskip,
                validationtext = respQuest.validationtext,
                previousanswer = respQuest.previousanswer,
            };
            retQuestion.validvaluelist = new List<string>();
            if (respQuest.validvaluelist != null)
            {
                for (int ctr = 0; ctr < respQuest.validvaluelist.Count; ctr++)
                {
                    retQuestion.validvaluelist.Add(respQuest.validvaluelist[ctr]);
                }
            }
            return retQuestion;
        }

        // Output classes
        #region NewApplicant
        [Serializable()]
        public class NewApplicantRootobject
        {
            [System.Xml.Serialization.XmlElementAttribute("applicant")]
            public NewApplicantContainer applicant { get; set; }
        }
        [Serializable()]
        public class NewApplicantContainer : PostAnswerContainer
        {
            [System.Xml.Serialization.XmlElementAttribute("applicantid")]
            public Int32 applicantid { get; set; }
        }
        [Serializable()]
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
        #endregion

        #region PostAnswers

        public class PostAnswerRootobject 
        {
            public PostAnswerContainer applicant { get; set; }
        }
        [Serializable()]
        public class PostAnswerContainer 
        {
            [System.Xml.Serialization.XmlArray("questions")]
            [System.Xml.Serialization.XmlArrayItem("question", typeof(ReturnQuestion))]
            public List<ReturnQuestion> questions { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("screeningresults")]
            public ScreeningResultsContainer screeningresults { get; set; }
        }
        #endregion

        #region Shared elements
        [Serializable()]
        public class ReturnQuestion
        {
            [System.Xml.Serialization.XmlElementAttribute("questionid")]
            public Int32 questionid { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("answertype")]
            public String answertype { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("answersize")]
            public Int32 answersize { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("questiontext")]
            public String questiontext { get; set; }
            [System.Xml.Serialization.XmlArray("validvaluelist")]
            [System.Xml.Serialization.XmlArrayItem("validvalue", typeof(String))]
            public List<String> validvaluelist { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("isrequired")]
            public Boolean isrequired { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("canskip")]
            public Boolean canskip { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("validationtext")]
            public String validationtext { get; set; }
            [System.Xml.Serialization.XmlElementAttribute("previousanswer")]
            public String previousanswer { get; set; }
        }

        public class ScreeningResultsContainer
        {
            public PostAnswerCreditList credits { get; set; }
            public List<PostAnswerForm> forms { get; set; }
            public PostAnswersStatus status { get; set; }
        }
        public class PostAnswerCreditList
        {
            public PostAnswerCredit federal { get; set; }
            public PostAnswerCredit state { get; set; }
        }
        public class PostAnswerCredit
        {
            public Boolean qualified { get; set; }
        }
        public class PostAnswerForm
        {
            public Int32 formid { get; set; }
            public String formname { get; set; }
        }
        public class PostAnswersStatus
        {
            public String screening { get; set; }
        }
        #endregion

    }

    public class question
    {
        public String order { get; set; }
        public String id { get; set; }
        public String answertype { get; set; }
        public String answersize { get; set; }
        public String text { get; set; }
        public String answervalue { get; set; }
        public List<String> validvaluelist { get; set; }
        public Boolean isrequired { get; set; }
        public Boolean canskip { get; set; }
        public String validationtext { get; set; }
        public String previousanswer { get; set; }
        public String questionrecording { get; set; }
        public String instructionrecording { get; set; }
    }
    public class credit
    {
        public String credittype { get; set; }
        public String qualified { get; set; }
    }
    public class status
    {
        public String processstep { get; set; }
        public String statuscode { get; set; }
    }

}