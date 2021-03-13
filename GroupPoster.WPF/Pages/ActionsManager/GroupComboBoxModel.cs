using GroupPoster.ApplicationLayer.Groups.Queries.GetGroups;

namespace GroupPoster.WPFUI.Pages.ActionsManager
{
    public class GroupComboBoxModel
    {
        public GroupComboBoxModel(GroupDto x)
        {
            Id = x.Id;
            Name = x.Name;
            Link = x.Link;
        }

        public int Id { get; }
        public string Name { get; }
        public string Link { get; }
    }
}
