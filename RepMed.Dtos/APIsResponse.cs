namespace RepMed.Dtos
{

   
    public class APIsResponse<T> 
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int Status { get; set; }
        public object ExtraData { get; set; }

        public APIsResponse()
        {

        }
        public APIsResponse(string message, bool success = false, object data = null, object extraData = null)
        {
            if (data == null)
            {
                data = default(T);
            }

            IsSuccess = success;
            Message = message;
            Data = (T)data;
            ExtraData = extraData;

        }

    }
    public class APIsSuccsss<T> : APIsResponse<T>
    {
        public APIsSuccsss(string message):base(message,true)
        {
            Status = 200;

        }
        public APIsSuccsss(string message, object data, object extraData = null) : base(message,true,data,extraData)
        {

            Status = 200;
        }

    }
    public class APIsError<T> : APIsResponse<T>
    {
        public APIsError(string message) : base(message, false)
        {
            Status = 400;

        }
        public APIsError(string message, object data, object extraData) : base(message, false, data, extraData)
        {

            Status = 500;

        }

    }
    public class APIsUnAuthorize : APIsResponse<bool>
    {
        public APIsUnAuthorize(string message) : base(message, false)
        {
            Status = 401;
        }

    }
}
