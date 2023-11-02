namespace s4dServer.DTOs.Request
{
    public class OrderDetailRequestDTO
    {
        public int OrderDetailId { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

}
