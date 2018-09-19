using Microsoft.TeamFoundation.VersionControl.Server;
using System.Configuration;
using TfsJira.Plugin.Extension;
using TfsJira.Plugin.Model;
using TfsJira.Plugin.Service;

namespace TfsJira.Plugin
{
    public class Program
    {
        static void Main(string[] args)
        {
            //CheckinNotification c = new CheckinNotification() { };
            //var service = new JiraIssueRemoteLinkService(new JiraApiService());
            //var request = new JiraIssueRemoteLinkRequest("50448", "CBS-36");
            //service.CreateRemoteLinkToIssue(request);


            // var comment = "CBS-36 sample test changeset";
            // var isBool = (!string.IsNullOrWhiteSpace(comment) && !ChangesetExtension.GetJiraIssueIdFrom(comment).Equals(string.Empty));
            //var testIssueId = ChangesetExtension.GetJiraIssueIdFrom("CBS-36 sample test changeset");

            var name = Configuration.Config.JiraApiHostUrl;
        }
    }
}
