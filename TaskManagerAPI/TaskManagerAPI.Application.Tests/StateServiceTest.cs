using Moq;
using TaskManagerAPI.Application.Services;
using TaskManagerAPI.Domain.Entities;
using TaskManagerAPI.Infrastructure.Repositories.Interfaces;

namespace TaskManagerAPI.Application.Tests
{
    public class StateServiceTest
    {
        #region GetAll

        [Fact]
        public void GetAll_ReturnsList()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetAll()).Returns(new List<State>
            {
                new State
                {
                    Id = 1,
                    Name = "Open",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new State
                {
                    Id = 2,
                    Name = "In Progress",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new State
                {
                    Id = 3,
                    Name = "Closed",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            });
            var service = new StateService(repoMock.Object);

            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Equal(3, result.Results.Count);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetAll_ReturnsNull()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetAll()).Returns((List<State>)null!);
            var service = new StateService(repoMock.Object);

            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Null(result.Results);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetAll_ReturnsEmptyList()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetAll()).Returns(new List<State>());
            var service = new StateService(repoMock.Object);

            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Empty(result.Results);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetAll_ThrowsException()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetAll()).Throws(new Exception("Database error"));
            var service = new StateService(repoMock.Object);
            
            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.Null(result.Results);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database error", result.MessageExceptionTechnical);
        }

        #endregion

        #region GetById

        [Fact]
        public void GetById_ReturnsObject()
        {
            int id = 1;
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetById(id)).Returns(new State
            {
                Id = id,
                Name = "Open",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
            var service = new StateService(repoMock.Object);

            var result = service.GetById(id);

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.IsType<State>(result.Result);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetById_ReturnsNull()
        {
            int id = 1;
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetById(id)).Returns((State)null!);
            var service = new StateService(repoMock.Object);

            var result = service.GetById(id);

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Null(result.Result);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetById_ThrowsException()
        {
            int id = 1;
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetById(id)).Throws(new Exception("Database error"));
            var service = new StateService(repoMock.Object);

            var result = service.GetById(id);

            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.Null(result.Result);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database error", result.MessageExceptionTechnical);
        }

        #endregion

        #region GetBy

        [Fact]
        public void GetBy_ReturnsList()
        {
            Func<State, bool> func = e => e.Name == "Open";
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetBy(func)).Returns(new List<State>
            {
                new State
                {
                    Id = 1,
                    Name = "Open",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            });

            var service = new StateService(repoMock.Object);

            var result = service.GetBy(func);

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.NotEmpty(result.Results);
            Assert.IsType<List<State>>(result.Results);
            Assert.Equal(1, result.Results.Count);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetBy_ReturnsEmptyList()
        {
            Func<State, bool> func = e => e.Name == "Open";
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetBy(func)).Returns(new List<State>());

            var service = new StateService(repoMock.Object);

            var result = service.GetBy(func);

            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.IsType<List<State>>(result.Results);
            Assert.Empty(result.Results);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
        }

        [Fact]
        public void GetBy_ThrowsException()
        {
            Func<State, bool> func = e => e.Name == "Open";
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(e => e.GetBy(func)).Throws(new Exception("Database Error"));

            var service = new StateService(repoMock.Object);

            var result = service.GetBy(func);

            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.Null(result.Results);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database Error", result.MessageExceptionTechnical);
        }

        #endregion

        #region Add

        [Fact]
        public void Add_State_Success()
        {
            var repoMock = new Mock<IStateRepository>();
            var service = new StateService(repoMock.Object);
            var newState = new State
            {
                Name = "New State",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = service.Add(newState);
            
            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
            repoMock.Verify(r => r.Add(It.Is<State>(s => s == newState)), Times.Once);
        }

        [Fact]
        public void Add_State_ThrowsException()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(r => r.Add(It.IsAny<State>())).Throws(new Exception("Database error"));
            var service = new StateService(repoMock.Object);
            var newState = new State
            {
                Name = "New State",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = service.Add(newState);
            
            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database error", result.MessageExceptionTechnical);
            repoMock.Verify(r => r.Add(It.Is<State>(s => s == newState)), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public void Update_State_Success()
        {
            var repoMock = new Mock<IStateRepository>();
            var service = new StateService(repoMock.Object);
            var existingState = new State
            {
                Id = 1,
                Name = "Updated State",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = service.Update(existingState);
            
            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
            repoMock.Verify(r => r.Update(It.Is<State>(s => s == existingState)), Times.Once);
        }

        [Fact]
        public void Update_State_ThrowsException()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(r => r.Update(It.IsAny<State>())).Throws(new Exception("Database error"));
            var service = new StateService(repoMock.Object);
            var existingState = new State
            {
                Id = 1,
                Name = "Updated State",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };
            
            var result = service.Update(existingState);
            
            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database error", result.MessageExceptionTechnical);
            repoMock.Verify(r => r.Update(It.Is<State>(s => s == existingState)), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_State_Success()
        {
            var repoMock = new Mock<IStateRepository>();
            var service = new StateService(repoMock.Object);
            int stateId = 1;
            
            var result = service.Delete(stateId);
            
            Assert.NotNull(result);
            Assert.True(result.stateOperation);
            Assert.Null(result.MessageExceptionTechnical);
            Assert.Null(result.MessageExceptionUser);
            repoMock.Verify(r => r.Delete(It.Is<int>(id => id == stateId)), Times.Once);
        }

        [Fact]
        public void Delete_State_ThrowsException()
        {
            var repoMock = new Mock<IStateRepository>();
            repoMock.Setup(r => r.Delete(It.IsAny<int>())).Throws(new Exception("Database error"));
            var service = new StateService(repoMock.Object);
            int stateId = 1;
            
            var result = service.Delete(stateId);
            
            Assert.NotNull(result);
            Assert.False(result.stateOperation);
            Assert.NotNull(result.MessageExceptionTechnical);
            Assert.Contains("Database error", result.MessageExceptionTechnical);
            repoMock.Verify(r => r.Delete(It.Is<int>(id => id == stateId)), Times.Once);
        }

        #endregion
    }
}