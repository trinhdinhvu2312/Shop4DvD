namespace s4dServer.DTOs.Request
{
    public class OrderRequestDTO
    {
        public int OrderId { get; set; }
        public int UserID { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
