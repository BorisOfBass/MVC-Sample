using System;
using System.Runtime.Serialization;

namespace ScreeningTesterWebSite.Models
{
    public class  InputGetCompanyByCode  : InputGetLocationByIncomingPhoneNumber 
    {
        public InputGetCompanyByCode()
        {
        }

        public String overridecode { get; set; }

        #region Override Methods
        public override void SetDefaults()
        {
            this.CandidateID = Guid.NewGuid().ToString();
            this.phonenumber = "placeholder";
            this.overridecode = "1110002222";
        }
        #endregion

        [DataContract(Name = "GetCompanyByCode")]
        public class GetCompanyByCodeCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public IVRGetCompanyByCodeObj applicant { get; set; }
        }


        [DataContract]
        public class IVRGetCompanyByCodeObj : IVRInitAppApplicantObj
        {
            [DataMember(Name = "overridecode")]
            public String overridecode { get; set; }
        }

        public GetCompanyByCodeCompactObject GetSerializableObject_()
        {
            GetCompanyByCodeCompactObject retVal 
                = new GetCompanyByCodeCompactObject();

            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };

            retVal.applicant = new IVRGetCompanyByCodeObj()
            {
                phonenumber = this.phonenumber,
                languageid = this.LanguageID,
                externalid = this.CandidateID,
                overridecode = this.overridecode
            };
            return retVal;
        }
    }
}