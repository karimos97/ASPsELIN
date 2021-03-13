using System;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Posts.Query.GetPosts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public static PostDto From(Post post) => new()
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content
        };
    }
}