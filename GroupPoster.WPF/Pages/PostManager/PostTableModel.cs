using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;

namespace GroupPoster.WPFUI.Pages.PostManager
{
    public class PostTableModel : INotifyPropertyChanged
    {
        private string title;
        private string content;

        public event PropertyChangedEventHandler PropertyChanged;

        public PostTableModel(int id, CreatePostCommand request)
        {
            this.Id = id;
            this.title = request.Title;
            this.content = request.Content;
        }

        public PostTableModel(PostDto post)
        {
            this.Id = post.Id;
            this.title = post.Title;
            this.content = post.Content;
        }

        public int Id { get; }
        public string Title
        {
            get => title; 
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
        public string Content
        {
            get => content; 
            set
            {
                content = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
    }
}
