namespace TfsJira.Plugin.Model
{
    public class CheckinNotificationModel
    {
        public int ChangesetId { get; set; }

        public string AuthorName { get; set; }

        public string Comment { get; set; }

    }
}
