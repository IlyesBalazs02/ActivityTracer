using System.ComponentModel.DataAnnotations;

namespace ActivityTracer.Models
{

    public enum Sports
    {
        Activity,
        Workout,
        Running,
        Hiking,
        Climbing,
        Mountaineering
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NullableProperty : Attribute
    {
    }


    public class AppActivity
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Choose a sport")]
        public Sports SelectedSport { get; set; }

        [StringLength(300)]
		[NullableProperty]
		public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime Time { get; set; }

        [NullableProperty]
        [Range(1,5000)]
        public int? Calories { get; set; }

        [NullableProperty]
        [Range(0, 5000)]
        public int? Elevation { get; set; }

        [NullableProperty]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public int? Pace { get; set; }

        [NullableProperty]
        [Range(1, 5000)]
        public int? Distance { get; set; }

		[NullableProperty]
		[Range(50,220)]
        public int? AvgHeartRate { get; set; }

		[NullableProperty]
		[Range(50, 220)]
		public int? MaxHeartRate { get; set; }
        
        //[StringLength(100)]
        //public List<string> PhotoUrls { get; set; }

        //[NullableProperty]
        //public int? coords { get; set; }

        public AppActivity()
        {
            Id = Guid.NewGuid().ToString();
            this.Description = "";
            this.Distance = 200;
        }
    }
}
