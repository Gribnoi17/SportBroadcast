using System.Collections;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.AppServices.UnitTests.Broadcasts.Data
{
    internal class SportBroadcastTestData : IEnumerable<object[]>
    {
        private readonly SportBroadcast _sportBroadcastOne;
        private readonly SportBroadcast _sportBroadcastTwo;
        private readonly SportBroadcast _sportBroadcastThree;

        public SportBroadcastTestData()
        {
            _sportBroadcastOne = new SportBroadcast()
            {
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08),
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 2, ExtraTime = 0,
                TotalExtraTime = 3,
                Status = BroadcastStatus.Started
            };
            
            _sportBroadcastTwo = new SportBroadcast()
            { 
                HomeTeam = "Кошки",
                GuestTeam = "Собачки",
                StartTime = new DateTime(2024, 12, 16),
                ScoreOfHomeTeam = 2,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 0,
                ExtraTime = 0,
                TotalExtraTime = 3,
                Status = BroadcastStatus.Ended
            };
            
            _sportBroadcastThree = new SportBroadcast()
            {
                HomeTeam = "Киты",
                GuestTeam = "Цска",
                StartTime = new DateTime(2024, 09, 08),
                Status = BroadcastStatus.NotStarted
            };
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { _sportBroadcastOne };
            yield return new object[] { _sportBroadcastTwo };
            yield return new object[] { _sportBroadcastThree };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public List<SportBroadcast> GetBroadcasts()
        {
            var broadcasts = new List<SportBroadcast>()
            {
                _sportBroadcastOne,
                _sportBroadcastTwo,
                _sportBroadcastThree
            };

            return broadcasts;
        }
    }
}