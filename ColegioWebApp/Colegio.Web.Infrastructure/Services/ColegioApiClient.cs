using System.Net.Http.Json;
using System.Text.Json;
using Colegio.Web.Infrastructure.Exceptions;

namespace Colegio.Web.Infrastructure.Services;

public sealed class ColegioApiClient
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _http;

    public ColegioApiClient(HttpClient http) => _http = http;

    public async Task<T> GetAsync<T>(string url, CancellationToken ct)
    {
        var res = await _http.GetAsync(url, ct);
        return await ReadOrThrow<T>(res, ct);
    }

    public async Task PostAsync<TReq>(string url, TReq body, CancellationToken ct)
    {
        var res = await _http.PostAsJsonAsync(url, body, JsonOpts, ct);
        await ReadOrThrowNoContent(res, ct);
    }

    public async Task PutAsync<TReq>(string url, TReq body, CancellationToken ct)
    {
        var res = await _http.PutAsJsonAsync(url, body, JsonOpts, ct);
        await ReadOrThrowNoContent(res, ct);
    }

    public async Task DeleteAsync(string url, CancellationToken ct)
    {
        var res = await _http.DeleteAsync(url, ct);
        await ReadOrThrowNoContent(res, ct);
    }

    private static async Task<T> ReadOrThrow<T>(HttpResponseMessage res, CancellationToken ct)
    {
        if (res.IsSuccessStatusCode)
        {
            var data = await res.Content.ReadFromJsonAsync<T>(JsonOpts, ct);
            if (data is null) throw new ApiException("Respuesta vacía del servidor.", (int)res.StatusCode);
            return data;
        }

        throw await ToApiException(res, ct);
    }

    private static async Task ReadOrThrowNoContent(HttpResponseMessage res, CancellationToken ct)
    {
        if (res.IsSuccessStatusCode) return;
        throw await ToApiException(res, ct);
    }

    private static async Task<ApiException> ToApiException(HttpResponseMessage res, CancellationToken ct)
    {
        var status = (int)res.StatusCode;

        string body = string.Empty;
        try
        {
            body = await res.Content.ReadAsStringAsync(ct);
        }
        catch
        {
        }

        ApiProblemModel? problem = null;
       
        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                problem = JsonSerializer.Deserialize<ApiProblemModel>(body, JsonOpts);
            }
            catch
            {
            }
        }
        
        var msg =
            problem?.Detail ??
            problem?.Title ??
            ExtractMessageFromJson(body) ??
            SafeTrim(body);

        if (string.IsNullOrWhiteSpace(msg))
            msg = "No se pudo completar la operación.";
        
        if (msg.Length > 400)
            msg = msg[..400] + "…";

        return new ApiException(msg, status, problem);
    }

    private static string? ExtractMessageFromJson(string body)
    {
        if (string.IsNullOrWhiteSpace(body)) return null;

        try
        {
            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            
            if (TryGetString(root, "detail", out var detail)) return detail;
            if (TryGetString(root, "title", out var title)) return title;
            if (TryGetString(root, "message", out var message)) return message;

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static bool TryGetString(JsonElement root, string prop, out string? value)
    {
        value = null;
        if (root.ValueKind != JsonValueKind.Object) return false;

        if (root.TryGetProperty(prop, out var p) && p.ValueKind == JsonValueKind.String)
        {
            value = p.GetString();
            return !string.IsNullOrWhiteSpace(value);
        }

        return false;
    }

    private static string? SafeTrim(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        return s.Trim();
    }
}