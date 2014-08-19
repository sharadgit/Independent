using DGuide.Infrastructure.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGuide.Infrastructure.Models
{
    public class Section
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Decimal Sequence { get; set; }

        [StringLength(UDTLength.Name, ErrorMessage = UDTLength.NameErrorLength)]
        [DataType(DataType.MultilineText)]
        public string Header { get; set; }

        [Required]
        [Display(Name = "Format")]
        public DataFormat ContentFormat { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int? DbDocumentId { get; set; }

        public virtual DbDocument DbDocument { get; set; }
    }
}
