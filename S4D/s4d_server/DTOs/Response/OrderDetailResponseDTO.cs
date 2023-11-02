namespace s4dServer.DTOs.Response
{
    public class OrderDetailResponseDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public ProductResponseDTO? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

}
