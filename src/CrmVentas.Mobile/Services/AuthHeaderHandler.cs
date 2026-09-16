using System.Net.Http.Headers;

namespace CrmVentas.Mobile.Services;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly TokenStore _tokenStore;

    public AuthHeaderHandler(TokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_tokenStore.Token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.Token);

        return base.SendAsync(request, cancellationToken);
    }
}
