using DGuide.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGuide.Infrastructure.Models
{
    public class Article 
        //: IValidatableObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(UDTLength.Name, ErrorMessage = UDTLength.NameErrorLength)]
        [DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(UDTLength.Description, ErrorMessage = UDTLength.DescriptionErrorLength)]
        public string Description { get; set; }
        
        public string Tags { get; set; }

        [Display(Name="Version")]
        public int? DbVersionId { get; set; }

        public DisplayStatus DisplayStatus { get; set; }

        public int Votes { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; }
        
        public string Author { get; set; }

        public virtual DbVersion DbVersion { get; set; }
        public virtual ICollection<Section> Sections { get; set; }        
    }
}
