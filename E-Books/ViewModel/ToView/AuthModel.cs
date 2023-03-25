using System.Text.Json.Serialization;

namespace E_Books.ViewModel.ToView;

public class AuthModel
{
    public string Masseage { get; set; }
    public bool IsAuthenticated { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhotoPicture{get; set;}
    public List<string> Roles { get; set; }
    public string Token { get; set; }
    public DateTime? ExpiresOn { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
