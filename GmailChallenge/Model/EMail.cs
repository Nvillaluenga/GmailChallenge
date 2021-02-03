using System;
using System.ComponentModel.DataAnnotations;

namespace GmailChallenge
{
    public class EMail
    {
        [Key]
        public int EMailId { get; set; }
        
        [Required]
        public DateTime Fecha { get; set; }
        
        [Required]
        public string From { get; set; }
        
        [Required]
        public string Subject { get; set; }
    }
}
