using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IHobbyService
    {
        #region Code from here

        void SaveHobbyForUser(string id, string[] hobby);


        #endregion

        void test();
    }

    public partial class HobbyService : IHobbyService
    {
        #region Code from here

        public void SaveHobbyForUser(string id, string[] hobbies)
        {
            foreach (var item in hobbies)
            {
                var hobby = new Hobby
                {
                    UserId = id,
                    SportId = Int32.Parse(item)
                };
                this.Create(hobby);
            }
        }

        #endregion

        public void test()
        {

        }
    }
}