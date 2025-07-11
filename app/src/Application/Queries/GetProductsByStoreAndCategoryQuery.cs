using Application.Common.Infrastructure;
using Application.Common.Response;
using AWS.Lambda.Powertools.Logging;
using System.Net;

namespace Application.Queries;
public class GetProductsByStoreAndCategoryQuery
{
    private readonly IDynamoDbService _db;

    public GetProductsByStoreAndCategoryQuery(IDynamoDbService db)
    {
        _db = db;
    }

    public async Task<ProductsResponse> Execute(string storeId, string category)
    {
        try
        {
            var products = await _db.GetProductsAsync(storeId, category);

            var response = new ProductsResponse
            {
                Message = products.Count > 0 ? "Productos encontrados" : "No se encontraron productos",
                Status = (int)HttpStatusCode.OK,
                Data = products
            };

            Logger.LogInformation("Consulta de productos exitosa: {@response}", response);

            return response;
        }
        catch (Exception ex)
        {
            var request = new { StoreId = storeId, Category = category };

            Logger.LogError(ex, "Error al obtener productos {@request}", request);

            return new ProductsResponse
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Message = "Error interno del servidor",
                Data = null
            };
        }
    }
}
