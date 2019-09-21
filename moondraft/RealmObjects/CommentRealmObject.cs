using moondraft.Logging;
using PropertyChanged;
using Realms;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

        public async Task UpdateAttachment()
        {
            System.Diagnostics.Debug.WriteLine("Update: " + AttachmentUrl);

            if (AttachmentUrl == null)
            {
                System.Diagnostics.Debug.WriteLine("Skipped uncontained url: " + AttachmentUrl);
                return;
            }

            if (IsDownloaded())
            {
                System.Diagnostics.Debug.WriteLine("Skipped donwloaded attachment: " + AttachmentUrl);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before GetAsync(): " + AttachmentUrl);
            var response = await httpClient.GetAsync(AttachmentUrl);
            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Bad success code: " + AttachmentUrl);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before update realm: " + AttachmentUrl);
            Realm.GetInstance().Write(async () =>
            {
                AttachmentFile = await response.Content.ReadAsByteArrayAsync();
                AttachmentFileByteSize = AttachmentFile.Length;
            });
            System.Diagnostics.Debug.WriteLine("After update realm: " + AttachmentUrl);
        }

        public bool IsDownloaded()
        {
            return AttachmentFile?.Length > 0;
        }
    }
}
