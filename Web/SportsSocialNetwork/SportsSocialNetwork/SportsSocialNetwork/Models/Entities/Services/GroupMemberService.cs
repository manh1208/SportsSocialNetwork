using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IGroupMemberService
    {
        #region Code from here
        IEnumerable<GroupMember> GetMemberList(int groupId);

        bool JoinGroup(int groupId, String userId);

        bool LeaveGroup(int groupId, string userId);

        GroupMember KickMember(int id);

        GroupMember CreateGroupAdmin(int groupId, String userId);

        GroupMember SetGroupAdmin(int id, String userId);

        bool CheckAdmin(String userId, int groupId);

        bool CheckMember(String userId, int groupId);

        IEnumerable<GroupMember> GetJoinedList(String userId);
        #endregion


        void test();
    }
    public partial class GroupMemberService : IGroupMemberService
    {


        #region Code from here
        public bool CheckAdmin(string userId, int groupId)
        {
            GroupMember member = this.FirstOrDefaultActive(x => x.GroupId == groupId && x.UserId == userId);

            if (member != null && member.Admin == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMember(string userId, int groupId)
        {
            GroupMember member = this.FirstOrDefaultActive(x => x.GroupId == groupId && x.UserId == userId);

            if (member != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public GroupMember CreateGroupAdmin(int groupId, string userId)
        {
            GroupMember member = new GroupMember();

            member.GroupId = groupId;
            member.UserId = userId;
            member.Admin = true;
            member.Status = 1;
            member.Active = true;
            this.Create(member);
            return member;
        }

        public IEnumerable<GroupMember> GetMemberList(int groupId)
        {
            return this.GetActive(x => x.GroupId == groupId);
        }

        public bool JoinGroup(int groupId, string userId)
        {
            GroupMember member = this.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
            if (member != null)
            {
                member.Active = true;
                this.Update(member);
                return true;
            }
            else
            {
                member = new GroupMember();

                member.GroupId = groupId;
                member.UserId = userId;
                member.Admin = false;
                member.Status = 1;
                member.Active = true;
                this.Create(member);
                if (this.FirstOrDefaultActive(x => x.Id == member.Id) != null)
                {
                    return true;
                }
                return false;
            }
        }

        public bool LeaveGroup(int groupId, string userId) {
            GroupMember member = this.FirstOrDefaultActive(x => x.GroupId == groupId && x.UserId == userId);
            if (member != null)
            {
                member.Active = false;
                this.Update(member);
                return true;
            }
            return false;
        }

        public GroupMember KickMember(int id)
        {
            GroupMember member = FirstOrDefaultActive(x => x.Id == id);
            this.Deactivate(member);
            return member;
        }

        public GroupMember SetGroupAdmin(int id, String userId)
        {
            GroupMember member = this.FirstOrDefaultActive(x => x.Id == id && x.UserId == userId);

            member.Admin = true;

            this.Save();

            return member;
        }

        public IEnumerable<GroupMember> GetJoinedList(string userId)
        {
            return this.GetActive(x => x.UserId == userId);
        }



        #endregion

        public void test()
        {

        }


    }
}