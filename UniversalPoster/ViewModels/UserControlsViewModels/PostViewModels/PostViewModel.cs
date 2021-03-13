using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.ApplicationLayer.Posts.Command.DeletePost;
using GroupPoster.ApplicationLayer.Posts.Command.UpdatePost;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;
using ReactiveUI;

namespace UniversalPoster.ViewModels.UserControlsViewModels.PostViewModels
{
    public class PostViewModel : ViewModelBase
    {
        private ISystemPersistence persistence;
        private SemaphoreSlim gate;
        private string title = string.Empty;
        private string content = string.Empty;
        private string error = string.Empty;

        public PostViewModel(ISystemPersistence persistence, SemaphoreSlim gate)
        {
            this.persistence = persistence;
            this.gate = gate;
            Load();
        }

        public ObservableCollection<PostTableModel> Posts { get; set; } = new();

        public string Title
        {
            get => title; set
            {
                this.RaiseAndSetIfChanged(ref title, value);
            }
        }

        public string Content
        {
            get => content;
            set
            {
                this.RaiseAndSetIfChanged(ref content, value);
            }
        }
        
        public string Error
        {
            get => error;
            set
            {
                this.RaiseAndSetIfChanged(ref error, value);
            }
        }

        private async void Load()
        {
            var posts = await new GetPostsQueryHandler(persistence).Handle(new GetPostsQuery());

            foreach (var post in posts)
            {
                AddPostToCollection(new(post));
            }
        }

        private async void PostModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is PostTableModel post)
            {
                var request = new UpdatePostCommand
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content
                };

                await new UpdatePostCommandHandler(persistence).Handle(request);
            }
        }

        private void AddPostToCollection(PostTableModel postModel)
        {
            postModel.PropertyChanged += PostModel_PropertyChanged;
            Posts.Add(postModel);
        }

        private bool ValidateData()
        {
            return !(string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Content));
        }

        private CreatePostCommand GetData()
        {
            return new()
            {
                Title = this.Title,
                Content = this.Content
            };
        }

        private void ClearData()
        {
            Title = string.Empty;
            Content = string.Empty;
            Error = string.Empty;
        }

        public async void AddPost()
        {
            if (ValidateData())
            {
                var request = GetData();
                int postId = await new CreatePostCommandHandler(persistence).Handle(request);
                AddPostToCollection(new(postId, request));
                ClearData();
            }
            else
            {
                Error = "All Fields Must Be Filled!";
            }
        }

        public async void DeleteRow(int id)
        {
            var request = new DeletePostCommand { Id = id };

            await gate.WaitAsync();
            await new DeletePostCommandHandler(persistence).Handle(request);
            gate.Release();

            Posts.Remove(Posts.First(x => x.Id == id));
        }
    }
}
