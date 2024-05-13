namespace ProjetRFID.Models
{
    public class Random_Forest
    {
        public int id {  get; set; }
        public string criterion { get; set; }
        public int min_samples_split { get; set; }
        public int min_samples_leaf { get; set; }
        public int min_weight_fraction_leaf { get; set; }
        public int max_leaf_nodes { get; set; }
        public float min_impurity_decrease { get; set; }
        public int n_jobs { get; set; }
        public int entier_detail {  get; set; }
        public int max_depth { get; set; }
        public float precision { get; set; }
       // public Simulation Simulation { get; set; }

    }
}
