using System.ComponentModel.DataAnnotations;

namespace APIRFID.Model
{
    public class User
    {

        public int id {  get; set; }
        [Required]
        public int login {  get; set; }
        [Required]
        public string password { get; set; }
    }
}
