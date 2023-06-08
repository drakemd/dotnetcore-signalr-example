namespace OrderService.Store
{
    public class Order
    {
        public Order(int id, string details)
        {
            Id = id;
            Details = details;
        }

        public int Id { get; set; }
        public string Details { get; set; }
        public string Status { get; set; } = "On Progress";
    }
}
