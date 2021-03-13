using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Accounts.Queries.GetAccounts;
using GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup;
using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;
using GroupPoster.Domain.Enums;

namespace GroupPoster.WPFUI.Pages.GroupManager
{
    public class GroupTableModel : INotifyPropertyChanged
    {
        private string name;
        private string link;
        //private GroupStatus status;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupTableModel(int groupId, CreateGroupCommand command)
        {
            this.Id = groupId;
            this.Name = command.Name;
            this.Link = command.Link;
            //this.Status = Enum.Parse<GroupStatus>(command.Status);
        }

        public GroupTableModel(GroupDto account)
        {
            this.Id = account.Id;
            this.Name = account.Name;
            this.Link = account.Link;
            //this.Status = Enum.Parse<GroupStatus>(account.Status);
        }

        public int Id { get; }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }

        public string Link
        {
            get => link;
            set
            {
                link = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
        //public GroupStatus Status
        //{
        //    get => status;
        //    set
        //    {
        //        status = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        //    }
        //}

    }
}
