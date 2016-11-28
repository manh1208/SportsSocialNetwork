using SportsSocialNetwork.Models.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IGroupMemberService
    {
        #region Code from here
        GroupMember CreateGroupMember(GroupMember groupMember);

        IEnumerable<GroupMember> GetMemberList(int groupId);

        bool JoinGroup(int groupId, String userId);

        bool LeaveGroup(int groupId, string userId);

        GroupMember KickMember(int id);

        GroupMember CreateGroupAdmin(int groupId, String userId);

        GroupMember SetGroupAdmin(int id);

        bool CheckAdmin(String userId, int groupId);

        bool CheckMember(String userId, int groupId);

        IEnumerable<GroupMember> GetJoinedList(String userId);

        int JoinLeaveGroup(string userId, int groupId);

        bool isOnlyOneAdmin(string userId, int groupId);

        GroupMemberRole CheckRoleMember(string userId, int groupId);

        GroupMember ApproveMember(string userId);

        bool JoinGroupByAdmin(int groupId, string userId);
        #endregion


        void test();
    }
    public partial class GroupMemberService : IGroupMemberService
    {


        #region Code from here
        public GroupMember CreateGroupMember(GroupMember groupMember)
        {
            this.Create(groupMember);
            return groupMember;
        }

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
                member.Status = (int)GroupMemberStatus.Pending;
                member.Admin = false;
                this.Update(member);
                this.Save();
                return true;
            }
            else
            {
                member = new GroupMember();

                member.GroupId = groupId;
                member.UserId = userId;
                member.Admin = false;
                member.Status = (int)GroupMemberStatus.Pending;
                member.Active = true;
                this.Create(member);
                this.Save();
                if (this.FirstOrDefaultActive(x => x.Id == member.Id) != null)
                {
                    return true;
                }
                return false;
            }
        }

        public bool JoinGroupByAdmin(int groupId, string userId)
        {
            GroupMember member = this.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);
            if (member != null)
            {
                member.Active = true;
                member.Status = (int)GroupMemberStatus.Approved;
                member.Admin = false;
                this.Update(member);
                this.Save();
                return true;
            }
            else
            {
                member = new GroupMember();

                member.GroupId = groupId;
                member.UserId = userId;
                member.Admin = false;
                member.Status = (int)GroupMemberStatus.Approved;
                member.Active = true;
                this.Create(member);
                this.Save();
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

        public GroupMember SetGroupAdmin(int id)
        {
            GroupMember member = this.FirstOrDefaultActive(x => x.Id == id);

            member.Admin = true;

            this.Save();

            return member;
        }

        public IEnumerable<GroupMember> GetJoinedList(string userId)
        {
            return this.GetActive(x => x.UserId == userId);
        }

        public int JoinLeaveGroup(string userId, int groupId)
        {
            int result = -1;
            GroupMember gm = this.FirstOrDefault(m => m.UserId.Equals(userId) && m.GroupId == groupId);

            if(gm == null)
            {
                gm = new GroupMember();
                gm.UserId = userId;
                gm.GroupId = groupId;
                gm.Admin = false;
                gm.Status = (int)GroupMemberStatus.Pending;
                this.Create(gm);
                this.Save();
                result = (int)JoinLeaveGroupResult.RequestSent;
            }
            else
            {
                if(gm.Active == false)
                {
                    gm.Status = (int)GroupMemberStatus.Pending;
                    gm.Admin = false;
                    this.Activate(gm);
                    result = (int)JoinLeaveGroupResult.ReJoined;
                }
                else
                {
                    if (gm.Status == (int)GroupMemberStatus.Pending)
                    {
                        this.Delete(gm);
                        result = (int)JoinLeaveGroupResult.CancelRequest;
                    }
                    else if (gm.Status == (int)GroupMemberStatus.Approved)
                    {
                        if (gm.Admin == false)
                        {
                            gm.Status = (int)GroupMemberStatus.Pending;
                            gm.Admin = false;
                            this.Deactivate(gm);
                            result = (int)JoinLeaveGroupResult.Leaved;
                        }
                        else
                        {
                            if (this.isOnlyOneAdmin(userId, groupId) == true)
                            {
                                result = (int)JoinLeaveGroupResult.CannotLeave;
                            }
                            else
                            {
                                gm.Status = (int)GroupMemberStatus.Pending;
                                gm.Admin = false;
                                this.Deactivate(gm);
                                result = (int)JoinLeaveGroupResult.Leaved;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool isOnlyOneAdmin(string userId, int groupId)
        {
            List<GroupMember> gml = this.GetActive(m => m.GroupId == groupId && m.Admin == true).ToList();
            foreach (var item in gml)
            {
                if(gml.Count > 1 && userId.Equals(item.UserId))
                {
                    return false;
                }
            }
            return true;
        }

        public GroupMemberRole CheckRoleMember(string userId, int groupId)
        {
            GroupMemberRole result = GroupMemberRole.NotMember;
            GroupMember gm = this.FirstOrDefaultActive(m => m.UserId.Equals(userId) && m.GroupId == groupId);
            if(gm == null)
            {
                result = GroupMemberRole.NotMember;
            }
            else
            {
                if(gm.Status == (int)GroupMemberStatus.Pending)
                {
                    result = GroupMemberRole.PendingMember;
                }
                else if (gm.Status == (int)GroupMemberStatus.Approved)
                {
                    if(gm.Admin == true)
                    {
                        result = GroupMemberRole.Admin;
                    }
                    else
                    {
                        result = GroupMemberRole.Member;
                    }
                }
            }
            return result;
        }

        public GroupMember ApproveMember(string userId)
        {
            GroupMember gm = this.FirstOrDefaultActive(g => g.UserId == userId && g.Status == (int)GroupMemberStatus.Pending);
            if(gm != null)
            {
                gm.Status = (int)GroupMemberStatus.Approved;
                this.Update(gm);
                this.Save();
                return gm;
            }
            else
            {
                return null;
            }

        }
        #endregion

        public void test()
        {

        }


    }
}