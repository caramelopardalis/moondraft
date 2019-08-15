using Realms;
using System.Collections.Generic;

namespace moondraft.RealmObjects
{
    public class NodeRealmObject : RealmObject
    {
        public string Url { get; set; }

        public IList<ThreadRealmObject> Threads { get; }
    }
}
