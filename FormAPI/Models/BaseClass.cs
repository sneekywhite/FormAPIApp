namespace FormAPI.Models
{
    public class BaseClass
    {
        public int id { get; set; }
        public DateTime createdTime { get; set; } = DateTime.UtcNow;
        public DateTime updatedTime { get; set; } = DateTime.Now;
        
    }
}
