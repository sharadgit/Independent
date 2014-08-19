using DGuide.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGuide.Infrastructure.Models
{
    public class Question
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(UDTLength.Name, ErrorMessage = UDTLength.NameErrorLength)]
        [DataType(DataType.MultilineText)]
        public string Header { get; set; }

        [Required]
        [Display(Name = "Format")]
        public DataFormat ContentFormat { get; set; }

        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public string Tags { get; set; }

        public DisplayStatus DisplayStatus { get; set; }

        public int Votes { get; set; }

        public string Author { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
