using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Application.Common.Infrastructure;
using Application.Queries;
using Application.Queries.ValidateJWT;
using AWS.Lambda.Powertools.Logging;
using Infrastructure.Repositories;
using Infrastructure.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambda;
public class Function
{
    private readonly DynamoDbRepository _productService = new DynamoDbRepository(new AmazonDynamoDBClient());
    private readonly IValidateTokenService _validateTokenRepository = new ValidateTokenService();

    [Logging(LogEvent = true)]
    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        Logger.LogInformation("Inicio de la función");

        if (request.HttpMethod != "GET" || request.HttpMethod == "OPTIONS")
        {
            return CreateCorsResponse(405, "Method Not Allowed");
        }

        if (request.HttpMethod == "OPTIONS")
        {
            return CreateCorsResponse(200, string.Empty);
        }

        try
        {
            if (!request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrEmpty(authHeader))
            {
                return CreateCorsResponse(401, "Token no proporcionado");
            }

            var token = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader.Substring("Bearer ".Length).Trim()
                : authHeader.Trim();

            var tokenIsValid = await new ValidateJWTQuery(_validateTokenRepository).Execute(token);

            if (!tokenIsValid)
            {
                Logger.LogWarning("Token inválido: {token}", token);
                return CreateCorsResponse(401, "Token inválido");
            }

            Logger.LogInformation("Token recibido: {token}", token);

            // Obtener parámetros de query string
            if (request.QueryStringParameters == null ||
                !request.QueryStringParameters.TryGetValue("store_id", out var storeId) ||
                !request.QueryStringParameters.TryGetValue("category", out var category))
            {
                return CreateCorsResponse(400, "Faltan parámetros obligatorios: store_id y category");
            }

            var result = await new GetProductsByStoreAndCategoryQuery(_productService).Execute(storeId, category);
            Logger.LogInformation("Fin de la función");
            return CreateCorsResponse(result.httpStatusCode, result.data);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error interno en la función");
            return CreateCorsResponse(500, "Internal Server Error");
        }
    }

    private APIGatewayProxyResponse CreateCorsResponse(int statusCode, string body)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = statusCode,
            Body = body,
            Headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Methods", "GET" },
                { "Access-Control-Allow-Headers", "Content-Type, Authorization" }
            }
        };
    }
}
