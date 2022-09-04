using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDi.Api.Models
{
    [Index(nameof(TrackingId),IsUnique = false)]
    [Index(nameof(TrackingId), nameof(System.DateTime), IsUnique = false)]
    public class TrackingEventData : DatedEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateTime { get; set; }
        [MaxLength(100)]
        public string TrackingId { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
        public string EventDetail { get; set; }
    }
}