namespace Part_2.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime ProductionDate { get; set; }
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }
        public decimal Price { get; set; }
    }
}
