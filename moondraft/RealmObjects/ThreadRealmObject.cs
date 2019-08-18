using moondraft.Pages;
using PropertyChanged;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

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
    }
}
