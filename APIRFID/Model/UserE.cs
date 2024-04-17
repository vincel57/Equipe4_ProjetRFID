using System.ComponentModel.DataAnnotations;

namespace APIRFID.Model
{
    public class UserE
    {
        public int id {get;set;}
        [Required]
        public int loginE { get;set;}
        [Required]
        public string passwordE { get;set;}
    }
}
