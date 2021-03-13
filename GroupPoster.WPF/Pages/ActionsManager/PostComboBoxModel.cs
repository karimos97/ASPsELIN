using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;

namespace GroupPoster.WPFUI.Pages.ActionsManager
{
    public class PostComboBoxModel
    {
        public PostComboBoxModel(PostDto x)
        {
            this.Id = x.Id;
            this.Title = x.Title;
            this.Content = x.Content;
        }

        public int Id { get; }
        public string Title { get; }
        public string Content { get; }
    }
}
