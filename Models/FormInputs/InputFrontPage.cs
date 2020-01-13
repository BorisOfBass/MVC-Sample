using System;

namespace ScreeningTesterWebSite.Models
{
    public class InputFrontPage : ModelBase
    {
        public override void SetDefaults()
        {
            if (String.IsNullOrEmpty(this.CoID))
            {
                this.CoID = "10877";
            }
            if (String.IsNullOrEmpty(this.LoID))
            {
                this.LoID = "TAMFL";
            }
            if (String.IsNullOrEmpty(this.LanguageID))
            {
                this.LoID = GetDefaultLanguageID();
            }
#if DEBUG
            if (String.IsNullOrEmpty(this.EnvironCode))
            {
                this.EnvironCode = "PQ";
            }
#else
            if (String.IsNullOrEmpty(this.EnvironCode))
            {
                this.EnvironCode = "IQ";
            }
#endif
        }
    }
}