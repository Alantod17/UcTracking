using System;
using System.Collections.Generic;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.TrackingData
{
    public class CreateTrackingEventDataParameter
    {
        public DateTime? DateTime { get; set; }
        public string TrackingId { get; set; }
        public string Href { get; set; }
        public string Type { get; set; }
    }

    public class CreateTrackingEventDataResult: CreateTrackingEventDataParameter
    {
        public int Id { get; set; }
    }
    public class CreateTrackingEventData
    {

        private readonly _DbContext.AwesomeDiContext _db;

        public CreateTrackingEventData(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public List<string> Validate(List<CreateTrackingEventDataParameter> paramList)
        {
            var errList = new List<string>();
            return errList;
        }

        public List<CreateTrackingEventDataResult> Handle(List<CreateTrackingEventDataParameter> paramList)
        {
            var resList = new List<CreateTrackingEventDataResult>();
            foreach (var param in paramList)
            {
                try
                {
                    var data = HelperObject.InjectData<TrackingEventData>(param);
                    _db.TrackingEventData.Add(data);
                    resList.Add(HelperObject.InjectData<CreateTrackingEventDataResult>(data));
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
