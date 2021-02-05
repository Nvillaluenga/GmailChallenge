using System;
using System.ComponentModel.DataAnnotations;

namespace GmailChallenge
{
    public class Email
    {
        [Key]
        public int EmailId { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }
        
        [Required]
        public string From { get; set; }
        
        [Required]
        public string Subject { get; set; }
    }
}
