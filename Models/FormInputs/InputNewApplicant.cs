using System;
using System.Runtime.Serialization;
using System.Text;

namespace ScreeningTesterWebSite.Models
{
    public class InputNewApplicant : ModelBase
    {
        public InputNewApplicant()
        {
        }
        public InputNewApplicant(AddressConfirmationOutput intQuestOutput)
        {
            TransferCommonProperties(intQuestOutput);

            this.ssn = intQuestOutput.ssn;
            this.fname = intQuestOutput.fname;
            this.mi = intQuestOutput.mi;
            this.lname = intQuestOutput.lname;
            this.birthdate = intQuestOutput.birthdate;

            this.address = intQuestOutput.address;
            this.city = intQuestOutput.city;
            this.state = intQuestOutput.state;
            this.zip = intQuestOutput.zip;

            this.hourlyrate = intQuestOutput.hourlyrate;
            this.position = intQuestOutput.position;
            this.optout = intQuestOutput.optout;
            this.primaryphone = intQuestOutput.primaryphone;
            this.email = intQuestOutput.email;
        }

        public String ResultsTargetPage { get; set; }
        public String ResultsTargetController { get; set; }
        public String MethodName { get; set; }

        public String ssn { get; set; }
        public String fname { get; set; }
        public String mi { get; set; }
        public String lname { get; set; }
        public String birthdate { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zip { get; set; }
        public String hourlyrate { get; set; }
        public String position { get; set; }
        public String optout { get; set; }
        public String primaryphone { get; set; }
        public String email { get; set; }
             
        public void PrepareForInput()
        {
            this.hourlyrate = "0";
            this.position = "Sod Installation Technician";
            if (String.IsNullOrEmpty(this.optout))
            {
                this.optout = "0";
            }
            this.primaryphone = "612-555-2756";
            this.email = "fake1@notreal.com";
        }

        #region Override Methods
        public override String ProduceXMLStringFromObject()
        {
            String candID = (this.CandidateID == null ? "" : this.CandidateID);

            StringBuilder sb;
            if (true)
            {
                sb = new StringBuilder(@"<" + this.MethodName.ToLower() + @" languageid = ""en"" xmlns = ""http://www.itaxgroup.com/jobcredits/PartnerWebService/Questionnaire.xsd"">");
                sb.AppendLine(@"<applicant companyid=""" + this.CoID + @""" locationid=""" + this.LoID
                    + @""" externalid=""" + candID + @""">");

                sb.AppendLine(UtilityFunctions.xmlElementString("ssn", this.ssn));
                sb.AppendLine(UtilityFunctions.xmlElementString("fname", this.fname));
                sb.AppendLine(UtilityFunctions.xmlElementString("mi", this.mi));
                sb.AppendLine(UtilityFunctions.xmlElementString("lname", this.lname));
                sb.AppendLine(UtilityFunctions.xmlElementString("birthdate", this.birthdate));
                sb.AppendLine(UtilityFunctions.xmlElementString("address", this.address));
                sb.AppendLine(UtilityFunctions.xmlElementString("city", this.city));
                sb.AppendLine(UtilityFunctions.xmlElementString("state", this.state));
                sb.AppendLine(UtilityFunctions.xmlElementString("zip", this.zip));
                sb.AppendLine(UtilityFunctions.xmlElementString("hourlyrate", this.hourlyrate));
                sb.AppendLine(UtilityFunctions.xmlElementString("position", this.position));
                sb.AppendLine(UtilityFunctions.xmlElementString("optout", this.optout));
                sb.AppendLine(UtilityFunctions.xmlElementString("primaryphone", this.primaryphone));
                sb.AppendLine(UtilityFunctions.xmlElementString("email", this.email));
                sb.AppendLine("</applicant>");
                sb.AppendLine("</" + this.MethodName.ToLower() + ">");
            }
            else // This matches the new input for InitializeQuestionnaire
            {
                sb = new StringBuilder();
                //sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF - 8""?>");
                //sb.AppendLine("<" + this.MethodName + ">");
                sb.AppendLine(@"<" + this.MethodName + @" languageid = ""en"" xmlns = ""http://www.itaxgroup.com/jobcredits/PartnerWebService/Questionnaire.xsd"">");
                /*sb.AppendLine(" <authentication>");
                sb.AppendLine("  <username>" + this.UserName + "</username>");
                sb.AppendLine("  <password>" + this.UserPW + "</password>");
                sb.AppendLine(" </authentication>");*/
                sb.AppendLine(" <applicant>");
                sb.AppendLine(UtilityFunctions.xmlElementString("address", this.address));
                sb.AppendLine(UtilityFunctions.xmlElementString("city", this.city));
                sb.AppendLine(UtilityFunctions.xmlElementString("state", this.state));
                sb.AppendLine(UtilityFunctions.xmlElementString("zip", this.zip));
                sb.AppendLine(UtilityFunctions.xmlElementString("companyid", this.CoID));
                sb.AppendLine(" </applicant>");
                sb.AppendLine("</" + this.MethodName + ">");
            }
            return sb.ToString();
        }

        public override void SetDefaults()
        {
            // Default Values
            if (string.IsNullOrEmpty(this.fname))
            {
                this.fname = "Roberto";
            }
            if (string.IsNullOrEmpty(this.lname))
            {
                this.lname = "Chavez";
            }
            if (string.IsNullOrEmpty(this.address))
            {
                this.address = "10218 Summerview Cir";
            }
            if (string.IsNullOrEmpty(this.city))
            {
                this.city = "Riverview";
            }
            if (string.IsNullOrEmpty(this.state))
            {
                this.state = "FL";
            }
            if (string.IsNullOrEmpty(this.zip))
            {
                this.zip = "33578";
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
        public class InitAppApplicantObj
        {
            [DataMember(Name = "address")]
            public String address { get; set; }
            [DataMember(Name = "city")]
            public String city { get; set; }
            [DataMember(Name = "state")]
            public String state { get; set; }
            [DataMember(Name = "zip")]
            public String zip { get; set; }
            [DataMember(Name = "companyid")]
            public String companyid { get; set; }
        }

        [DataContract(Name = "RequestNewApplicant")]
        public class InitializeQuestionnaireCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public InitAppApplicantObj applicant { get; set; }
            [DataMember(Name = "languageid")]
            public String languageid { get; set; }
        }

        public InitializeQuestionnaireCompactObject GetSerializableObject_InitializeQuestionnaire()
        {
            InitializeQuestionnaireCompactObject retVal = new InitializeQuestionnaireCompactObject()
            {
                languageid = this.LanguageID
            };
            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };
            retVal.applicant = new InitAppApplicantObj()
            {
                address = this.address,
                city = this.city,
                state = this.state,
                zip = this.zip,
                companyid = this.CoID.ToString()
            };
            return retVal;
        }

        [DataContract]
        public class NewAppApplicantObj : InputBase.BaseApplicant
        {
            [DataMember(Name = "fname")]
            public String fname { get; set; }
            [DataMember(Name = "mi")]
            public String mi { get; set; }
            [DataMember(Name = "lname")]
            public String lname { get; set; }
            [DataMember(Name = "birthdate")]
            public String birthdate { get; set; }
            [DataMember(Name = "address")]
            public String address { get; set; }
            [DataMember(Name = "city")]
            public String city { get; set; }
            [DataMember(Name = "state")]
            public String state { get; set; }
            [DataMember(Name = "zip")]
            public String zip { get; set; }
            [DataMember(Name = "hourlyrate")]
            public Int32 hourlyrate { get; set; }
            [DataMember(Name = "position")]
            public String position { get; set; }
            [DataMember(Name = "optout")]
            public Int32 optout { get; set; }
            [DataMember(Name = "primaryphone")]
            public String primaryphone { get; set; }
            [DataMember(Name = "email")]
            public String email { get; set; }
            [DataMember(Name = "locationid")]
            public String locationid { get; set; }
        }

        [DataContract(Name = "NewApplicantQuestionnaire")]
        public class NewApplicantCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public NewAppApplicantObj applicant { get; set; }
            [DataMember(Name = "languageid")]
            public String languageid { get; set; }
        }
        public NewApplicantCompactObject GetSerializableObject_NewApplicant()
        {
            NewApplicantCompactObject retVal = new NewApplicantCompactObject()
            {
                languageid = this.LanguageID
            };
            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };
            retVal.applicant = new NewAppApplicantObj()
            {
                fname = this.fname,
                mi = this.mi,
                lname = this.lname,
                birthdate = this.birthdate,
                address = this.address,
                city = this.city,
                state = this.state,
                zip = this.zip,
                hourlyrate = Convert.ToInt32(this.hourlyrate),
                position = this.position,
                optout = Convert.ToInt32(this.optout),
                primaryphone = this.primaryphone,
                email = this.email,
                locationid = this.LoID,
                companyid = Convert.ToInt32(this.CoID),
                externalid = this.CandidateID,
                applicantid = Convert.ToInt32(this.EmID),
                ssn = this.ssn
            };
            return retVal;
        }
    }
}