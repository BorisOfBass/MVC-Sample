using System;
using System.Runtime.Serialization;
using System.Text;

namespace ScreeningTesterWebSite.Models
{
    public class InputIVRNewApplicant : ModelBase
    {
        public InputIVRNewApplicant()
        {
        }
        public InputIVRNewApplicant(InputGetLocationByIncomingPhoneNumber inputGetLocation)
        {
            TransferCommonProperties(inputGetLocation);
            SetDefaults();
        }

        public InputIVRNewApplicant(InputGetCompanyByCode inputGetLocation)
        {
            TransferCommonProperties(inputGetLocation);
            SetDefaults();
        }

        public String ssn { get; set; }
        public String birthdate { get; set; }
        public String zip { get; set; }
        public Int32 under40 { get; set; }
        // Display only, not part of input
        public String action { get; set; }
        public String redirecturl { get; set; }
        public String recordingfile { get; set; }

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
            // Default Values
            Random randObj = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
            if (string.IsNullOrEmpty(this.ssn))
            {
                int ssnInt = randObj.Next(112233445, 998998998);
                this.ssn = ssnInt.ToString();
            }
            if (String.IsNullOrEmpty(this.birthdate))
            {
                this.under40 = 1;
                this.birthdate = DateTime.Today.AddDays(-50 * randObj.Next(152, 285)).ToString("d");
            }
            if (string.IsNullOrEmpty(this.zip))
            {
                this.zip = randObj.Next(12450, 90123).ToString();
            }
        }
        #endregion


        [DataContract]
        public class authenticationObj
        {
            [DataMember(Name = "username")]
            public String username { get; set; }
            [DataMember(Name = "password")]
            public String password { get; set; }
        }

        [DataContract]
        public class IVRNewAppApplicantObj 
        {
            [DataMember(Name = "externalid")]
            public String externalid { get; set; }
            [DataMember(Name = "birthdate")]
            public String birthdate { get; set; }
            [DataMember(Name = "zip")]
            public String zip { get; set; }
            [DataMember(Name = "ssn")]
            public String ssn { get; set; }
            [DataMember(Name = "under40")]
            public Boolean under40 { get; set; }
        }

        [DataContract(Name = "NewApplicantQuestionnaire")]
        public class NewApplicantCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public IVRNewAppApplicantObj applicant { get; set; }
        }
        public NewApplicantCompactObject GetSerializableObject_NewApplicant()
        {
            NewApplicantCompactObject retVal 
                = new NewApplicantCompactObject();

            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };

            retVal.applicant = new IVRNewAppApplicantObj()
            {
                birthdate = this.birthdate,
                zip = this.zip,
                externalid = this.CandidateID,
                ssn = this.ssn,
                under40 = (this.under40 == 1 ? true : false)
            };
            return retVal;
        }
    }
}