using Microsoft.TeamFoundation.Framework.Server;
using System;
using System.Net.Http;
using System.Text;
using TfsJira.Plugin.Configuration;

namespace TfsJira.Plugin.Service
{
    public class JiraApiService
    {
        private static string ApiHost => Config.JiraApiHostUrl;
        private string GetApiUrlForIssue(string issueId)
        {
            return $"{ApiHost}/issue/{issueId}/remotelink";
        }

        public string CreateIssueLinkFor(string issueId, string linkUrl)
        {
            //TeamFoundationApplicationCore.Log(string.Format("issue:{0}, link:{1}",issueId, linkUrl), 123, System.Diagnostics.EventLogEntryType.Information);
            using (HttpClient client = new HttpClient())
            {
                Uri serviceUrl = new Uri(GetApiUrlForIssue(issueId));
                //var authParameters = GenerateOAuthParameters();
                //string AuthHeader = OAuthUtil.GenerateHeader(serviceUrl, verb, authParameters, false);
                //AuthHeader += string.Format(",realm=\"{0}\"", AccountId);

                //client.DefaultRequestHeaders.Add("User-Agent", Config.Browser.UserAgent);

                client.DefaultRequestHeaders.Add("Authorization", Config.Authorization.Header);
                client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var result = client.PostAsync(serviceUrl,new StringContent(linkUrl,Encoding.UTF8, "application/json")).Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }

    }
}
