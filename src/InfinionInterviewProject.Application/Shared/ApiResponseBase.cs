namespace InfinionInterviewProject.Application.Shared;

public class ApiResponseBase
{
    protected ApiResponseBase(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public bool Succeeded { get; }
    public string Message { get; }
}