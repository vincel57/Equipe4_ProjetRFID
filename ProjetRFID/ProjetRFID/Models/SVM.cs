namespace ProjetRFID.Models
{
    public class SVM
    {
        public int id { get; set; }
        public float C {  get; set; }
        public string kernel { get; set; }
        public string gamma { get; set; }
        public float coef0 { get; set; }

        public float tol { get; set; }
        public float cache_size { get; set; }
        public int max_iter { get; set; }
        public DateTime date_sim { get; set; }

    }
}
