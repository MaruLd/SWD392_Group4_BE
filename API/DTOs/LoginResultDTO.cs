namespace API.DTOs
{
	public class LoginResultDTO
	{
		public Guid Id { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
		public string Role { get; set; }
		public string Image { get; set; }

	}
}