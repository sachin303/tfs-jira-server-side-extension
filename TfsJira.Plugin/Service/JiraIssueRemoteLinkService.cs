using System;
using System.IO;
using System.Runtime.Serialization.Json;
using TfsJira.Plugin.Model;

namespace TfsJira.Plugin.Service
{
    public class JiraIssueRemoteLinkService
    {
        JiraApiService _jiraApiService;
        public JiraIssueRemoteLinkService(JiraApiService jiraApiService)
        {
            _jiraApiService = jiraApiService;
        }

        public void CreateRemoteLinkToIssue(JiraIssueRemoteLinkRequest jiraIssueRemoteLinkRequest)
        {
            if (jiraIssueRemoteLinkRequest == null)
            {
                throw new ArgumentNullException(nameof(jiraIssueRemoteLinkRequest));
            }

            _jiraApiService.CreateIssueLinkFor(jiraIssueRemoteLinkRequest.JiraIssueId, 
                SerializeToJson(jiraIssueRemoteLinkRequest));
        }

        private string SerializeToJson(JiraIssueRemoteLinkRequest jiraIssueRemoteLinkRequest)
        {
            var serializer = new DataContractJsonSerializer(jiraIssueRemoteLinkRequest.GetType());
            string jsonString = string.Empty;
            using (var stream1 = new MemoryStream())
            {
                serializer.WriteObject(stream1, jiraIssueRemoteLinkRequest);
                stream1.Position = 0;
                using (var sr = new StreamReader(stream1))
                {
                    jsonString = sr.ReadToEnd();
                }
            }

            return jsonString;
        }
    }
}
