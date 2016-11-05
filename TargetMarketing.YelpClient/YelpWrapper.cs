using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SimpleOAuth;
using System.IO;
using System.Net;

namespace TargetMarketing.YelpClient
{
    public class YelpWrapper
    {
        // Search("bars", "ll=41.2033,-77.1945")        
        public static List<SearchResult> Search(string term, string location)
        {
            List<SearchResult> result = new List<SearchResult>();
            var client = new YelpAPIClient();
            JObject response = client.Search(term, location);
            JArray businesses = (JArray)response.GetValue("businesses");
            foreach(JObject b in businesses)
            {
                SearchResult r = new SearchResult();
                r.latitude = Convert.ToDouble(((JObject)((JObject)b.GetValue("location")).GetValue("coordinate")).GetValue("latitude"));
                r.longitude = Convert.ToDouble(((JObject)((JObject)b.GetValue("location")).GetValue("coordinate")).GetValue("longitude"));
                result.Add(r);
            }
            return result;
        }
    }

    class YelpAPIClient
    {
        private const string CONSUMER_KEY = "Xb4ADOZCfLqXNxCsZT_CAw";
        private const string CONSUMER_SECRET = "HhfaCOSSd8Mx1otjC_urSC3n1b4";
        private const string TOKEN = "zrROWpEg6sOyZLOjSDD8LqUkAewAt0z1";
        private const string TOKEN_SECRET = "KDUwZlx6GAAKo8PWnNMf1cx7iU0";
        private const string API_HOST = "https://api.yelp.com";
        private const string SEARCH_PATH = "/v2/search/";
        private const int SEARCH_LIMIT = 20;

        private JObject PerformRequest(string baseURL, Dictionary<string, string> queryParams = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(String.Empty);
            if (queryParams == null)
            {
                queryParams = new Dictionary<string, string>();
            }
            foreach (var queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }
            var uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Query = query.ToString();
            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";
            request.SignRequest(
                new Tokens
                {
                    ConsumerKey = CONSUMER_KEY,
                    ConsumerSecret = CONSUMER_SECRET,
                    AccessToken = TOKEN,
                    AccessTokenSecret = TOKEN_SECRET
                }
            ).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return JObject.Parse(stream.ReadToEnd());
        }
        public JObject Search(string term, string location)
        {
            string baseURL = API_HOST + SEARCH_PATH;
            var queryParams = new Dictionary<string, string>()
            {
                { "term", term },
                { "location", location },
                { "limit", SEARCH_LIMIT.ToString() }
            };
            return PerformRequest(baseURL, queryParams);
        }
    }
    public class Options
    {
        public string Term { get; set; }
        public string Location { get; set; }
    }
    public class SearchResult
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
