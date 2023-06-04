using Dapper;
using Domain.Dtos;
using Infrasturcture.Context;

namespace Infrasturcture.Services;

public class QuoteService
{
    private readonly DapperContext _context;
    private readonly IFileService _fileService;
    public QuoteService(DapperContext context,IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    public List<GetQuoteDto> GetQuotesById()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = "select id,author,quotetext,categoryid,file_name as filename from quotes where id = @Id;";
            return conn.Query<GetQuoteDto>(sql).ToList();
        }
    }
    public List<GetQuoteDto> GetQuotesByCategory()
    {
        using (var conn = _context.CreateConnection())
        {
            var sql = "select id,author,quotetext,categoryid,file_name as filename from quotes where Category_id = @categoryId ;";
            return conn.Query<GetQuoteDto>(sql).ToList();
        }
    }
 
    // public string DeleteFile(string fileName,string Folder)
    //   {
    //      if (quote.File != null)
    //         {
    //             _fileService.DeleteFile("images", existing.FileName);
                
    //         }

         

    //   }

        public GetQuoteDto AddQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            
            var filename = _fileService.CreateFile("images", quote.File);
            var sql = "insert into quotes (author, quotetext, categoryid, file_name) VALUES (@author, @quotetext, @categoryid, @filename) where id = @Id;";
            var result =  conn.ExecuteScalar<int>(sql,new
            {
                
                quote.QuoteText,
                quote.CategoryId,
                filename
            });
           return new GetQuoteDto()
            {
                
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }
    }
     public GetQuoteDto UpdateQuote(AddQuoteDto quote)
    {
        using (var conn = _context.CreateConnection())
        {
            var existing =
                conn.QuerySingleOrDefault<GetQuoteDto>(
                    "select id, author,quotetext,categoryid,file_name as filename from quotes where id=@id;",
                    new {quote.Id});
            if (existing == null)
            {
                return new GetQuoteDto();
            }

            string filename = null;
            if (quote.File != null && existing.FileName != null)
            {
                _fileService.DeleteFile("images", existing.FileName);
                filename = _fileService.CreateFile("images", quote.File);
            }
            else if (quote.File != null && existing.FileName == null)
            {
                filename = _fileService.CreateFile("images", quote.File);
            }
            var sql = "update quotes set author=@author, quotetext=@quotetext,categoryid=@categoryid  where id=@id";
            if (quote.File != null)
            {
                sql =
                    "update quotes set author=@author, quotetext=@quotetext,categoryid=@categoryid,file_name=@filename where id=@id";
            }
            var result =  conn.Execute(sql,new
            {
                quote.Author,
                quote.QuoteText,
                quote.CategoryId,
                filename,
                quote.Id
            });
            return new GetQuoteDto()
            {
                Author = quote.Author,
                QuoteText = quote.QuoteText,
                CategoryId = quote.CategoryId,
                FileName = filename,
                Id = result
            };
        }
    }

}
