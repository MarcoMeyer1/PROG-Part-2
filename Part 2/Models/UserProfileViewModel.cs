namespace Part_2.Models
{
    public class UserProfileViewModel
    {
        public User Farmer { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public UserProfile Profile { get; set; }  
    }
}
