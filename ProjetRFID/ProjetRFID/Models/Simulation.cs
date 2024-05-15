using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetRFID.Models
{
    public class Simulation
    {
        public int id {  get; set; }
        public DateTime time { get; set; }
        [ForeignKey("Analytique")]
        public int idA { get; set; }
        public Analytique Analytique { get; set; }
        [ForeignKey("SVM")]
        public int idS { get; set; }
        public SVM SVM { get; set; }
        [ForeignKey("KNN")]
        public int idk { get; set; }
        public KNN KNN { get; set; }
        [ForeignKey("Random_Forest")]
        public int idR { get; set; }
        public Random_Forest Random_Forest { get; set; }
        [ForeignKey("AspNetUsers")]
        public string UserId { get; set; }
    }
}
