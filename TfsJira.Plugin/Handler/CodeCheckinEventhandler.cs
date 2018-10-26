using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.VersionControl.Server;
using System;
using TfsJira.Plugin.Extension;
using TfsJira.Plugin.Service;

namespace TfsJira.Plugin.Handler
{
    public class CodeCheckInEventHandler : ISubscriber
    {
        private readonly JiraIssueRemoteLinkService  jiraIssueRemoteLinkService;

        public CodeCheckInEventHandler()
        {
            jiraIssueRemoteLinkService = new JiraIssueRemoteLinkService(new JiraApiService());
        }

        public string Name
        {
            get { return "CodeCheckInEventHandler"; }
        }

        public SubscriberPriority Priority
        {
            get { return SubscriberPriority.Normal; }
        }

        public EventNotificationStatus ProcessEvent(IVssRequestContext requestContext, NotificationType notificationType, object notificationEventArgs, out int statusCode, out string statusMessage, out ExceptionPropertyCollection properties)
        {
            requestContext.MapAndLoadCustomConfig();

            statusCode = 0;
            properties = null;
            statusMessage = String.Empty;
            var logMessage = "New Changeset was checked in by {0}. ID: {1}, comments: {2}";

            try
            {
                if (notificationType == NotificationType.Notification && notificationEventArgs is CheckinNotification)
                {
                    var checkinNotification = notificationEventArgs as CheckinNotification;

                    var checkinNotificationModel = checkinNotification.ToCheckinNotificationModel(requestContext);

                    if (checkinNotificationModel.ContainsValidJiraIssueId())
                    {
                        TeamFoundationApplicationCore.Log(
                            message: string.Format(logMessage, checkinNotificationModel.AuthorName, checkinNotificationModel.ChangesetId, checkinNotificationModel.Comment),
                            eventId: 123,
                            level: System.Diagnostics.EventLogEntryType.Information);

                        var jiraLinkRequests = checkinNotificationModel.CreateJiraIssueLinkRequests();

                        foreach (var request in jiraLinkRequests)
                        {
                            jiraIssueRemoteLinkService.CreateRemoteLinkToIssue(request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TeamFoundationApplicationCore.Log("Sample.SourceControl.Server.PlugIns.CodeCheckInEventHandler encountered an exception \n Exception:" + ex.ToString(), 123, System.Diagnostics.EventLogEntryType.Error);
            }
            return EventNotificationStatus.ActionPermitted;
        }

        public Type[] SubscribedTypes()
        {
            return new Type[1] { typeof(CheckinNotification) };
        }
    }
}
