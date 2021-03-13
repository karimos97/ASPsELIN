using System;
using System.Threading;
using System.Windows;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Infrastructure.DataAccess;
using GroupPoster.Infrastructure.DataAccess.DataInteractor;
using GroupPoster.Infrastructure.BrowserAccess;
using GroupPoster.WPFUI.Pages.AccountManager;
using GroupPoster.WPFUI.Pages.ActionsManager;
using GroupPoster.WPFUI.Pages.GroupManager;
using GroupPoster.WPFUI.Pages.PostManager;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;

namespace GroupPoster.WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ISystemPersistence persistence;

        public MainWindow()
        {
            SemaphoreSlim semaphore = new(1, 1);
            FileInteractor context = new(semaphore);
            persistence = new SystemPersistence(context);

            InitializeComponent();
            AddAccounts_Click(this, new RoutedEventArgs());
        }

        private void AddAccounts_Click(object sender, RoutedEventArgs e)
        {
            if (PageAlreadyOpened<AccountManagerPage>()) return;
            AssignPage(new AccountManagerPage(persistence));
        }

        private void AddGroups_Click(object sender, RoutedEventArgs e)
        {
            if (PageAlreadyOpened<GroupManagerPage>()) return;
            AssignPage(new GroupManagerPage(persistence));
        }

        private void AddPosts_Click(object sender, RoutedEventArgs e)
        {
            if (PageAlreadyOpened<PostManagerPage>()) return;
            AssignPage(new PostManagerPage(persistence));
        }

        private void Actions_Click(object sender, RoutedEventArgs e)
        {
            if (PageAlreadyOpened<ActionsManagerPage>()) return;
               AssignPage(new ActionsManagerPage(persistence));
        }

        private bool PageAlreadyOpened<T>()
        {
            var page = this.MainPage.Content;
            return page != null && page.GetType() == typeof(T);
        }

        private void AssignPage(object page)
        {
            this.MainPage.Content = page;
        }

        private void ShutDown_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
