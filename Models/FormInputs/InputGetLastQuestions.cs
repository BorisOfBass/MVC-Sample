using System;
using System.Runtime.Serialization;

namespace ScreeningTesterWebSite.Models
{
    public class InputGetLastQuestions : InputBase
    {
        public String MethodName { get; set; }

        public InputGetLastQuestions()
        {
        }
        public InputGetLastQuestions(NewApplicantOutput inNewApplOut)
        {
            TransferCommonProperties(inNewApplOut);
        }
        #region Override Methods
        public override String ProduceXMLStringFromObject()
        {
            throw new NotImplementedException();
        }
        public override void SetDefaults()
        {
            if (String.IsNullOrEmpty(this.EmID))
            {
                this.EmID = "94772523";
            }
        }
        #endregion

        // Output classes
        [DataContract(Name = "RequestGetLastQuestions")]
        public class GetLastQuestionsCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public BaseApplicant applicant { get; set; }
            [DataMember(Name = "languageid")]
            public String languageid { get; set; }
        }

        public GetLastQuestionsCompactObject GetSerializableObject()
        {
            GetLastQuestionsCompactObject retVal = new GetLastQuestionsCompactObject()
            {
                languageid = this.LanguageID
            };
            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };
            retVal.applicant = new BaseApplicant()
            {
                applicantid = Convert.ToInt32(this.EmID),
                companyid = Convert.ToInt32(this.CoID),
                externalid = this.CandidateID
            };
            return retVal;
        }
    }
}