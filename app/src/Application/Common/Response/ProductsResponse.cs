using System.Collections.Generic;

namespace Application.Common.Response
{
    public class ProductsResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public dynamic? Data { get; set; }
    }
}
