namespace s4dServer.DTOs.Request
{
    public class ReviewRequestDTO
    {
        public int ReviewId { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
    }

}
