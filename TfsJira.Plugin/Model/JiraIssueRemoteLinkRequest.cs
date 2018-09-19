using System.Runtime.Serialization;
using TfsJira.Plugin.Configuration;

namespace TfsJira.Plugin.Model
{
    [DataContract]
    public class JiraIssueRemoteLinkRequest
    {
        public JiraIssueRemoteLinkRequest(string tfsChangesetId, string jiraIssueId, string changesetComment = "")
        {
            JiraIssueId = jiraIssueId;
            RemoteLink = new IssueRemoteLink(tfsChangesetId, changesetComment);
            Relationship = Config.RemoteLinkTitle;
        }

        [IgnoreDataMember]
        public string JiraIssueId { get; private set; }

        [DataMember(Name = "relationship")]
        public string Relationship { get; private set; }

        [DataMember(Name ="object")]
        public IssueRemoteLink RemoteLink { get; private set; }
    }

    [DataContract]
    public class RemoteLinkIcon
    {
        public RemoteLinkIcon()
        {
            IconUrl = Config.RemoteLinkIconUrl;
        }
        [DataMember(Name = "url16x16")]
        public string IconUrl { get; set; }

    }

    [DataContract]
    public class IssueRemoteLink {


        private string baseTitle = "";
        private string baseTfsChangesetUrl = Config.TfsChangesetBaseUrl;
        public IssueRemoteLink(string tfsChangesetId, string summary = "")
        {
            Title = baseTitle + tfsChangesetId;
            Url = baseTfsChangesetUrl + tfsChangesetId;
            Summary = summary;
            Icon = new RemoteLinkIcon();
        }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "icon")]
        public RemoteLinkIcon Icon { get; set; }

        [DataMember(Name = "title")]
        public string Title
        {
            get;set;
        }
    }
}
