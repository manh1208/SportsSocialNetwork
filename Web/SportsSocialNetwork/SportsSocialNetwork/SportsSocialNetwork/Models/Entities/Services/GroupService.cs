using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IGroupService
    {
        #region Code from here
        IEnumerable<Group> FindGroup(String name, int skip, int take);

        Group FindGroupById(int id);

        Group CreateGroup(Group model);

        Group UpdateGroup(Group model);

        String ChangeCoverImage(int id,String image);

        bool DeleteGroup(int id);

        List<Group> GetSuggestGroup(int groupId);

        #endregion

        void test();
    }
    public partial class GroupService : IGroupService
    {
        #region Code from here
        public IEnumerable<Group> FindGroup(String name, int skip, int take)
        {
            IEnumerable<Group> result = null;

            result = this.GetActive(x => x.Name.Contains(name)).OrderBy(x=> x.Name).Skip(skip).Take(take);

            return result;
        }


        public Group FindGroupById(int id)
        {
            return this.FirstOrDefaultActive(x => x.Id == id);
        }

        public Group CreateGroup(Group model)
        {
            model.Active = true;
            this.Create(model);
            return model;
        }

        public Group UpdateGroup(Group model)
        {
            Group group = FirstOrDefaultActive(x => x.Id == model.Id);
            group.Name = model.Name;
            group.Description = model.Description;
            group.SportId = model.SportId;
            this.Update(group);
            return group;
        }

        public String ChangeCoverImage(int id,String image) {
            Group group = this.FindGroupById(id);
            group.CoverImage = image;
            Save();
            return group.CoverImage;
        }

        public bool DeleteGroup(int id)
        {
            Group group = this.FindGroupById(id);
            if(group != null)
            {
                this.Deactivate(group);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public void test()
        {

        }

        public List<Group> GetSuggestGroup(int groupId)
        {
            Group curGroup = this.FindGroupById(groupId);

            List<Group> suggestGroupList = this.GetActive(g => g.SportId == curGroup.SportId).ToList();
            return suggestGroupList;
        }
    }
}