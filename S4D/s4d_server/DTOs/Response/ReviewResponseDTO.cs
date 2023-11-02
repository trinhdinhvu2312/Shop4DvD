﻿namespace s4dServer.DTOs.Response
{
    public class ReviewResponseDTO
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
    }

}
