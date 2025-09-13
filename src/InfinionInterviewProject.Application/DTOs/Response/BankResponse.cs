namespace InfinionInterviewProject.Application.DTOs.Response
{
    public class BankResponse
    {
        public List<BankResult>? result { get; set; }

        public string? errorMessage { get; set; }

        public List<string>? errorMessages { get; set; }

        public bool? hasError { get; set; }

        public DateTime? timeGenerated { get; set; }
    }

    public class BankResult
    {
        public string? bankName { get; set; }

        public string? bankCode { get; set; }
    }

}
