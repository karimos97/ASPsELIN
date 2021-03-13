using System.ComponentModel;
using System.Runtime.CompilerServices;
using GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;

namespace UniversalPoster.ViewModels.UserControlsViewModels.GroupViewModels
{
    public class GroupTableModel : INotifyPropertyChanged
    {
        private string name;
        private string link;
        public event PropertyChangedEventHandler? PropertyChanged;

        public GroupTableModel(int groupId, CreateGroupCommand request)
        {
            this.Id = groupId;
            this.name = request.Name;
            this.link = request.Link;
        }

        public GroupTableModel(GroupDto groupDto)
        {
            this.Id = groupDto.Id;
            this.name = groupDto.Name;
            this.link = groupDto.Link;
        }

        public int Id { get; set; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                InvokeChange();
            }
        }
        public string Link
        {
            get => link;
            set
            {
                link = value;
                InvokeChange();
            }
        }
        
        private void InvokeChange([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}