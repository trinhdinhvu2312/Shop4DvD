namespace s4dServer.DTOs.Response
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(bool status)
        {
            if (!status)
            {
                Status = false;
                ErrorCode = 400;
            }
        }

        public T? Data { get; set; }

        public int ErrorCode { get; set; } = 200;
        public bool Status { get; set; } = true;
        public string Message { get; set; } = "OK";
    }
}
