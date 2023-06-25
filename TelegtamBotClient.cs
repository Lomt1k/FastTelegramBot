using FastTelegramBot.DataTypes;

namespace FastTelegramBot;
public class TelegtamBotClient
{
    public const string apiUrl = "https://api.telegram.org/";

    private HttpClient _httpClient = new();
    private string _baseRequestUrl;

    public string Token { get; }

    public TelegtamBotClient(string token)
    {
        _baseRequestUrl = $"{apiUrl}bot{token}/";
        Token = token;
    }

    public async Task<User> GetMeAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _baseRequestUrl + "getMe");
        var httpResponse = await _httpClient.SendAsync(request).ConfigureAwait(false);
        var contentJson = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var user = JsonHelper.ReadResult<User>(contentJson);
        return user;
    }

    

}
