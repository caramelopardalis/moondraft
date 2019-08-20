using AngleSharp.Html.Parser;
using moondraft.Pages;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace moondraft.RealmObjects
{

    [DoNotNotify]
    public class ThreadRealmObject : RealmObject
    {
        public static readonly string ThreadUrl = "thread.cgi/{threadTitle}";

        [PrimaryKey]
        public string ThreadTitle { get; set; }

        public NodeRealmObject Node { get; set; }

        public DateTimeOffset ThreadModifiedDateTime { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        public IList<CommentRealmObject> Comments { get; }

        public ICommand OpenThreadCommand
        {
            get
            {
                return new Command(async (object parameter) =>
                {
                    var realm = Realm.GetInstance();
                    realm.Write(() =>
                    {
                        realm.All<SettingsRealmObject>().First().CurrentNode.CurrentThread = parameter as ThreadRealmObject;
                    });
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ThreadPage()));
                });
            }
        }

        public async Task<int> UpdateAsync(int pageNumber = 0)
        {
            var httpClient = new HttpClient();
            var url = Node.Url + ThreadUrl.Replace("{threadTitle}", ThreadTitle) + (pageNumber > 0 ? "/p" + pageNumber : "");
            var response = await httpClient.GetAsync(url);
            var document = await new HtmlParser().ParseDocumentAsync(await response.Content.ReadAsStringAsync());
            var dtElements = document.QuerySelectorAll("#records > dt");
            var ddElements = document.QuerySelectorAll("#records > dd");
            var pagingNumberAElements = document.QuerySelectorAll("[href=\"#top\"] ~ a");

            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                for (var i = 0; i < dtElements.Length; i++)
                {
                    var dtElement = dtElements[i];
                    var ddElement = ddElements[i];

                    var commentId = dtElement.GetAttribute("data-record-id");
                    var commentAuthorName = dtElement.QuerySelector(".name").TextContent;
                    var commentDateTime = dtElement.QuerySelector(".stamp").TextContent;
                    var commentBody = ddElement.TextContent.Length > 0 ? ddElement.TextContent.Substring(0, ddElement.TextContent.Length - "\n\n\n\n".Length) : "";

                    var comment = Comments.Where(o => o.CommentId == commentId).FirstOrDefault();
                    if (comment == null)
                    {
                        comment = new CommentRealmObject
                        {
                            CommentId = commentId,
                        };
                        Comments.Add(comment);
                    }
                    comment.CommentAuthorName = commentAuthorName;
                    comment.CommentDateTime = DateTimeOffset.Parse(commentDateTime);
                    comment.CommentBody = commentBody;
                }
            });

            int maxPageNumber = 0;
            foreach (var pagingNumberAElement in pagingNumberAElements)
            {
                if (Int32.TryParse(pagingNumberAElement.TextContent.Trim(), out pageNumber))
                {
                    maxPageNumber = maxPageNumber < pageNumber ? pageNumber : maxPageNumber;
                }
            }

            System.Diagnostics.Debug.WriteLine("New comments count: " + Comments.Count);

            return maxPageNumber;
        }
    }
}
