using Microsoft.AspNetCore.Identity;

namespace ActivityTracer.Models
{
	public class SiteUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string ContentType { get; set; }
		public byte[] Data { get; set; }

	}
}
