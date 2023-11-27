using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtension
    {
        public static void AddPaginationHeader(this HttpResponse response,int currentPage,int itemsPerPage,int totalItems,int totalPages)
        {
            var PaginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
            var options=new JsonSerializerOptions
            {
                PropertyNamingPolicy=JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination",JsonSerializer.Serialize(PaginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}