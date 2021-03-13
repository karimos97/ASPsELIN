using System.ComponentModel;
using System.Runtime.CompilerServices;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;
using Tmds.DBus;

namespace UniversalPoster.ViewModels.UserControlsViewModels.PostViewModels
{
    public class PostTableModel : INotifyPropertyChanged
    {
        private string content = string.Empty;
        private string title = string.Empty;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public PostTableModel(PostDto post)
        {
            this.Id = post.Id;
            this.Title = post.Title;
            this.Content = post.Content;
        }

        public PostTableModel(int postId, CreatePostCommand request)
        {
            this.Id = postId;
            this.content = request.Content;
            this.title = request.Title;
        }

        public int Id { get; set; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                InvokeChange();
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                InvokeChange();
            }
        }

        private void InvokeChange([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}