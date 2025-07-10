using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Application.Common.Infrastructure;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class DynamoDbRepository : IDynamoDbService
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        public DynamoDbRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }

        public async Task<List<ProductDetail>> GetProductsAsync(string storeId, string category)
        {
            // 1. Consultar la tabla stock para obtener los sku_id de la tienda
            var stockRequest = new ScanRequest
            {
                TableName = "stock",
                FilterExpression = "store_id = :storeId",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":storeId", new AttributeValue { S = storeId } }
                }
            };
            var stockResponse = await _dynamoDb.ScanAsync(stockRequest);
            var skuIds = stockResponse.Items.Select(i => i["sku_id"].S).ToList();
            if (!skuIds.Any()) return new List<ProductDetail>();

            // 2. Consultar la tabla skus para obtener los detalles de los productos filtrando por category = "juegos"
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
                    skusResponse.Item.TryGetValue("category", out var cat) && cat.S == category)
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
