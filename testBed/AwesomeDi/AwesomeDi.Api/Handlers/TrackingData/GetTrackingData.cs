using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.TrackingData
{
    public class GetTrackingDataParameter
    {
        public string TrackingId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
    public class GetTrackingDataResult
    {
        public string Type { get; set; }
        public string TrackingId { get; set; }
        public DateTime DateTime { get; set; }
        public string DataDetail { get; set; }
    }
    public class GetTrackingData
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public GetTrackingData(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public List<KeyValuePair<string, string>> Validate(GetTrackingDataParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if(param.StartDateTime == null) errorList.Add(new KeyValuePair<string, string>(nameof(param.StartDateTime), "StartDateTime is required"));
            if(param.EndDateTime == null) errorList.Add(new KeyValuePair<string, string>(nameof(param.EndDateTime), "EndDateTime is required"));
            return errorList;
        }

        public List<GetTrackingDataResult> Handle(GetTrackingDataParameter param)
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

            var uiDataList = uiData.ToList();
            var requestDataList = requestData.ToList();
            var eventDataList = eventData.ToList();
            var resList = new List<GetTrackingDataResult>();
            foreach (var ui in uiDataList)
            {
                var res = new GetTrackingDataResult();
                res.DateTime = (DateTime)ui.DateTime;
                res.TrackingId = ui.TrackingId;
                res.Type = "Ui";
                res.DataDetail = JsonConvert.SerializeObject(ui);
                resList.Add(res);
            }
            foreach (var request in requestDataList)
            {
                var res = new GetTrackingDataResult();
                res.DateTime = (DateTime)request.DateTime;
                res.TrackingId = request.TrackingId;
                res.Type = "Request";
                res.DataDetail = JsonConvert.SerializeObject(request);
                resList.Add(res);
            }
            foreach (var eve in eventDataList)
            {
                var res = new GetTrackingDataResult();
                res.DateTime = (DateTime)eve.DateTime;
                res.TrackingId = eve.TrackingId;
                res.Type = "Event";
                res.DataDetail = JsonConvert.SerializeObject(eve);
                resList.Add(res);
            }

            return resList.OrderBy(x => x.DateTime).ToList();

        }
    }
}
