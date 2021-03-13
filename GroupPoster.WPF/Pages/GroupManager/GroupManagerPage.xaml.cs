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
using GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.DeleteGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.UpdateGroup;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using GroupPoster.Domain.Enums;
using GroupPoster.WPFUI.Pages.AccountManager;

namespace GroupPoster.WPFUI.Pages.GroupManager
{
    /// <summary>
    /// Interaction logic for GroupManagerPage.xaml
    /// </summary>
    public partial class GroupManagerPage : Page
    {
        private readonly ISystemPersistence persistence;
        private readonly SemaphoreSlim gate;

        public ObservableCollection<GroupTableModel> TableCollection { get; }

        public GroupManagerPage(ISystemPersistence persistence)
        {
            this.persistence = persistence;
            this.gate = new SemaphoreSlim(1, 1);
            this.TableCollection = new ObservableCollection<GroupTableModel>();
            this.TableCollection.CollectionChanged += TableCollection_CollectionChanged;
            InitializeComponent();
        }


        private async void GroupManagerPage_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<GroupDto> accounts = await new GetGroupsQueryHandler(persistence).Handle(new GetGroupsQuery());

            foreach (GroupDto account in accounts)
            {
                AddToGroupTable(new GroupTableModel(account));
            }

            this.GroupTable.ItemsSource = TableCollection;
        }

        private async void GroupTableModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is GroupTableModel groupModel)
            {
                UpdateGroupCommand request = new()
                {
                    Id = groupModel.Id,
                    Name = groupModel.Name,
                    Link = groupModel.Link
                };

                await new UpdatedGroupCommandHandler(persistence).Handle(request);
            }
        }

        private async void TableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                await gate.WaitAsync();
                await DeleteGroup(e.OldItems[0]);
                gate.Release();
            }
        }

        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                CreateGroupCommand command = GetData();
                int groupId = await new CreateGroupCommandHandler(persistence).Handle(command);
                AddToGroupTable(new GroupTableModel(groupId, command));
                ClearInput();
                return;
            }

            MessageBox.Show("Provided Data is Invalid", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private bool ValidateData()
        {
            return !string.IsNullOrWhiteSpace(Link_TextBox.Text)
                && !string.IsNullOrWhiteSpace(Name_TextBox.Text)
                /*&& Status_ComboBox.SelectedIndex != -1*/;
        }

        private CreateGroupCommand GetData()
        {
            return new()
            {
                Name = Name_TextBox.Text,
                Link = Link_TextBox.Text,
                //Status = ((GroupStatus)Status_ComboBox.SelectedItem).ToString()
            };
        }

        private void AddToGroupTable(GroupTableModel groupTableModel)
        {
            groupTableModel.PropertyChanged += GroupTableModel_PropertyChanged;
            TableCollection.Add(groupTableModel);
        }

        private void ClearInput()
        {
            Name_TextBox.Text = string.Empty;
            Link_TextBox.Text = string.Empty;
        }
        
        private async Task DeleteGroup(object target)
        {
            if (target is GroupTableModel groupModel)
            {
                DeleteGroupCommand request = new()
                {
                    Id = groupModel.Id
                };

                await new DeleteGroupCommandHandler(persistence).Handle(request);
            }
        }
    }
}
