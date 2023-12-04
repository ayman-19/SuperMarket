namespace DB_Core.Models
{
	public class UserToken
	{
        public string UserName { get; set; }
        public string Massage { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpirations { get; set; }
        public DateTime RefreshTokenExpirations { get; set; }
    }
}
