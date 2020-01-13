using System;
using System.Runtime.Serialization;
using System.Text;

namespace ScreeningTesterWebSite.Models
{
    public class InputHireApplicants : ModelBase
    {
        public String ssn { get; set; }
        public String fname { get; set; }
        public String mi { get; set; }
        public String lname { get; set; }
        public String birthdate { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public String zip { get; set; }
        public String offerdate { get; set; }
        public String hiredate { get; set; }
        public String startdate { get; set; }
        public String termdate { get; set; }
        public String hourlyrate { get; set; }
        public String position { get; set; }

        public void PrepareForInput()
        {
            this.birthdate = "";
            this.hourlyrate = "0";
            this.position = "Merchandiser";
            this.offerdate = "";
        }

        #region Override Methods
        public override String ProduceXMLStringFromObject()
        {
            String emID = (this.EmID == null ? "" : this.EmID);
            String caID = (this.CandidateID == null ? "" : this.CandidateID);
            StringBuilder sb = new StringBuilder(@"<hireapplicants xmlns=""http://www.itaxgroup.com/jobcredits/PartnerWebService/HireApplicants.xsd"">");
            sb.AppendLine(@"<applicant externalid=""" + caID + @""" companyid=""" + this.CoID + @""" locationid=""" + this.LoID + @""" id=""" + emID + @""">");
            sb.AppendLine(UtilityFunctions.xmlElementString("ssn", this.ssn));
            sb.AppendLine(UtilityFunctions.xmlElementString("fname", this.fname));
            sb.AppendLine(UtilityFunctions.xmlElementString("mi", this.mi));
            sb.AppendLine(UtilityFunctions.xmlElementString("lname", this.lname));
            sb.AppendLine(UtilityFunctions.xmlElementString("birthdate", this.birthdate));
            sb.AppendLine(UtilityFunctions.xmlElementString("address", this.address));
            sb.AppendLine(UtilityFunctions.xmlElementString("city", this.city));
            sb.AppendLine(UtilityFunctions.xmlElementString("state", this.state));
            sb.AppendLine(UtilityFunctions.xmlElementString("zip", this.zip));
            sb.AppendLine(UtilityFunctions.xmlElementString("offerdate", this.offerdate));
            sb.AppendLine(UtilityFunctions.xmlElementString("hiredate", this.hiredate));
            sb.AppendLine(UtilityFunctions.xmlElementString("startdate", this.startdate));
            sb.AppendLine(UtilityFunctions.xmlElementString("hourlyrate", this.hourlyrate));
            sb.AppendLine(UtilityFunctions.xmlElementString("position", this.position));
            sb.AppendLine("</applicant>");
            sb.AppendLine("</hireapplicants>");

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
            if (string.IsNullOrEmpty(this.ssn))
            {
                this.ssn = "589093331";
            }
            if (string.IsNullOrEmpty(this.address))
            {
                this.address = "18125 6th Ave";
            }
            if (string.IsNullOrEmpty(this.city))
            {
                this.city = "Three Rivers";
            }
            if (string.IsNullOrEmpty(this.state))
            {
                this.state = "MI";
            }
            if (string.IsNullOrEmpty(this.zip))
            {
                this.zip = "49093";
            }
            if (string.IsNullOrEmpty(this.hiredate))
            {
                this.hiredate = "2017-05-07";
            }
            if (string.IsNullOrEmpty(this.startdate))
            {
                this.startdate = "2017-05-07";
            }
        }
        #endregion

        #region Output Classes / Calls
        public HireApplicantsCompactObject GetSerializableObject()
        {
            HireApplicantsCompactObject retVal = new HireApplicantsCompactObject();
            retVal.authentication = new authenticationObj()
            {
                username = this.UserName,
                password = this.UserPW
            };
            retVal.applicant = new applicantObj()
            {
                ssn = this.ssn,
                fname = this.fname,
                mi = this.mi,
                lname = this.lname,
                birthdate = this.birthdate,
                address = this.address,
                city = this.city,
                state = this.state,
                zip = this.zip,
                offerdate = StringToNullableDate(this.offerdate),
                hiredate = StringToNullableDate(this.hiredate),
                startdate = StringToNullableDate(this.startdate),
                termdate = StringToNullableDate(this.termdate),
                hourlyrate = (String.IsNullOrEmpty(this.hourlyrate) ? 0 : Convert.ToInt32(this.hourlyrate)),
                position = this.position,
                companyid = Convert.ToInt32(this.CoID),
                locationid = this.LoID,
                externalid = this.CandidateID,
                applicantid = this.EmID
            };
            return retVal;
        }
        
        [DataContract(Name = "RequestHireApplicants")]
        public class HireApplicantsCompactObject
        {
            [DataMember(Name = "authentication")]
            public authenticationObj authentication { get; set; }
            [DataMember(Name = "applicant")]
            public applicantObj applicant { get; set; }
        }
        [DataContract]
        public class authenticationObj
        {
            [DataMember(Name = "username")]
            public String username { get; set; }
            [DataMember(Name = "password")]
            public String password { get; set; }
        }
        public class applicantObj
        {
            [DataMember(Name = "ssn")]
            public String ssn { get; set; }
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
            [DataMember(Name = "offerdate")]
            public DateTime? offerdate { get; set; }
            [DataMember(Name = "hiredate")]
            public DateTime? hiredate { get; set; }
            [DataMember(Name = "startdate")]
            public DateTime? startdate { get; set; }
            [DataMember(Name = "termdate")]
            public DateTime? termdate { get; set; }
            [DataMember(Name = "hourlyrate")]
            public Int32 hourlyrate { get; set; }
            [DataMember(Name = "position")]
            public String position { get; set; }
            [DataMember(Name = "companyid")]
            public Int32 companyid { get; set; }
            [DataMember(Name = "locationid")]
            public String locationid { get; set; }
            [DataMember(Name = "externalid")]
            public String externalid { get; set; }
            [DataMember(Name = "applicantid")]
            public String applicantid { get; set; }
        }
        #endregion
        private DateTime? StringToNullableDate(String dateText)
        {
            if (String.IsNullOrEmpty(dateText))
            {
                return null;
            }
            else
            {
                DateTime nonNull = Convert.ToDateTime(dateText);
                return nonNull as DateTime?;
            }   
        }
    }
}