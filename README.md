# tfs-jira-server-side-extension
A Tfs server handler which subscribes to tfs CheckinNotification event, retrieve changesets details and create Web Issue link in Jira Issue.

# Summary
As we have moved our agile planning from TFS to Jira, we are still TFS for its other features such as version control, Build and test management.


## Associate TFS changeset with Jira Issues

### TfsJira

TfsJira is a custom solution to link TFS changeset with Jira issues. It included a TFS server extension (server side event handler) which runs on the server and a checkin convention, with in the group of teams who use TFS with Jira, to put their Jira issue in comments while creating TFS checkins.

Developers are supposed to put Jira issue id in their changeset's comment which will be sent to the server as part of changeset and a TFS server side handler would retrieve Jira issue id from the comment and create Jira [Issue Link](https://confluence.atlassian.com/jiracoreserver073/linking-issues-861257339.html#Linkingissues-CreatingalinktoanywebpageURL) to link changeset (Via tfs web Url) with Jira issue. Jira rest API is used to create Jira issue link.

#### Development

TfsJira is developed as custom service side (TFS) event handler to subscribe TFS CheckinNotification event. As the name suggests, It is a post-checkin event handler, give access to whole context of the TFS changeset and can be used to read changeset details such as Id and comments.

You will need TFS server access to test this handler since it needs TFS context to execute.

#### Deployment

Deployment of this server side handler to the TFS server is pretty straightforward. Just drop the assembly containing the event handlers into the Plugins folder of TFS server which in most cases is "../Program Files/Microsoft Team Foundation Server {version}/Application Tier/Web Services/bin/Plugins".