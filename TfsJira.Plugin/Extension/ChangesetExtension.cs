using System;
using System.Linq;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.VersionControl.Server;
using TfsJira.Plugin.Configuration;
using TfsJira.Plugin.Model;

namespace TfsJira.Plugin.Extension
{
    public static class ChangesetExtension
    {
       public static JiraIssueRemoteLinkRequest CreateJiraIssueLinkRequest(this CheckinNotificationModel checkinNotification)
        {
            var jiraIssueId = GetJiraIssueIdFrom(checkinNotification.Comment);
            return new JiraIssueRemoteLinkRequest(checkinNotification.ChangesetId.ToString(), jiraIssueId, checkinNotification.Comment);
        }

        /// <summary>
        /// contains all logic to fetch/retrieve jiraissue Id from comment
        /// </summary>
        /// <param name="changesetComment"></param>
        /// <returns></returns>
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

        public static bool HasValidComment(this CheckinNotificationModel checkinNotification)
        {
            return (!string.IsNullOrWhiteSpace(checkinNotification.Comment) && !GetJiraIssueIdFrom(checkinNotification.Comment).Equals(string.Empty));
        }

        public static CheckinNotificationModel ToCheckinNotificationModel(this CheckinNotification checkinNotification, IVssRequestContext requestContext)
        {
            return new CheckinNotificationModel
            {
                AuthorName = checkinNotification.ChangesetOwner.GetUniqueName(requestContext),
                ChangesetId = checkinNotification.Changeset,
                Comment = checkinNotification.Comment
            };
        }
    }
}
