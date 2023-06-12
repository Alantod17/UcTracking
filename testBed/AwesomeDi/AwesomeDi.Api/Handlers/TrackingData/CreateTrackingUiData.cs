using System;
using System.Collections.Generic;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.TrackingData
{
    public class CreateTrackingUiDataParameter
    {
        public DateTime? DateTime { get; set; }
        public string TrackingId { get; set; }
        public string EventType { get; set; }
        public int? PageWidth { get; set; }
        public int? PageHeight { get; set; }
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class CreateTrackingUiDataResult: CreateTrackingUiDataParameter
    {
        public int Id { get; set; }
    }
    public class CreateTrackingUiData
    {

        private readonly _DbContext.AwesomeDiContext _db;

        public CreateTrackingUiData(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public List<string> Validate(List<CreateTrackingUiDataParameter> paramList)
        {
            var errList = new List<string>();
            return errList;
        }

        public List<CreateTrackingUiDataResult> Handle(List<CreateTrackingUiDataParameter> paramList)
        {
            var resList = new List<CreateTrackingUiDataResult>();
            foreach (var param in paramList)
            {
                try
                {
                    var data = HelperObject.InjectData<TrackingUiData>(param);
                    _db.TrackingUiData.Add(data);
                    resList.Add(HelperObject.InjectData<CreateTrackingUiDataResult>(data));
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
