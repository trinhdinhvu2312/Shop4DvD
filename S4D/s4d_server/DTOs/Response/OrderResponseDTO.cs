namespace s4dServer.DTOs.Response
{
    public class OrderResponseDTO
    {
        public int OrderID { get; set; }
        public UserResponseDTO? User { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
