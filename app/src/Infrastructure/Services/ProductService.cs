using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
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

    public class ProductService
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        public ProductService(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<List<ProductDetail>> GetProductsAsync(string storeId, string category)
        {
            // 1. Consultar la tabla stock para obtener los sku_id activos para la tienda
            var stockRequest = new QueryRequest
            {
                TableName = "stock",
                IndexName = null, // Si tienes un GSI para store_id, ponlo aquí
                KeyConditionExpression = "store_id = :storeId",
                FilterExpression = "#status = :active",
                ExpressionAttributeNames = new Dictionary<string, string> { {"#status", "status"} },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":storeId", new AttributeValue { S = storeId } },
                    { ":active", new AttributeValue { S = "active" } }
                }
            };
            var stockResponse = await _dynamoDb.QueryAsync(stockRequest);
            var skuIds = stockResponse.Items.Select(i => i["sku_id"].S).ToList();
            if (!skuIds.Any()) return new List<ProductDetail>();

            // 2. Consultar la tabla skus para obtener los detalles de los productos filtrando por sku_id, category y status
            var products = new List<ProductDetail>();
            foreach (var skuId in skuIds)
            {
                var skusRequest = new GetItemRequest
                {
                    TableName = "skus",
                    Key = new Dictionary<string, AttributeValue> { { "sku_id", new AttributeValue { S = skuId } } }
                };
                var skusResponse = await _dynamoDb.GetItemAsync(skusRequest);
                if (skusResponse.Item != null &&
                    skusResponse.Item.TryGetValue("category", out var cat) && cat.S == category &&
                    skusResponse.Item.TryGetValue("status", out var stat) && stat.S == "active")
                {
                    products.Add(new ProductDetail
                    {
                        sku_id = skuId,
                        brand = skusResponse.Item["brand"].S,
                        category = skusResponse.Item["category"].S,
                        name = skusResponse.Item["name"].S,
                        price = int.Parse(skusResponse.Item["price"].N),
                        status = skusResponse.Item["status"].S
                    });
                }
            }
            return products;
        }
    }
}
