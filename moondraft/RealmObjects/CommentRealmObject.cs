using PropertyChanged;
using Realms;
using System;

namespace moondraft.RealmObjects
{

    [DoNotNotify]
    public class CommentRealmObject : RealmObject
    {
        [PrimaryKey]
        public string CommentId { get; set; }

        public string CommentBody { get; set; }

        public string CommentAuthorName { get; set; }

        public DateTimeOffset CommentDateTime { get; set; }

        public string AttachmentFileName { get; set; }

        public string AttachmentUrl { get; set; }

        public double AttachmentFileByteSize { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }
    }
}
