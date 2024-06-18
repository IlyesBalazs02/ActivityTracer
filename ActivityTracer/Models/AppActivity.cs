using ActivityTracer.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

    [ModelBinder(BinderType = typeof(AppActivityBinder))]
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
        public DateTime? Pace { get; set; }

        [NullableProperty]
        [Range(1, 5000)]
        public int? Distance { get; set; }

		[NullableProperty]
		[Range(50,220)]
        public int? AvgHeartRate { get; set; }

		[NullableProperty]
		[Range(50, 220)]
		public int? MaxHeartRate { get; set; }

        [BindNever]
        public string OwnerId { get; set; }

        [BindNever]
        [NotMapped] //lazyloading
        [JsonIgnore]
        public virtual SiteUser Owner { get; set; }

		public List<string>? PhotoUrl { get; set; }

		//[NullableProperty]
		//public int? coords { get; set; }

		public AppActivity()
        {
            Id = Guid.NewGuid().ToString();
            this.Description = "";
        }
    }
}
