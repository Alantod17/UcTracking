using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDi.Api.Models
{

    [Index(nameof(TrackingId), IsUnique = false)]
    [Index(nameof(TrackingId), nameof(System.DateTime), IsUnique = false)]
    public class TrackingRequestData: DatedEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateTime { get; set; }
        [MaxLength(100)]
        public string TrackingId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public string Href { get; set; }
        public string EndPoint { get; set; }
        public string Parameter { get; set; }
        public string Result { get; set; }
        public int ResponseCode { get; set; }
        public bool IsSuccess { get; set; }
        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string Environment { get; set; }
        public string VersionNumber { get; set; }
    }
}