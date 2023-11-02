namespace s4dServer.DTOs.Request
{
    public class ProductOrderRequestDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public double? Price { get; set; }
    }
}
