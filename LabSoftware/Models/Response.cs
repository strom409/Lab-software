namespace EasioCore.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public object ResponseData { get; set; }

        public bool ResponseIsError() => !IsSuccess;
        public bool ResponseIsWarning() => IsSuccess && Status != 1;
        public bool ResponseIsSuccess() => IsSuccess && Status == 1;
    }
}
