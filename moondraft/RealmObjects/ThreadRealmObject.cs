using PropertyChanged;
using Realms;
using System;

namespace moondraft.RealmObjects
{

    [DoNotNotify]
    public class ThreadRealmObject : RealmObject
    {
        [PrimaryKey]
        public string ThreadTitle { get; set; }

        public DateTimeOffset ThreadModifiedDateTime { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }
    }
}
