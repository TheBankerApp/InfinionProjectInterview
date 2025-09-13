namespace InfinionInterviewProject.Application.Shared;

public class ApiErrorResponse : ApiResponseBase
{
    public ApiErrorResponse(string message)
        : base(false, message)
    {
    }

    public ApiErrorResponse(string error, string customMessage)
        : base(false, customMessage)
    {
        Error = error;
    }

    public string Error { get; }
}