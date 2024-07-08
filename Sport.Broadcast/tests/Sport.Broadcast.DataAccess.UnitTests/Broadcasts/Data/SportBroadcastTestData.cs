using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;

namespace Sport.Broadcast.DataAccess.UnitTests.Broadcasts.Data
{
    internal class SportBroadcastTestData
    {
        private List<SportBroadcast> _testSportBroadcasts;

        public List<SportBroadcast> GetTestBroadcasts()
        {
            if (_testSportBroadcasts == null)
            {
                _testSportBroadcasts = InitializeBroadcastsTest();
            }

            return _testSportBroadcasts.ToList();
        }

        private List<SportBroadcast> InitializeBroadcastsTest()
        {
            var broadcasts = new List<SportBroadcast>();

            broadcasts.Add(new SportBroadcast(){ HomeTeam = "Быки", GuestTeam = "Вороны", StartTime = new DateTime(2024, 08, 08, 12,00,00)});
            broadcasts.Add(new SportBroadcast(){ HomeTeam = "Кошки", GuestTeam = "Собачки", StartTime = new DateTime(2024, 12, 16, 15,30,00)});
            broadcasts.Add(new SportBroadcast(){ HomeTeam = "Рыбы", GuestTeam = "Киты", StartTime = new DateTime(2024, 09, 08, 11,45,00)});
            broadcasts.Add(new SportBroadcast(){ HomeTeam = "Тигры", GuestTeam = "Львы", StartTime = new DateTime(2024, 09, 08, 12,15,00)});

            return broadcasts;
        }
    }
}