using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracer.Models
{
	public class SiteUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string ContentType { get; set; }
		public byte[] Data { get; set; }

		[NotMapped]
		public virtual ICollection<UserFollow> Followers { get; set; }

		[NotMapped]
		public virtual ICollection<UserFollow> Followings { get; set; }

		[NotMapped]
		public virtual ICollection<AppActivity> Activities { get; set; }

		public SiteUser()
		{
			Followers = new List<UserFollow>();
			Followings = new List<UserFollow>();
			Activities = new List<AppActivity>();
		}
	}
}
