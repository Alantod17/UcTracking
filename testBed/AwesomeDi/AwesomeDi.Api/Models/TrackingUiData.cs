using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDi.Api.Models
{
    [Index(nameof(TrackingId),IsUnique = false)]
    [Index(nameof(TrackingId), nameof(System.DateTime), IsUnique = false)]
    public class TrackingUiData: DatedEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateTime { get; set; }
        [MaxLength(100)]
        public string TrackingId { get; set; }
        public string Href { get; set; }
        public string EventType { get; set; }
        public int? PageWidth { get; set; }
        public int? PageHeight { get; set; }
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }
        public string ImageBase64 { get; set; }
    }
}