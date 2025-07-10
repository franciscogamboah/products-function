namespace Domain.Entities
{
    public class ProductDetail
    {
        public string sku_id { get; set; } = string.Empty;
        public string brand { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public int price { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
