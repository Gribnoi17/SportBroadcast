using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.DataAccess.Broadcasts.Models;

namespace Sport.Broadcast.DataAccess.UnitTests.Broadcasts.Data
{
    internal class BroadcastEntityTestData
    {
        private List<BroadcastEntity> _testSportBroadcastEntities;

        public List<BroadcastEntity> GetTestBroadcastEntities()
        {
            if (_testSportBroadcastEntities == null)
            {
                _testSportBroadcastEntities = InitializeBroadcastEntitiesTest();
            }

            return _testSportBroadcastEntities.ToList();
        }

        private List<BroadcastEntity> InitializeBroadcastEntitiesTest()
        {
            var broadcasts = new List<BroadcastEntity>();

            broadcasts.Add(new BroadcastEntity(){ HomeTeam = "Быки", GuestTeam = "Вороны", StartTime = new DateTime(2024, 08, 08, 12,00,00), Status = BroadcastStatus.Started, ScoreOfGuestTeam = 1, ScoreOfHomeTeam = 1, CurrentHalf = 1});
            broadcasts.Add(new BroadcastEntity(){ HomeTeam = "Кошки", GuestTeam = "Собачки", StartTime = new DateTime(2024, 12, 16, 15,30,00), Status = BroadcastStatus.Ended, ScoreOfGuestTeam = 2, ScoreOfHomeTeam = 0});
            broadcasts.Add(new BroadcastEntity(){ HomeTeam = "Рыбы", GuestTeam = "Киты", StartTime = new DateTime(2024, 09, 08, 11,45,00), Status = BroadcastStatus.NotStarted});
            broadcasts.Add(new BroadcastEntity(){ HomeTeam = "Тигры", GuestTeam = "Львы", StartTime = new DateTime(2024, 09, 08, 12,15,00), Status = BroadcastStatus.NotStarted});

            return broadcasts;
        }

    }
}