using Amazon.DynamoDBv2.Model;
using Domain.Entities;

namespace Application.Common.Infrastructure;
public interface IDynamoDbService
{
    Task<List<ProductDetail>> GetProductsAsync(string storeId, string category);
}
