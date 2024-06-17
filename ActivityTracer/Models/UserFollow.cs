namespace ActivityTracer.Models
{
	public class UserFollow
	{
		public string FollowerId { get; set; }
		public SiteUser Follower { get; set; }

		public string FollowedId { get; set; }
		public SiteUser Followed { get; set; }
	}
}
