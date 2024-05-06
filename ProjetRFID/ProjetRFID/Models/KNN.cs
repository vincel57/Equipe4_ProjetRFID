namespace ProjetRFID.Models
{
    public class KNN
    {
        public int id {  get; set; }
        public int n_neighbors { get; set; }
        public string weight { get; set; }
        public string metric { get; set; }
        public float p {  get; set; }
        public string metric_params { get; set; }
        public string algorithm { get; set; }
        public int leaf_size { get; set; }
        public DateTime date_simulation { get; set; }
    }
}
