using System;
using System.Collections.Generic;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.TrackingData
{
    public class CreateTrackingRequestDataParameter
    {
        public DateTime? DateTime { get; set; }
        public string TrackingId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public string ScreenHref { get; set; }
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

    public class CreateTrackingRequestDataResult: CreateTrackingRequestDataParameter
    {
        public int Id { get; set; }
    }
    public class CreateTrackingRequestData
    {

        private readonly _DbContext.AwesomeDiContext _db;

        public CreateTrackingRequestData(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public List<string> Validate(List<CreateTrackingRequestDataParameter> paramList)
        {
            var errList = new List<string>();
            return errList;
        }

        public List<CreateTrackingRequestDataResult> Handle(List<CreateTrackingRequestDataParameter> paramList)
        {
            var resList = new List<CreateTrackingRequestDataResult>();
            foreach (var param in paramList)
            {
                try
                {
                    var data = HelperObject.InjectData<TrackingRequestData>(param);
                    if (data.EndTime != null && data.StartTime != null)
                    {
                        data.Duration = (int)(data.EndTime - data.StartTime).Value.TotalMilliseconds;
                    }

                    data.IsSuccess = data.ResponseCode >= 200 && data.ResponseCode < 300;
                    _db.TrackingRequestData.Add(data);
                    resList.Add(HelperObject.InjectData<CreateTrackingRequestDataResult>(data));
                }
                catch (Exception)
                {
                    //ignore
                }
            }
            _db.SaveChanges();
            return resList;
        }
    }
}
