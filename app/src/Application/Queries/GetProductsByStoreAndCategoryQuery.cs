using Application.Common.Response;
using AWS.Lambda.Powertools.Logging;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Queries;
public class GetProductsByStoreAndCategoryQuery
{
    private readonly ProductService _productService;

    public GetProductsByStoreAndCategoryQuery(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductsResponse> Execute(string storeId, string category)
    {
        try
        {
            var products = await _productService.GetProductsAsync(storeId, category);
            var response = new ProductsResponse
            {
                detail = products.Count > 0 ? "Productos encontrados" : "No se encontraron productos",
                httpStatusCode = (int)HttpStatusCode.OK,
                data = JsonSerializer.Serialize(products)
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
                detail = "Error interno del servidor",
                httpStatusCode = (int)HttpStatusCode.InternalServerError,
                data = string.Empty
            };
        }
    }
}
