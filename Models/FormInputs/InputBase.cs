using System;
using System.Runtime.Serialization;

namespace ScreeningTesterWebSite.Models
{
    public class InputBase : ModelBase
    {
        // Shared Output classes
        [DataContract]
        public class authenticationObj
        {
            [DataMember(Name = "username")]
            public String username { get; set; }
            [DataMember(Name = "password")]
            public String password { get; set; }
        }
        [DataContract]
        public class BaseApplicant
        {
            [DataMember(Name = "applicantid")]
            public Int32 applicantid { get; set; }
            [DataMember(Name = "companyid")]
            public int companyid { get; set; }
            [DataMember(Name = "externalid")]
            public String externalid { get; set; }
            [DataMember(Name = "ssn")]
            public String ssn { get; set; }
        }
        public class answer
        {
            public String questionid { get; set; }
            public String answervalue { get; set; }
        }
    }
    
}