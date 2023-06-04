using Microsoft.AspNetCore.Http;


namespace Domain.Dtos;

public class GetQuoteDto : QuoteDto
{
    public IFormFile File { get; set; }
}
