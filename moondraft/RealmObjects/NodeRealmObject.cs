using AngleSharp.Html.Parser;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace moondraft.RealmObjects
{
    [DoNotNotify]
    public class NodeRealmObject : RealmObject
    {
        public static readonly string RecentUrl = "gateway.cgi/changes";

        public static readonly string ThreadUrl = "thread.cgi/{threadTitle}";

        public string Url { get; set; }

        public IList<ThreadRealmObject> Threads { get; }

        public ThreadRealmObject CurrentThread { get; set; }

        public async Task UpdateThreadsAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(Url + RecentUrl);
            var document = await new HtmlParser().ParseDocumentAsync(await response.Content.ReadAsStringAsync());
            var liElements = document.QuerySelectorAll("#thread_index > li");
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                foreach (var liElement in liElements)
                {
                    var threadTitle = liElement.QuerySelector("a").TextContent;
                    var thread = Threads.Where(o => o.ThreadTitle == threadTitle).FirstOrDefault();
                    if (thread == null)
                    {
                        thread = new ThreadRealmObject
                        {
                            ThreadTitle = threadTitle,
                        };
                        Threads.Add(thread);
                    }

                    thread.ThreadModifiedDateTime = DateTimeOffset.Parse(liElement.QuerySelector(".stamp").TextContent);
                }
            });
        }
    }
}
