using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.TrackingData
{
    public class SearchTrackingDataParameter
    {
        public string TrackingId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
    public class SearchTrackingDataResult
    {
        public string TrackingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
    public class SearchTrackingData
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public SearchTrackingData(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public List<KeyValuePair<string, string>> Validate(SearchTrackingDataParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if(param.StartDateTime == null) errorList.Add(new KeyValuePair<string, string>(nameof(param.StartDateTime), "StartDateTime is required"));
            if(param.EndDateTime == null) errorList.Add(new KeyValuePair<string, string>(nameof(param.EndDateTime), "EndDateTime is required"));
            return errorList;
        }

        public List<SearchTrackingDataResult> Handle(SearchTrackingDataParameter param)
        {
            IQueryable<TrackingUiData> uiData = _db.TrackingUiData.Where(x=>x.DateTime != null);
            IQueryable<TrackingRequestData> requestData = _db.TrackingRequestData.Where(x => x.DateTime != null);
            IQueryable<TrackingEventData> eventData = _db.TrackingEventData.Where(x => x.DateTime != null);

            if (!string.IsNullOrWhiteSpace(param.TrackingId))
            {
                uiData = uiData.Where(x => x.TrackingId == param.TrackingId);
                requestData = requestData.Where(x => x.TrackingId == param.TrackingId);
                eventData = eventData.Where(x => x.TrackingId == param.TrackingId);
            }
            if (param.StartDateTime != null)
            {
                uiData = uiData.Where(x => x.DateTime >= param.StartDateTime);
                requestData = requestData.Where(x => x.DateTime >= param.StartDateTime);
                eventData = eventData.Where(x => x.DateTime >= param.StartDateTime);
            }
            if (param.EndDateTime != null)
            {
                uiData = uiData.Where(x => x.DateTime <= param.EndDateTime);
                requestData = requestData.Where(x => x.DateTime <= param.EndDateTime);
                eventData = eventData.Where(x => x.DateTime <= param.EndDateTime);
            }

            var uiDataList = uiData.GroupBy(x=>x.TrackingId).Select(x=> new SearchTrackingDataResult{TrackingId = x.Key, StartDateTime = (DateTime)x.Min(v=>v.DateTime), EndDateTime = (DateTime)x.Max(v=>v.DateTime)}).ToList();
            var requestDataList = requestData.GroupBy(x => x.TrackingId).Select(x => new SearchTrackingDataResult { TrackingId = x.Key, StartDateTime = (DateTime)x.Min(v => v.DateTime), EndDateTime = (DateTime)x.Max(v => v.DateTime) }).ToList();
            var eventDataList = eventData.GroupBy(x => x.TrackingId).Select(x => new SearchTrackingDataResult { TrackingId = x.Key, StartDateTime = (DateTime)x.Min(v => v.DateTime), EndDateTime = (DateTime)x.Max(v => v.DateTime) }).ToList();
            var resList = new List<SearchTrackingDataResult>();
            resList.AddRange(uiDataList);
            resList.AddRange(requestDataList);
            resList.AddRange(eventDataList);
            resList = resList.GroupBy(x => x.TrackingId).Select(x => new SearchTrackingDataResult { TrackingId = x.Key, StartDateTime = x.Min(v => v.StartDateTime), EndDateTime = x.Max(v => v.EndDateTime) }).ToList();
            return resList;
        }
    }
}
