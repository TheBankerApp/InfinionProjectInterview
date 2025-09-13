namespace InfinionInterviewProject.Application.Shared;

public class ApiResponse<T> : ApiResponseBase
{
    public ApiResponse(bool succeeded)
        : base(succeeded, string.Empty)
    {
    }

    public ApiResponse(T data, string message = "", bool succeeded = true)
        : base(succeeded, message)
    {
        Data = data;
    }

    public ApiResponse(string message)
        : base(true, message)
    {
    }

    public ApiResponse(string message, bool succeeded)
        : base(succeeded, message)
    {
    }

    public T Data { get; }
}

//public class ApiResponse<T> : ApiResponseBase
//{
//    public ApiResponse(bool succeeded)
//        : base(succeeded, string.Empty)
//    {
//    }

//    public ApiResponse(T data, string message = "", bool succeeded = true)
//        : base(succeeded, message)
//    {
//        Data = data;
//    }

//    public ApiResponse(string message)
//        : base(true, message)
//    {
//    }

//    public ApiResponse(string message, int value)
//        : base(true, message)
//    {
//    }

//    public T Data { get; }
//}