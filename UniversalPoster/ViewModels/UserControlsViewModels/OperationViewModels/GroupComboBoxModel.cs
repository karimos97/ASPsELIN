using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;

namespace UniversalPoster.ViewModels.UserControlsViewModels.OperationViewModels
{
    public class GroupComboBoxModel
    {
        public GroupComboBoxModel(GroupDto x)
        {
            Name = x.Name;
            Link = x.Link;
        }

        public string Name { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}