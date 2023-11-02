namespace s4dServer.Models
{
    public class PagingModel<T>
    {
        public PagingModel()
        {
        }

        public List<T>? Data { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalPage { get; set; }
    }
}
