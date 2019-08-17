using Realms;
using System;

namespace moondraft.RealmObjects
{
    public class ThreadRealmObject : RealmObject
    {
        [PrimaryKey]
        public string ThreadTitle { get; set; }

        public DateTimeOffset ThreadModifiedDateTime { get; set; }
    }
}
