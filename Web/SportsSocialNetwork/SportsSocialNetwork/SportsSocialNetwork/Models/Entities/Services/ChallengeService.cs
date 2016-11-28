using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsSocialNetwork.Models.Enumerable;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IChallengeService
    {
        #region Code from here

        Challenge CreateChallengeRequest(int fromGroup, int toGroup, string description);

        IEnumerable<Challenge> GetAllChallengeRequest(int groupId);

        IEnumerable<Challenge> GetChallengedList(int groupId);

        IEnumerable<Challenge> GetNotOperateChallengeList(int groupId);

        IEnumerable<Challenge> GetSentChallengeRequest(int groupId);

        bool UpdateChallenge(int challengeId, int status);

        Challenge FindById(int id);
        #endregion

    }
    public partial class ChallengeService : IChallengeService
    {
        #region Code from here

        public Challenge CreateChallengeRequest(int fromGroup, int toGroup, string description)
        {
            Challenge challenge = new Challenge();
            challenge.FromGroup = fromGroup;
            challenge.ToGroup = toGroup;
            challenge.Description = description;
            challenge.Accepted = false;
            challenge.Status = (int)ChallengeStatus.Pending;

            this.Create(challenge);

            return challenge;
        }

        public IEnumerable<Challenge> GetAllChallengeRequest(int groupId)
        {
            return this.GetActive(c => c.ToGroup == groupId && c.Accepted == false && c.Status == (int)ChallengeStatus.Pending).OrderByDescending(c => c.Id);
        }

        public IEnumerable<Challenge> GetChallengedList(int groupId)
        {
            return this.GetActive(c => (c.ToGroup == groupId || c.FromGroup == groupId) && c.Accepted == true && c.Status == (int)ChallengeStatus.Done);
        }

        public IEnumerable<Challenge> GetNotOperateChallengeList(int groupId)
        {
            return this.GetActive(c => (c.ToGroup == groupId || c.FromGroup == groupId) && c.Accepted == true && c.Status == (int)ChallengeStatus.NotOperate);
        }

        public IEnumerable<Challenge> GetSentChallengeRequest(int groupId)
        {
            return this.GetActive(c => c.FromGroup == groupId && c.Accepted == false && c.Status == (int)ChallengeStatus.Pending).OrderByDescending(c => c.Id);
        }

        public bool UpdateChallenge(int challengeId, int status)
        {
            bool result = false;
            Challenge cha = this.FirstOrDefaultActive(c => c.Id == challengeId);
            if (cha != null)
            {
                switch(status)
                {
                    case (int)ChallengeStatus.NotAvailable:
                        cha.Status = (int)ChallengeStatus.NotAvailable;
                        this.Update(cha);
                        this.Save();
                        result = true;
                        break;

                    case (int)ChallengeStatus.Done:
                        cha.Status = (int)ChallengeStatus.Done;
                        this.Update(cha);
                        this.Save();
                        result = true;
                        break;

                    case (int)ChallengeStatus.NotOperate:
                        cha.Accepted = true;
                        cha.Status = (int)ChallengeStatus.NotOperate;
                        this.Update(cha);
                        this.Save();
                        result = true;
                        break;

                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        public Challenge FindById(int id)
        {
            Challenge cha = this.FirstOrDefaultActive(c => c.Id == id);
            return cha;
        }

        #endregion
    }
}