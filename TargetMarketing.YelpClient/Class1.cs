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
    public class Class1
    {

    }

    class YelpAPIClient
    {
        private const string CONSUMER_KEY = "Xb4ADOZCfLqXNxCsZT_CAw";
        private const string CONSUMER_SECRET = "HhfaCOSSd8Mx1otjC_urSC3n1b4";
        private const string TOKEN = "zrROWpEg6sOyZLOjSDD8LqUkAewAt0z1";
        private const string TOKEN_SECRET = "KDUwZlx6GAAKo8PWnNMf1cx7iU0";
        private const string API_HOST = "https://api.yelp.com";
        private const string SEARCH_PATH = "/v2/search/";
        private const int SEARCH_LIMIT = 40;

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
    class Options
    {
        public string Term { get; set; }
        public string Location { get; set; }
    }
    class SearchResult
    {

    }
    class Program
    {
        static void Main(string[] args)
        {// ll=37.788022,-122.399797
            //cll=37.77493,-122.419415
            Program.QueryAPIAndPrintResult("bars", "ll=41.2033,-77.1945");
            Console.Read();
        }
        public static void QueryAPIAndPrintResult(string term, string location)
        {
            var client = new YelpAPIClient();

            Console.WriteLine("Querying for {0} in {1}...", term, location);

            JObject response = client.Search(term, location);

            JArray businesses = (JArray)response.GetValue("businesses");

            if (businesses.Count == 0)
            {
                Console.WriteLine("No businesses for {0} in {1} found.", term, location);
                return;
            }

            string business_id = (string)businesses[0]["id"];

            Console.WriteLine(
                "{0} businesses found, querying business info for the top result \"{1}\"...",
                businesses.Count,
                business_id
            );
        }
    }
}
