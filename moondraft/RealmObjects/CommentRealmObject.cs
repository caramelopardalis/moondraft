﻿using moondraft.Logging;
using PropertyChanged;
using RealmClone;
using Realms;
using System;
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

        public async Task UpdateAttachment()
        {
            var unmanagedComment = await Device.InvokeOnMainThreadAsync(() => this.Clone());
            System.Diagnostics.Debug.WriteLine("Update: " + unmanagedComment.AttachmentUrl);

            if (unmanagedComment.AttachmentUrl == null)
            {
                System.Diagnostics.Debug.WriteLine("Skipped uncontained url: " + unmanagedComment.AttachmentUrl);
                return;
            }

            if (unmanagedComment.IsDownloaded())
            {
                System.Diagnostics.Debug.WriteLine("Skipped donwloaded attachment: " + unmanagedComment.AttachmentUrl);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before GetAsync(): " + unmanagedComment.AttachmentUrl);
            var response = await httpClient.GetAsync(unmanagedComment.AttachmentUrl);
            if (!response.IsSuccessStatusCode)
            {
                System.Diagnostics.Debug.WriteLine("Bad success code: " + unmanagedComment.AttachmentUrl);
                return;
            }

            System.Diagnostics.Debug.WriteLine("Before update realm: " + unmanagedComment.AttachmentUrl);
            var file = await response.Content.ReadAsByteArrayAsync();
            var commentReference = await Device.InvokeOnMainThreadAsync(() => ThreadSafeReference.Create(this));
            var resolvedComment = Realm.GetInstance().ResolveReference(commentReference);
            Realm.GetInstance().Write(() =>
            {
                resolvedComment.AttachmentFile = file;
                resolvedComment.AttachmentFileByteSize = resolvedComment.AttachmentFile.Length;
            });
            System.Diagnostics.Debug.WriteLine("After update realm: " + unmanagedComment.AttachmentUrl);
        }

        public bool IsDownloaded()
        {
            return AttachmentFile?.Length > 0;
        }
    }
}
