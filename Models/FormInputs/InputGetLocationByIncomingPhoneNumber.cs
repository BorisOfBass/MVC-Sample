using System;
using System.Runtime.Serialization;

namespace ScreeningTesterWebSite.Models
{
    public class InputGetLocationByIncomingPhoneNumber : ModelBase
    {
        public InputGetLocationByIncomingPhoneNumber()
        {
        }

        public String phonenumber { get; set; }

        #region Override Methods
        //public override String ProduceXMLStringFromObject()
        //{
        //    String candID = (this.CandidateID == null ? "" : this.CandidateID);

        //    StringBuilder sb;
        //    if (true)
        //    {
        //        sb = new StringBuilder(@"<" + this.MethodName.ToLower() + @" languageid = ""en"" xmlns = ""http://www.itaxgroup.com/jobcredits/PartnerWebService/Questionnaire.xsd"">");
        //        sb.AppendLine(@"<applicant companyid=""" + this.CoID + @""" locationid=""" + this.LoID
        //            + @""" externalid=""" + candID + @""">");

        //        sb.AppendLine(UtilityFunctions.xmlElementString("ssn", this.ssn));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("fname", this.fname));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("mi", this.mi));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("lname", this.lname));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("birthdate", this.birthdate));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("address", this.address));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("city", this.city));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("state", this.state));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("zip", this.zip));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("hourlyrate", this.hourlyrate));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("position", this.position));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("optout", this.optout));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("primaryphone", this.primaryphone));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("email", this.email));
        //        sb.AppendLine("</applicant>");
        //        sb.AppendLine("</" + this.MethodName.ToLower() + ">");
        //    }
        //    else // This matches the new input for InitializeQuestionnaire
        //    {
        //        sb = new StringBuilder();
        //        //sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF - 8""?>");
        //        //sb.AppendLine("<" + this.MethodName + ">");
        //        sb.AppendLine(@"<" + this.MethodName + @" languageid = ""en"" xmlns = ""http://www.itaxgroup.com/jobcredits/PartnerWebService/Questionnaire.xsd"">");
        //        /*sb.AppendLine(" <authentication>");
        //        sb.AppendLine("  <username>" + this.UserName + "</username>");
        //        sb.AppendLine("  <password>" + this.UserPW + "</password>");
        //        sb.AppendLine(" </authentication>");*/
        //        sb.AppendLine(" <applicant>");
        //        sb.AppendLine(UtilityFunctions.xmlElementString("address", this.address));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("city", this.city));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("state", this.state));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("zip", this.zip));
        //        sb.AppendLine(UtilityFunctions.xmlElementString("companyid", this.CoID));
        //        sb.AppendLine(" </applicant>");
        //        sb.AppendLine("</" + this.MethodName + ">");
        //    }
        //    return sb.ToString();
        //}

        public override void SetDefaults()
        {
            if (string.IsNullOrEmpty(this.phonenumber))
            {
                this.phonenumber = "8999898988";  
            }
            if (string.IsNullOrEmpty(this.CandidateID))
            {
                DateTime currDat = DateTime.Now;
                Char char0 = Convert.ToChar("0");
                if (false)
                {
                    this.CandidateID = currDat.Year.ToString()
                        + currDat.Month.ToString().PadLeft(2, char0)
                        + currDat.Day.ToString().PadLeft(2, char0)
                        + currDat.Hour.ToString().PadLeft(2, char0)
                        + currDat.Minute.ToString().PadLeft(2, char0);
                }
                else
                {
                    this.CandidateID = Guid.NewGuid().ToString();
                }
            }
        }
        #endregion

        [DataContract(Name = "GetLocationByIncomingPhoneNumber")]
        public class GetLocationByIncomingPhoneNumberCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public IVRInitAppApplicantObj applicant { get; set; }
        }

        [DataContract]
        public class authenticationObj
        {
            [DataMember(Name = "username")]
            public String username { get; set; }
            [DataMember(Name = "password")]
            public String password { get; set; }
        }

        [DataContract]
        public class IVRInitAppApplicantObj
        {
            [DataMember(Name = "phonenumber")]
            public String phonenumber { get; set; }
            [DataMember(Name = "languageid")]
            public String languageid { get; set; }
            [DataMember(Name = "externalid")]
            public String externalid { get; set; }

        }

        public GetLocationByIncomingPhoneNumberCompactObject GetSerializableObject()
        {
            GetLocationByIncomingPhoneNumberCompactObject retVal 
                = new GetLocationByIncomingPhoneNumberCompactObject();

            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };

            retVal.applicant = new IVRInitAppApplicantObj()
            {
                phonenumber = this.phonenumber,
                languageid = this.LanguageID,
                externalid = this.CandidateID
            };
            return retVal;
        }
    }
}