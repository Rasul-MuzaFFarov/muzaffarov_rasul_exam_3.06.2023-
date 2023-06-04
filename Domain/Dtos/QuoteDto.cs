namespace Domain.Dtos;

public class QuoteDto
{
    public int Id  { get; set; }
    public string Author { get; set; }
    public string QuoteText { get; set; }
    public string FileName { get; set; }
    public int CategoryId { get; set; }
}
