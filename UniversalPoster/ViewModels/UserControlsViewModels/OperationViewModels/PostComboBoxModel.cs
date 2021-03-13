using System.Xml.Linq;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;

namespace UniversalPoster.ViewModels.UserControlsViewModels.OperationViewModels
{
    public class PostComboBoxModel
    {
        public PostComboBoxModel(PostDto x)
        {
            Title = x.Title;
            Content = x.Content;
        }

        public string Title { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}