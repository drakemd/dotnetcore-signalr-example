namespace OrderService.Store
{
    public static class OrderStore
    {
        private static List<Order> orders = new List<Order>();

        public static int GetCurrentSequence()
        {
            return orders.Count;
        }

        public static void AddOrder(Order model)
        {
            orders.Add(model);
        }

        public static List<Order> GetOrders()
        {
            return orders;
        }

        public static List<Order> GetOrdersBySequence(int from, int to)
        {
            return orders.Where(q => q.Id >= from && q.Id <= to).ToList();
        }
    }
}
