using DAL.Models;

namespace DAL.Models.Core
{
    public class Medicine : CommonData
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
