using System;

namespace ScreeningTesterWebSite.Models
{
    public class AddressConfirmationOutput : InitializeQuestionnaireOutput
    {
        public AddressConfirmationOutput()
        {
        }
        public AddressConfirmationOutput (InitializeQuestionnaireOutput baseObj, InputNewApplicant newApplicantIn)
        {
            TransferCommonProperties(newApplicantIn);
            PassThroughBaseProps(baseObj);
            PassThroughContactInfo(newApplicantIn);
        }

        public String ssn { get; set; }
        public String fname { get; set; }
        public String mi { get; set; }
        public String lname { get; set; }
        public String birthdate { get; set; }
        public String hourlyrate { get; set; }
        public String position { get; set; }
        public String optout { get; set; }
        public String primaryphone { get; set; }
        public String email { get; set; }

        protected void PassThroughBaseProps(InitializeQuestionnaireOutput baseObj)
        {
            this.optouttext = baseObj.optouttext;
            this.address = baseObj?.address ?? "";
            this.city = baseObj?.city ?? "";
            this.state = baseObj?.state ?? "";
            this.zip = baseObj?.zip ?? "";
            this.ReturnedErrorText = baseObj.ReturnedErrorText;
        }

        protected void PassThroughContactInfo(InputNewApplicant newApplicantIn)
        {
            this.ssn = newApplicantIn.ssn;
            this.fname = newApplicantIn.fname;
            this.mi = newApplicantIn.mi;
            this.lname = newApplicantIn.lname;
            this.birthdate = newApplicantIn.birthdate;
            this.hourlyrate = newApplicantIn.hourlyrate;
            this.position = newApplicantIn.position;
            this.optout = newApplicantIn.optout;
            this.primaryphone = newApplicantIn.primaryphone;
            this.email = newApplicantIn.email;
        }
    }
}