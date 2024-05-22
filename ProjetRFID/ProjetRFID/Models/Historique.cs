using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetRFID.Models
{
    public class Historique
    {
        public int id { get; set; }
        public DateTime time_connex { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserName { get; set; }
        
    }
}
