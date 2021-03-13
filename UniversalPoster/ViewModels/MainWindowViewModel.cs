using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using ReactiveUI;
using UniversalPoster.ViewModels.UserControlsViewModels.AccountViewModels;
using UniversalPoster.ViewModels.UserControlsViewModels.GroupViewModels;
using UniversalPoster.ViewModels.UserControlsViewModels.OperationViewModels;
using UniversalPoster.ViewModels.UserControlsViewModels.PostViewModels;

namespace UniversalPoster.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase content;
        private readonly ISystemPersistence persistence;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainWindowViewModel(ISystemPersistence persistence)
        {
            this.persistence = persistence;
            OpenAccountView();
        }

        public ViewModelBase Content
        {
            get { return content; }
            set { this.RaiseAndSetIfChanged(ref content, value); }
        }

        public string Greeting => "Welcome to Avalonia!";

        public void OpenOperationView()
        {
            Content = new OperationViewModel(persistence, new SemaphoreSlim(1, 1));
        }

        public void OpenAccountView()
        {
            Content = new AccountViewModel(persistence, new SemaphoreSlim(1, 1));
        }

        public void OpenGroupView()
        {
            Content = new GroupViewModel(persistence, new SemaphoreSlim(1, 1));
        }

        public void OpenPostView()
        {
            Content = new PostViewModel(persistence, new SemaphoreSlim(1, 1));
        }
    }
}
