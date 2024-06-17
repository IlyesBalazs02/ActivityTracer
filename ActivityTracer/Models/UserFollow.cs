using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityTracer.Models
{
	public class UserFollow
	{
		[Key]
		public int Id { get; set; }

		public string FollowerId { get; set; }

		[ForeignKey("FollowerId")]
		public virtual SiteUser Follower { get; set; }

		public string FollowingId { get; set; }

		[ForeignKey("FollowingId")]
		public virtual SiteUser Following { get; set; }
	}
}
