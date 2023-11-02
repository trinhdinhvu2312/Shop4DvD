using System;
using System.Collections.Generic;

namespace s4dServer.DTOs.Response
{
    public class OrderInfoResponseDTO
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}