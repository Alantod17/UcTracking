using System;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Helpers
{
	public static class HelperObject
    {
        public static T InjectData<T>(Object input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(input));
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
