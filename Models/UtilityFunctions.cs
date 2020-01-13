using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace ScreeningTesterWebSite.Models
{
    public class UtilityFunctions
    {
        public static String xmlAttributeAndValueString(String elementName, String attributeName, String attributeValue, String elementValue)
        {
            return "<" + elementName + " " + attributeName + @"=""" + attributeValue + @""">" + elementValue + @"</" + elementName + @">";
        }

        public static String xmlElementString(String eName, String eValue)
        {
            if (String.IsNullOrEmpty(eValue))
            {
                return "<" + eName + " />";
            }
            else
            {
                return "<" + eName + ">" + eValue + "</" + eName + ">";
            }
        }

        public static String xmlElementDate(String eName, DateTime? eValue)
        {
            if (eValue == null)
            {
                return "<" + eName + "></" + eName + ">";
            }
            else
            {
                return "<" + eName + ">" + eValue.ToString() + "</" + eName + ">"; 
            }
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static String GetWebAPIInputType()
        {
            return "JSON";
        }

        /// <summary>
        /// Returns a Web Service Caller object given just the Environment Code. 
        /// </summary>
        /// <param name="environCode"></param>
        /// <returns></returns>
        public static IFadvWebServiceCaller GetWebService_Caller(IModelBase baseForm)
        {
            // Default values
            String inUser = "RealIVR"; // TO DO default credential values to config??
            String inPW = "x1kR@Zy$";
            if (!String.IsNullOrEmpty(baseForm.UserName) && !String.IsNullOrEmpty(baseForm.UserPW))
            {
                inUser = baseForm.UserName;
                inPW = baseForm.UserPW;
            }
            // Factory
            if (("AD~AQ~AU~AP~").Contains(baseForm.EnvironCode + "~"))  
            {
                return new WebAPI_Caller(inUser, inPW, baseForm.EnvironCode);
            }
            else if (("ID~IQ~IU~IP~").Contains(baseForm.EnvironCode + "~"))
            {
                return new IVRWebService_Caller(inUser, inPW, baseForm.EnvironCode);
            }
            else 
            {
                return new PWS_Caller(inUser, inPW, baseForm.EnvironCode);
            }
        }

        public static void AddLogEntry(String msgText, String level = "INFO")
        {
            DateTime dateStamp = DateTime.Today;
            Char zeroChar = Convert.ToChar("0");
            String logPath = @"D:\logs\WebApiTesterLogs\";

            String logFile = logPath + "WebApiTester" + dateStamp.Year.ToString() + dateStamp.Month.ToString().PadLeft(2, zeroChar) +
                dateStamp.Day.ToString().PadLeft(2, zeroChar) + ".txt";

            msgText = DateTime.Now.ToString() + " " + level + ": " + msgText;
            // This text is added as the file is created 
            if (!File.Exists(logFile))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(msgText);
                }
            }
            else
            {
                // This text is added
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(msgText);
                }
            }
        }

        public static String IVRReplaceRedirectURL(String argumentValue, IModelBase activeObject, Boolean locationSearch = false)
        {
            StringBuilder sb = new StringBuilder(GetBaseURL(activeObject.EnvironCode));

            if (locationSearch)
            {
                sb.Append(@"JobCredits/Questionnaire/csr/LocationSearch.aspx");
            }
            else if (String.IsNullOrEmpty(argumentValue))
            {
                return "";
            }
            else
            {
                sb.Append(@"JobCredits/Questionnaire/csr/ApplicantInfoIVR.aspx?Src=IvrTransfer&arg=");
                sb.Append(argumentValue);
            }

            return sb.ToString();
        }

        public static String IVRReplaceRecordingFile(String argumentValue, IModelBase activeObject, String actionText)
        {
            StringBuilder sb = new StringBuilder(GetBaseURL(activeObject.EnvironCode));

            if (String.IsNullOrEmpty(argumentValue))
            {
                return "";
            }
            else if (actionText == "PlayCompanyRecording")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/CompanyNames/");
            }
            else if (actionText == "PlayCloseRecording")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/Holidays/");
            }
            else if (actionText == "PlayThanksRecording")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/Thanks/");
            }
            else if (actionText == "Question")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/Questions/");
            }
            else if (actionText == "Instruction")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/QuestionTypes/");
            }
            else if (actionText == "Validation")
            {
                sb.Append(@"services/WebAPIIVR/AudioFiles/");
            }
            else
            {
                return "";
            }

            sb.Append(argumentValue);
            return sb.ToString();
        }

        private static String GetBaseURL(String environCode)
        {
            if (environCode.Substring(1, 1) == "P")
                return @"https://www.jobcredits.com/";
            else if (environCode.Substring(1, 1) == "U")
                return @"https://test.jobcredits.com/";
            else
                return @"http://apacblr01web34q:99/";
        }
    }
    /// <summary>
    /// The Encrypt and Decrypt methods in this class need to be used as a pair. 
    /// </summary>
    public static class StringUtil
    {
        public static string Encrypt(string clearText)
        {
            if (String.IsNullOrEmpty(clearText))
            {
                return "";
            }
            string returnText = clearText;

            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    returnText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return returnText;
        }
        public static string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText))
            {
                return "";
            }
            string returnText = cipherText;

            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    returnText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return returnText;
        }
    }
}