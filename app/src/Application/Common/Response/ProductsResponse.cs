using System.Collections.Generic;

namespace Application.Common.Response
{
    public class ProductsResponse
    {
        public string detail { get; set; } = string.Empty;
        public int httpStatusCode { get; set; }
        public string data { get; set; } = string.Empty;
    }
}
