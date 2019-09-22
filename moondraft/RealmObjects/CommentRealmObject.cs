using moondraft.Logging;
using PropertyChanged;
using Realms;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace moondraft.RealmObjects
{
    [DoNotNotify]
    public class CommentRealmObject : RealmObject
    {
        public string CommentId { get; set; }

        public string CommentBody { get; set; }

        public string CommentAuthorName { get; set; }

        public DateTimeOffset CommentDateTime { get; set; }

        public string AttachmentFileName { get; set; }

        public string AttachmentExtension { get; set; }

        public string AttachmentUrl { get; set; }

        public double AttachmentFileByteSize { get; set; }

        public byte[] AttachmentFile { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        static HttpClient httpClient = new HttpClient(new HttpClientLoggingHandler(new HttpClientHandler()));

        public async Task UpdateAttachment(string threadTitle, string commentId)
        {
            var r = Realm.GetInstance();
            r.Refresh();
            var comments = r.All<SettingsRealmObject>().First().CurrentNode.Threads.First(o => o.ThreadTitle == threadTitle).Comments.ToList();
            Logger.Debug("comment: {0}", comments.Count);
            var url = Realm.GetInstance().All<CommentRealmObject>().First(o => o.CommentId == commentId).AttachmentUrl;
            System.Diagnostics.Debug.WriteLine("Update: " + url);

            if (Realm.GetInstance().All<CommentRealmObject>().First(o => o.CommentId == commentId).AttachmentUrl == null)
            {
                System.Diagnostics.Debug.WriteLine("Skipped uncontained url: " + url);
                return;
            }

            if (IsDownloaded(commentId))
            {
                System.Diagnostics.Debug.WriteLine("Skipped donwloaded attachment: " + url);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before GetAsync(): " + url);
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Bad success code: " + url);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before update realm: " + url);
            var file = await response.Content.ReadAsByteArrayAsync();
            await Device.InvokeOnMainThreadAsync(() =>
            {
                var realm = Realm.GetInstance();
                var comment = realm.All<CommentRealmObject>().First(o => o.CommentId == commentId);
                realm.Write(() =>
                {
                    comment.AttachmentFile = file;
                    comment.AttachmentFileByteSize = comment.AttachmentFile.Length;
                });
            });
            System.Diagnostics.Debug.WriteLine("After update realm: " + url);
        }

        public bool IsDownloaded(string commentId)
        {
            var comment = Realm.GetInstance().All<CommentRealmObject>().First(o => o.CommentId == commentId);
            Logger.Debug("comment: " + comment + ", attachment file: " + comment?.AttachmentFile);
            return comment?.AttachmentFile?.Length > 0;
        }
    }
}
