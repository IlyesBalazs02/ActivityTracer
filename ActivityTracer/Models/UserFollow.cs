using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracer.Models
{
	public class UserFollow
	{
		public string FollowerId { get; set; }
		[NotMapped]
		public virtual SiteUser Follower { get; set; }

		public string FollowingId { get; set; }
		[NotMapped]
		public virtual SiteUser Following { get; set; }
	}
}
