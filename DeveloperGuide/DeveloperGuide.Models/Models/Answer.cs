using DGuide.Infrastructure.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGuide.Infrastructure.Models
{
    public class Answer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Format")]
        public DataFormat ContentFormat { get; set; }

        public int Votes { get; set; }

        public string Author { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
