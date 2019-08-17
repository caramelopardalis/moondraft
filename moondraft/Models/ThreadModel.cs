using AngleSharp.Html.Parser;
using moondraft.Constants;
using moondraft.RealmObjects;
using Realms;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace moondraft.Models
{
    public class ThreadModel
    {
        public static async Task UpdateThreads(NodeRealmObject node)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(node.Url + ApiConstant.Recent);
            var document = await new HtmlParser().ParseDocumentAsync(await response.Content.ReadAsStringAsync());
            var liElements = document.QuerySelectorAll("#thread_index > li");
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                foreach (var liElement in liElements)
                {
                    var threadTitle = liElement.QuerySelector("a").TextContent;
                    var thread = node.Threads.Where(o => o.ThreadTitle == threadTitle).FirstOrDefault();
                    if (thread == null)
                    {
                        thread = new ThreadRealmObject
                        {
                            ThreadTitle = threadTitle,
                        };
                        node.Threads.Add(thread);
                    }

                    thread.ThreadModifiedDateTime = DateTimeOffset.Parse(liElement.QuerySelector(".stamp").TextContent);
                }
            });
        }
    }
}
