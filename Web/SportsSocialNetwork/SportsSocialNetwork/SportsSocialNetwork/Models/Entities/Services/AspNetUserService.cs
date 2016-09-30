using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IAspNetUserService
    {
        #region Code from here

        String FindUserName(String userId);

        #endregion 

        void test();
    }
    public partial class AspNetUserService:IAspNetUserService
    {
        #region Code from here

        public String FindUserName(String userId) {
            return this.FirstOrDefault(x => x.Id == userId).UserName;
        }

        #endregion

        public void test()
        {

        }
    }
}