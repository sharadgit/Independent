using System.ComponentModel.DataAnnotations;

namespace DGuide.Infrastructure.Core
{
    public enum DataFormat
    {
        [Display(Name = "Text")]
        TEXT,
        [Display(Name = "Html")]
        HTML
    }
    
    public enum DisplayStatus
    {
        Normal,
        Editing,
        Hidden,
        Closed
    }

    public static class UDTLength
    {
        public const int Name = 100;
        public const string NameErrorLength = "Maximum data length is 100 characters.";
        public const int Description = 1000;
        public const string DescriptionErrorLength = "Maximum data length is 1000 characters.";
    }
}
