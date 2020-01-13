using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ScreeningTesterWebSite.Models
{
    public interface IModelBase
    {
        String CoID {get; set;}
        String LoID { get; set; }
        String EnvironCode { get; set; }
        String CandidateID { get; set; }
        String EmID { get; set; }
        String UserName { get; set; }
        String UserPW { get; set; }
        String LanguageID { get; set; }

        void TransferCommonProperties(IModelBase sourceObj);
    }
    public class ModelBase : IModelBase
    {
        #region IModelBase Implementation
        public String CoID { get; set; }
        public String LoID { get; set; }
        public String EnvironCode { get; set; }

        public String CandidateID { get; set; }
        public String EmID { get; set; }

        private String _UserName { get; set; }
        public String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _EncryptedUserName = StringUtil.Encrypt(value);
                _UserName = value;
            }
        }

        private String _UserPW;
        public String UserPW
        {
            get
            {
                return _UserPW;
            }
            set
            {
                _EncryptedUserPW = StringUtil.Encrypt(value);
                _UserPW = value;
            }
        }

        public String LanguageID { get; set; }

        public void TransferCommonProperties(IModelBase sourceObj)
        {
            this.CoID = (String.IsNullOrEmpty(this.CoID) ? sourceObj.CoID : this.CoID);
            this.LoID = (String.IsNullOrEmpty(this.LoID) ? sourceObj.LoID : this.LoID);
            this.EnvironCode = (String.IsNullOrEmpty(this.EnvironCode) ? sourceObj.EnvironCode : this.EnvironCode);
            this.CandidateID = (String.IsNullOrEmpty(this.CandidateID) ? sourceObj.CandidateID : this.CandidateID);
            if (String.IsNullOrEmpty(this.EmID) || this.EmID == "0")
            {
                this.EmID = sourceObj.EmID;
            }
            this.UserName = (String.IsNullOrEmpty(this.UserName) ? sourceObj.UserName : this.UserName);
            this.UserPW = (String.IsNullOrEmpty(this.UserPW) ? sourceObj.UserPW : this.UserPW);
            this.LanguageID = (String.IsNullOrEmpty(this.LanguageID) ? sourceObj.LanguageID : this.LanguageID);
        }
        #endregion

        public String _EncryptedUserName;
        public String EncryptedUserName
        {
            get
            {
                return _EncryptedUserName;
            }
        }

        public String _EncryptedUserPW;
        public String EncryptedUserPW
        {
            get
            {
                return _EncryptedUserPW;
            }
        }
        public String ReturnedErrorText { get; set; }

        public String ToXml
        {
            get
            {
                return ProduceXMLStringFromObject();
            }
        }

        #region Methods to Override
        public virtual String ProduceXMLStringFromObject()
        {
            return "";
        }
        public virtual void SetDefaults()
        {
        }
        #endregion

        #region Business related constants
        public Boolean DebugBuildModeFlag
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public String GetDefaultLanguageID()
        {
            return "en"; // "es";  For development testing of Spanish
        }

        public List<SelectListItem> EnvironmentOptions
        {
            get
            {
                List<SelectListItem> retObj = new List<SelectListItem>();
#if DEBUG
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("PD"), Value = "PD" });
#endif
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("PQ"), Value = "PQ" });
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("PU"), Value = "PU" });
#if DEBUG
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("PP"), Value = "PP" });
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("AD"), Value = "AD" });
#endif
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("AQ"), Value = "AQ" });
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("AU"), Value = "AU" });
#if DEBUG     
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("AP"), Value = "AP" });
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("ID"), Value = "ID" });
#endif
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("IQ"), Value = "IQ" });
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("IU"), Value = "IU" });
#if DEBUG
                retObj.Add(new SelectListItem { Text = GetEnvironmentDescription("IP"), Value = "IP" });
#endif

                return retObj;
            }
        }
        public String EnvironmentDescription
        {
            get
            {
                return GetEnvironmentDescription(this.EnvironCode);
            }
        }
        public String GetEnvironmentDescription(String environCode)
        {
            if (environCode == "PD")
            {
                return "PWS Development (local)";
            }
            else if(environCode == "PQ")
            {
                return "PWS QA";
            }
            else if (environCode == "PU")
            {
                return "PWS UAT";
            }
            else if(environCode == "PP")
            {
                return "PWS Production";
            }
            else if (environCode == "AD")
            {
                return "Web API Development (local)";
            }
            else if (environCode == "AQ")
            {
                return "Web API QA";
            }
            else if (environCode == "AU")
            {
                return "Web API UAT";
            }
            else if (environCode == "AP")
            {
                return "Web API Production";
            }
            else if (environCode == "ID")
            {
                return "IVR Web API Development (local)";
            }
            else if (environCode == "IQ")
            {
                return "IVR Web API QA";
            }
            else if (environCode == "IU")
            {
                return "IVR Web API UAT";
            }
            else if (environCode == "IP")
            {
                return "IVR Web API Production";
            }
            else
            {
                return "PWS UAT";
            }
        }

        public List<SelectListItem> LaunguageIDOptions
        {
            get
            {
                List<SelectListItem> retObj = new List<SelectListItem>();
                retObj.Add(new SelectListItem { Text = GetLanguageDescription("EN"), Value = "EN" });
                retObj.Add(new SelectListItem { Text = GetLanguageDescription("ES"), Value = "ES" });

                return retObj;
            }
        }

        public String LanguageDescription
        {
            get
            {
                return GetLanguageDescription(this.LanguageID);
            }
        }

        public String GetLanguageDescription(String langID)
        {
            if (langID == "ES")
            {
                return "Spanish";
            }
            else
            {
                return "English";
            }
        }

        public List<String> QuestionNotes
        {
            get
            {
                return GetNotes(false, false);
            }
        }
        public List<String> PostAnswersNotes
        {
            get
            {
                return GetNotes(true, false);
            }
        }
        private List<String> _IVRQuestionNotes;
        public List<String> IVRQuestionNotes
        {
            get
            {
                if (_IVRQuestionNotes == null)
                {
                    _IVRQuestionNotes = GetNotes(false, true);
                }
                return _IVRQuestionNotes;
            }
        }
        private List<String> _IVRPostAnswersNotes;
        public List<String> IVRPostAnswersNotes
        {
            get
            {
                if (_IVRPostAnswersNotes == null)
                {
                    _IVRPostAnswersNotes = GetNotes(true, true);
                }
                return _IVRPostAnswersNotes;
            }
        }

        private List<String> GetNotes(Boolean fromPostAnswers, Boolean forIVR)
        {
            List<String> retObj = new List<String>();
            if (fromPostAnswers)
            {
                retObj.Add("Rows with Question ID -999 will be ignored");
            }
            if (forIVR)
            {
                retObj.Add("Listen to the Instructions Recording for the entry format");
            }
            else
            {
                retObj.Add("On YesNo types, 0 = No, 1 = Yes");
                retObj.Add("On YesNoMaybe types, 0 = No, 1 = Yes, 3 = Maybe");
                retObj.Add("On Date types, use yyyy-mm-dd or mm/dd/yyyy format");
            }

            return retObj;
        }
#endregion
    }
}