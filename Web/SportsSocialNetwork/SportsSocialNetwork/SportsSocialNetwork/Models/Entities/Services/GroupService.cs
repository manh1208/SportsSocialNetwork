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


        #endregion

        public void test()
        {

        }
    }
}