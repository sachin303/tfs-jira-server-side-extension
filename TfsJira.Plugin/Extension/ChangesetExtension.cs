using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.VersionControl.Server;
using TfsJira.Plugin.Configuration;
using TfsJira.Plugin.Model;

namespace TfsJira.Plugin.Extension
{
    public static class ChangesetExtension
    {
       public static IEnumerable<JiraIssueRemoteLinkRequest> CreateJiraIssueLinkRequests(this CheckinNotificationModel checkinNotification)
        {
            var requests = new List<JiraIssueRemoteLinkRequest>();
            var jiraIssueIds = GetJiraIssueIdsFrom(checkinNotification.Comment);

            foreach (var issueId in jiraIssueIds)
            {
                requests.Add(new JiraIssueRemoteLinkRequest(checkinNotification.ChangesetId.ToString(), issueId, checkinNotification.Comment));
            }
            return requests;
        }

        public static string[] GetJiraIssueIdsFrom(string changesetComment)
        {
            var regexString = @"\b({0}-\d*[0-9])\b";
            var allowedJiraProjectKeys = Config.AllowedJiraProjectsKey.Split(Config.AllowedJiraProjectsKeySeparator);
            var issueIds = new List<string>();
            foreach (var projectKey in allowedJiraProjectKeys)
            {
                var match = Regex.Match(changesetComment, string.Format(regexString, projectKey));

                while (match.Captures.Count > 0)
                {
                    if (!issueIds.Contains(match.Value))
                    {
                        issueIds.Add(match.Value);
                    }
                    match = match.NextMatch();
                }


                //if (match.Length > 0)
                //{
                //    for (int i = 0; i < match.Captures.Count; i++)
                //    {
                //        issueIds.Add(match.Captures[i].Value);
                //    }
                //}
            }
            return issueIds.ToArray();
        }

        /// <summary>
        /// contains all logic to fetch/retrieve jiraissue Id from comment (Only support first word as Jira Issue Id)
        /// </summary>
        /// <param name="changesetComment"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GetJiraIssueIdFrom(string changesetComment)
        {
            var jiraIssueId = string.Empty;
            var spaceSeparatedCommentsWords = changesetComment.Split(' ');

            if (spaceSeparatedCommentsWords.Length > 0)
            {
                var jiraRawIssueId = spaceSeparatedCommentsWords.First();
                ParseJiraId(jiraRawIssueId, out jiraIssueId);
            }

            return jiraIssueId;
        }

        [Obsolete]
        private static bool ParseJiraId(string jiraRawIssueId, out string jiraIssueId)
        {
            jiraIssueId = string.Empty;
            var validationresult = false;
            var allowedJiraProjectKeys = Config.AllowedJiraProjectsKey.Split(Config.AllowedJiraProjectsKeySeparator);
            var jiraIssueIdstring = jiraRawIssueId.Split('-');

            if (jiraIssueIdstring.Length > 1)
            {
                var inputJiraProjectKey = jiraIssueIdstring.First();
                var inputJiraIssueIdNumString = jiraIssueIdstring.Last();
                int inputJiraIssueIdNumber;

                if (allowedJiraProjectKeys.Contains(inputJiraProjectKey.ToUpper()) && int.TryParse(inputJiraIssueIdNumString, out inputJiraIssueIdNumber))
                {
                    jiraIssueId = jiraRawIssueId;
                }
            }


            return validationresult;
        }

        public static bool ContainsValidJiraIssueId(this CheckinNotificationModel checkinNotification)
        {
            return (!string.IsNullOrWhiteSpace(checkinNotification.Comment) && GetJiraIssueIdsFrom(checkinNotification.Comment).Any());
        }

        public static CheckinNotificationModel ToCheckinNotificationModel(this CheckinNotification checkinNotification, IVssRequestContext requestContext)
        {
            return new CheckinNotificationModel
            {
                AuthorName = checkinNotification.ChangesetOwner.DisplayName,
                ChangesetId = checkinNotification.Changeset,
                Comment = checkinNotification.Comment
            };
        }
    }
}
