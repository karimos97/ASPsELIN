using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.DeleteGroup;
using GroupPoster.ApplicationLayer.Groups.Commnand.UpdateGroup;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using ReactiveUI;

namespace UniversalPoster.ViewModels.UserControlsViewModels.GroupViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        private ISystemPersistence persistence;
        private string link = string.Empty;
        private string name = string.Empty;
        private string error = string.Empty;
        private SemaphoreSlim gate;

        public GroupViewModel(ISystemPersistence persistence, SemaphoreSlim semaphoreSlim)
        {
            this.persistence = persistence;
            this.gate = semaphoreSlim;
            Load();
        }

        public string Name
        {
            get => name;
            set
            {
                this.RaiseAndSetIfChanged(ref name, value);
            }
        }

        public string Link
        {
            get => link;
            set
            {
                this.RaiseAndSetIfChanged(ref link, value);

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
            var groupDtos = await new GetGroupsQueryHandler(persistence).Handle(new());

            foreach (var groupDto in groupDtos)
            {
                AddGroupToCollection(new(groupDto));
            }
        }

        public ObservableCollection<GroupTableModel> Groups { get; set; } = new ObservableCollection<GroupTableModel>();

        private async void GroupTableModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
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

        public async void AddGroup()
        {
            if (ValidateData())
            {
                var request = GetData();
                int groupId = await new CreateGroupCommandHandler(persistence).Handle(request);
                AddGroupToCollection(new GroupTableModel(groupId, request));
                ClearData();
            }
            else
            {
                Error = "All Fields Must Be Filled!";
            }
        }

        public bool ValidateData()
        {
            return !(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Link));
        }

        private CreateGroupCommand GetData()
        {
            return new CreateGroupCommand() { Link = link, Name = name };
        }

        private void ClearData()
        {
            Name = string.Empty;
            Link = string.Empty;
            Error = string.Empty;
        }

        public async void DeleteRow(int id)
        {
            var request = new DeleteGroupCommand { Id = id };

            await gate.WaitAsync();
            await new DeleteGroupCommandHandler(persistence).Handle(request);
            gate.Release();

            Groups.Remove(Groups.First(x => x.Id == id));
        }

        private void AddGroupToCollection(GroupTableModel groupModel)
        {
            groupModel.PropertyChanged += GroupTableModel_PropertyChanged;
            Groups.Add(groupModel);
        }
    }
}
