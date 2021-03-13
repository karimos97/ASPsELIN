using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Posts.Command.CreatePost;
using GroupPoster.ApplicationLayer.Posts.Command.DeletePost;
using GroupPoster.ApplicationLayer.Posts.Command.UpdatePost;
using GroupPoster.ApplicationLayer.Posts.Query.GetPosts;

namespace GroupPoster.WPFUI.Pages.PostManager
{
    /// <summary>
    /// Interaction logic for PostManagerPage.xaml
    /// </summary>
    public partial class PostManagerPage : Page
    {
        private readonly ObservableCollection<PostTableModel> TableCollection;
        private readonly ISystemPersistence persistence;
        private readonly SemaphoreSlim gate;

        public PostManagerPage(ISystemPersistence persistence)
        {
            this.TableCollection = new ObservableCollection<PostTableModel>();
            this.TableCollection.CollectionChanged += TableCollection_CollectionChanged;
            this.gate = new SemaphoreSlim(1, 1);
            this.persistence = persistence;

            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<PostDto> posts = await new GetPostsQueryHandler(persistence).Handle(new GetPostsQuery());

            foreach (PostDto post in posts)
            {
                TableCollection.Add(new PostTableModel(post));
            }


            this.PostTable.ItemsSource = this.TableCollection;
            this.PostTable.SelectionChanged += PostTable_SelectionChanged;
        }

        private void PostTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid grid)
            {
                PostTableModel model = (PostTableModel)grid.SelectedItem;

                if (model is null) return;

                Id_TextBox.Text = model.Id.ToString();
                Title_TextBox.Text = model.Title;
                Content_TextBox.Text = model.Content;
            }
        }

        private async void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is PostTableModel postModel)
            {
                UpdatePostCommand request = new()
                {
                    Id = postModel.Id,
                    Title = postModel.Title,
                    Content = postModel.Content
                };

                await new UpdatePostCommandHandler(persistence).Handle(request);
            }
        }

        private async void TableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                await gate.WaitAsync();
                await DeletePost(e.OldItems[0]);
                gate.Release();
            }
        }


        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyData())
            {
                CreatePostCommand request = GetData();
                int postId = await new CreatePostCommandHandler(persistence).Handle(request);
                AddToTable(new PostTableModel(postId, request));
                CleanInput();
                return;
            }

            MessageBox.Show("Provided Data is Invalid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyData())
            {
                UpdateOperation();
                return;
            }

            MessageBox.Show("Provided Data is Invalid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void UpdateOperation()
        {
            if (int.TryParse(Id_TextBox.Text, out int id))
            {
                PostTableModel model = TableCollection.FirstOrDefault(x => x.Id == id);
                UpdateModel(model);
            }
        }

        private bool VerifyData()
        {
            return !string.IsNullOrWhiteSpace(Title_TextBox.Text)
                && !string.IsNullOrWhiteSpace(Content_TextBox.Text);
        }

        private CreatePostCommand GetData() => new()
        {
            Title = Title_TextBox.Text,
            Content = Content_TextBox.Text
        };

        private void CleanInput()
        {
            Title_TextBox.Text = string.Empty;
            Content_TextBox.Text = string.Empty;
        }

        private void AddToTable(PostTableModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
            TableCollection.Add(model);
        }

        private void UpdateModel(PostTableModel model)
        {
            if (VerifyData())
            {
                model.Title = Title_TextBox.Text;
                model.Content = Content_TextBox.Text;
            }
        }
        
        private async Task DeletePost(object target)
        {
            if (target is PostTableModel postModel)
            {
                DeletePostCommand request = new()
                {
                    Id = postModel.Id
                };

                await new DeletePostCommandHandler(persistence).Handle(request);
            }
        }
    }
}
