using Sport.Broadcast.Api.Contracts.Broadcasts.Requests;
using Sport.Broadcast.Api.Contracts.Broadcasts.Responses;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Enums;
using Sport.Broadcast.Api.Contracts.Broadcasts.SignalR.Requests;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Enums;
using Sport.Broadcast.AppServices.Contract.Broadcasts.Models;
using Sport.Broadcast.AppServices.Contract.Broadcasts.SignalR.Requests;
using Sport.Broadcast.Host.Infrastructure.MapService;
using Xunit;

namespace Sport.Broadcast.Host.UnitTests.Infrastructure.MapService
{
    public class MappingServiceTests
    {
        [Fact]
        public void MapToBroadcastRegistrationInternalRequest_MapSuccess_ReturnRightData()
        {
            //Arrange
            var request = new BroadcastRegistrationRequest()
            {
                HomeTeam = "Цска",
                GuestTeam = "Спартак",
                StartTime = new DateTime(2024, 01, 01, 12, 30, 00)
            };

            var expectedResult = new BroadcastInternalRequest()
            {
                HomeTeam = "Цска",
                GuestTeam = "Спартак",
                StartTime = new DateTime(2024, 01, 01, 12, 30, 00)
            };
            
            //Act
            var result = MappingService.MapToBroadcastRegistrationInternalRequest(request);
            
            //Assert
            Assert.Equal(expectedResult.HomeTeam, result.HomeTeam);
            Assert.Equal(expectedResult.GuestTeam, result.GuestTeam);
            Assert.Equal(expectedResult.StartTime, result.StartTime);
        }
        
        [Fact]
        public void MapToBroadcastResponse_MapSuccess_ReturnRightData()
        {
            //Arrange
            var request = new SportBroadcast()
            {
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08),
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 2,
                ExtraTime = 0,
                TotalExtraTime = 3,
                Status = BroadcastStatus.Started
            };

            var expectedResult = new BroadcastResponse()
            {
                HomeTeam = "Быки",
                GuestTeam = "Вороны",
                StartTime = new DateTime(2024, 08, 08),
                ScoreOfHomeTeam = 0,
                ScoreOfGuestTeam = 1,
                CurrentHalf = 2,
                ExtraTime = 0,
                TotalExtraTime = 3,
                Status = Api.Contracts.Broadcasts.Enums.BroadcastStatus.Started
            };
            
            //Act
            var result = MappingService.MapToBroadcastResponse(request);
            
            //Assert
            Assert.Equal(expectedResult.HomeTeam, result.HomeTeam);
            Assert.Equal(expectedResult.GuestTeam, result.GuestTeam);
            Assert.Equal(expectedResult.StartTime, result.StartTime);
            Assert.Equal(expectedResult.ScoreOfHomeTeam, result.ScoreOfHomeTeam);
            Assert.Equal(expectedResult.ScoreOfGuestTeam, result.ScoreOfGuestTeam);
            Assert.Equal(expectedResult.CurrentHalf, result.CurrentHalf);
            Assert.Equal(expectedResult.ExtraTime, result.ExtraTime);
            Assert.Equal(expectedResult.TotalExtraTime, result.TotalExtraTime);
            Assert.Equal(expectedResult.Status, result.Status);
        }
        
        [Fact]
        public void MapToBroadcastMessageInternalRequest_MapSuccess_ReturnRightData()
        {
            //Arrange
            var request = new MessageRequest()
            {
                Minute = "10",
                Event = EventInGame.Goal,
                ExtraTime = 2,
                HalfNumberAfterBreak = 2,
                PlayerName = "Андрей",
                Score = "2-1",
                Text = "Какой то текст"
            };
        
            var expectedResult = new MessageInternalRequest()
            {
                Minute = "10",
                Event = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.Goal,
                ExtraTime = 2,
                HalfNumberAfterBreak = 2,
                PlayerName = "Андрей",
                Score = "2-1",
                Text = "Какой то текст"
            };
            
            //Act
            var result = MappingService.MapToBroadcastMessageInternalRequest(request);
            
            //Assert
            //Assert
            Assert.Equal(expectedResult.Minute, result.Minute);
            Assert.Equal(expectedResult.Event, result.Event);
            Assert.Equal(expectedResult.ExtraTime, result.ExtraTime);
            Assert.Equal(expectedResult.HalfNumberAfterBreak, result.HalfNumberAfterBreak);
            Assert.Equal(expectedResult.PlayerName, result.PlayerName);
            Assert.Equal(expectedResult.Score, result.Score);
            Assert.Equal(expectedResult.Text, result.Text);
        }
        
        [Fact]
        public void MapToBroadcastResponses_MapSuccess_ReturnRightData()
        {
            //Arrange
            var request = new List<SportBroadcast>()
            {
                new()
                {
                    HomeTeam = "Быки",
                    GuestTeam = "Вороны",
                    StartTime = new DateTime(2024, 08, 08),
                    ScoreOfHomeTeam = 0,
                    ScoreOfGuestTeam = 1,
                    CurrentHalf = 2,
                    ExtraTime = 0,
                    TotalExtraTime = 3,
                    Status = BroadcastStatus.Started
                },
                new()
                {
                    HomeTeam = "Котлеты",
                    GuestTeam = "Сосиски",
                    StartTime = new DateTime(2024, 07, 07),
                    ScoreOfHomeTeam = 2,
                    ScoreOfGuestTeam = 2,
                    CurrentHalf = 1,
                    ExtraTime = 2,
                    TotalExtraTime = 0,
                    Status = BroadcastStatus.Started
                }
            };
        
            var expectedResult = new BroadcastResponse[]
            {
                new()
                {
                    HomeTeam = "Быки",
                    GuestTeam = "Вороны",
                    StartTime = new DateTime(2024, 08, 08),
                    ScoreOfHomeTeam = 0,
                    ScoreOfGuestTeam = 1,
                    CurrentHalf = 2,
                    ExtraTime = 0,
                    TotalExtraTime = 3,
                    Status = Api.Contracts.Broadcasts.Enums.BroadcastStatus.Started
                },
                new()
                {
                    HomeTeam = "Котлеты",
                    GuestTeam = "Сосиски",
                    StartTime = new DateTime(2024, 07, 07),
                    ScoreOfHomeTeam = 2,
                    ScoreOfGuestTeam = 2,
                    CurrentHalf = 1,
                    ExtraTime = 2,
                    TotalExtraTime = 0,
                    Status =  Api.Contracts.Broadcasts.Enums.BroadcastStatus.Started
                }
            };
            
            //Act
            var result = MappingService.MapToBroadcastResponses(request);
            
            //Assert
            for (int i = 0; i < expectedResult.Length; i++)
            {
                Assert.Equal(expectedResult[i].HomeTeam, result[i].HomeTeam);
                Assert.Equal(expectedResult[i].GuestTeam, result[i].GuestTeam);
                Assert.Equal(expectedResult[i].StartTime, result[i].StartTime);
                Assert.Equal(expectedResult[i].ScoreOfHomeTeam, result[i].ScoreOfHomeTeam);
                Assert.Equal(expectedResult[i].ScoreOfGuestTeam, result[i].ScoreOfGuestTeam);
                Assert.Equal(expectedResult[i].CurrentHalf, result[i].CurrentHalf);
                Assert.Equal(expectedResult[i].ExtraTime, result[i].ExtraTime);
                Assert.Equal(expectedResult[i].TotalExtraTime, result[i].TotalExtraTime);
                Assert.Equal(expectedResult[i].Status, result[i].Status);
            }
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventVarGoal_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.Goal;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.Goal;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MapToInternalEventInGame_EventVarGoalCancellation_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.VarGoalCancellation;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.VarGoalCancellation;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventSubstitution_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.Substitution;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.Substitution;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventYellowCard_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.YellowCard;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.YellowCard;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventSecondYellowCard_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.SecondYellowCard;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.SecondYellowCard;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventRedCard_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.RedCard;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.RedCard;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventBreak_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.Break;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.Break;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventExtraTime_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.ExtraTime;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.ExtraTime;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
        
        [Fact]
        public void MapToInternalEventInGame_EventBeginningHalf_ReturnRightData()
        {
            //Arrange
            var eventGoal = EventInGame.BeginningHalf;
            
            var expectedResult = AppServices.Contract.Broadcasts.SignalR.Enums.EventInGame.BeginningHalf;
            
            //Act
            var result = MappingService.MapToInternalEventInGame(eventGoal);
            
            //Assert
            Assert.Equal(expectedResult, result);
        }
    }
}