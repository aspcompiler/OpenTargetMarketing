using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeocodeSharp.Google;

namespace TargetMarketing.Geolocation
{
    //Google API key: AIzaSyCG_-wLmFPaMl2ld7Oh36I82OatdHHZsTQ 
    public class GeolocationService
    {
        public async Task<GeocodeResult> Geocode(string address)
        {
            var client = new GeocodeClient("AIzaSyCG_-wLmFPaMl2ld7Oh36I82OatdHHZsTQ");

            var response = await client.GeocodeAddress(address);
            if (response.Status == GeocodeStatus.Ok)
            {
                var firstResult = response.Results.First();
                return firstResult;
            }
            else
            {
                throw new Exception(response.StatusText);
            }
        }
    }
}
