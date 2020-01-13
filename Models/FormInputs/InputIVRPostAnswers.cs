using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ScreeningTesterWebSite.Models
{
    public class InputIVRPostAnswers : InputBase
    {
        public InputIVRPostAnswers()
        { }

        public InputIVRPostAnswers(IVRNewApplicantOutput inIVRNewApplOut)
        {
            NewApplicantOutput inNewApplOut = new NewApplicantOutput();
            inNewApplOut.TransferCommonProperties(inIVRNewApplOut);
            inNewApplOut.questions = new List<question>();
            inNewApplOut.questions.Add(
                new question()
                {
                    answersize = inIVRNewApplOut.question.answersize.ToString(),
                    answertype = inIVRNewApplOut.question.answertype,
                    id = inIVRNewApplOut.question.questionid.ToString(),
                    text = inIVRNewApplOut.question.questiontext,
                    answervalue = inIVRNewApplOut.question.answervalue
                }
             );
            FillObject(inNewApplOut);
        }

        public InputIVRPostAnswers(NewApplicantOutput inNewApplOut)
        {
            FillObject(inNewApplOut);
        }

        private void FillObject(NewApplicantOutput inNewApplOut)
        {
            String answerValue;
            TransferCommonProperties(inNewApplOut);

            Int32 maxQuestions = inNewApplOut.questions.Count;
            if (inNewApplOut.UserName == "PAFadvUser")
            {
                maxQuestions = 1;
            }
            this.answers = new List<answer>();
            for (int i = 0; i < maxQuestions; i++)
            {
                answerValue = "";
                if (!String.IsNullOrEmpty(inNewApplOut.questions[i]?.answervalue))
                {
                    answerValue = inNewApplOut.questions[i].answervalue;
                }
                this.answers.Add(
                    new answer()
                    {
                        questionid = inNewApplOut.questions[i].id,
                        answervalue = answerValue
                    }
                );
            }
        }

        public List<answer> answers { get; set; }

        public InputIVRPostAnswers PrepareAnsweredQuestions()
        {
            InputIVRPostAnswers retObj = new InputIVRPostAnswers();
            retObj.LoID = this.LoID;
            retObj.EnvironCode = this.EnvironCode;
            retObj.LanguageID = this.LanguageID;
            retObj.CandidateID = this.CandidateID;
            retObj.EmID = this.EmID;
            retObj.CoID = this.CoID;
            retObj.UserName = this.UserName;
            retObj.UserPW = this.UserPW;

            if (this.answers != null)
            {
                retObj.answers = new List<answer>();
                foreach (answer ans in this.answers)
                {
                    if (ans.questionid != "-999")
                    {
                        retObj.answers.Add(ans);
                    }
                }
            }
            return retObj;
        }

        #region Override Methods
        public override String ProduceXMLStringFromObject()
        {
            StringBuilder sb = new StringBuilder(@"<postanswers languageid = ""en"" xmlns = ""http://www.itaxgroup.com/jobcredits/PartnerWebService/Questionnaire.xsd"">");
            sb.AppendLine(@"<answers applicantid=""" + this.EmID + @""">");
            if (this.answers != null)
            {
                foreach (answer ans in this.answers)
                {
                    sb.AppendLine(UtilityFunctions
                        .xmlAttributeAndValueString("answer", "questionid", ans.questionid, ans.answervalue));
                }
            }
            sb.AppendLine("</answers>");
            sb.AppendLine("</postanswers>");

            return sb.ToString();
        }
        public override void SetDefaults()
        {
            if (String.IsNullOrEmpty(this.EmID))
            {
                this.EmID = "94772523";
            }
            if (answers == null || answers.Count == 0)
            {
                answers = new List<answer>();
                for (int i = 0; i < 10; i++)
                {
                    answers.Add(new answer() { questionid = "-999", answervalue = "0" });
                }
            }
        }
        #endregion

        // Output classes
        [DataContract(Name = "RequestPostAnswers")]
        public class PostAnswersCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public PostAnswerApplicant applicant { get; set; }
            [DataMember(Name = "languageid")]
            public String languageid { get; set; }
        }

        [DataContract(Name = "RequestIVRPostAnswers")]
        public class PostAnswersIVRCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public PostAnswerIVRApplicant applicant { get; set; }
        }

        [DataContract]
        public class PostAnswerApplicant : BaseApplicant
        {
            [DataMember(Name = "answers")]
            public List<PostAnswersAnswer> answers { get; set; }
        }
        [DataContract]
        public class PostAnswerIVRApplicant
        {
            [DataMember(Name = "answers")]
            public List<PostAnswersAnswer> answers { get; set; }
            [DataMember(Name = "externalid")]
            public String externalid { get; set; }
        }
        [DataContract]
        public class PostAnswersAnswer
        {
            [DataMember(Name = "questionid")]
            public Int32 questionid { get; set; }
            [DataMember(Name = "value")]
            public String value { get; set; }
        }

        public PostAnswersCompactObject GetSerializableObject()
        {
            PostAnswersCompactObject retVal = new PostAnswersCompactObject()
            {
                languageid = this.LanguageID
            };
            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };

            retVal.applicant = new PostAnswerApplicant();
            retVal.applicant.answers = new List<PostAnswersAnswer>();
            for (int i = 0; i < this.answers.Count; i++)
            {
                retVal.applicant.answers.Add(
                    new PostAnswersAnswer()
                    {
                        questionid = Convert.ToInt32(this.answers[i].questionid),
                        value = this.answers[i].answervalue
                    });
            }
            retVal.applicant.applicantid = Convert.ToInt32(this.EmID);
            retVal.applicant.companyid = Convert.ToInt32(this.CoID);
            retVal.applicant.externalid = this.CandidateID;

            return retVal;
        }

        public PostAnswersIVRCompactObject GetSerializableObject_IVR()
        {
            PostAnswersCompactObject rootClass = GetSerializableObject();

            PostAnswersIVRCompactObject retObj = new PostAnswersIVRCompactObject()
            {
                authentication = new authenticationObj()
                {
                    username = this.UserName,
                    password = this.UserPW
                },
                applicant = new PostAnswerIVRApplicant()
                {
                    externalid = this.CandidateID,
                    answers = new List<PostAnswersAnswer>()
                }
            };
            for (int i = 0; i < rootClass.applicant?.answers?.Count; i++)
            {
                retObj.applicant.answers.Add(rootClass.applicant.answers[i]);
            }

            return retObj;
        }
    }
}