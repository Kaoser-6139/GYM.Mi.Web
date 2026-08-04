using Autofac.Extras.Moq;
using GYM.Application.Services;
using GYM.Domain;
using GYM.Domain.Dtos;
using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using GYM.Domain.Services;
using GYM.Mi.Domain;
using Moq;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace GYM.Application.Tests
{
    [ExcludeFromCodeCoverage]
    public class Tests
    {
        private AutoMock _moq;
        private IUserService _userService;
        private Mock<IApplicationUnitOfWork> _applicationUnitOfWorkMock;
        private Mock<IUserRepository> _userRepositoryMock;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
           _moq =AutoMock.GetLoose();
            
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _moq.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _userService = _moq.Create<UserService>();
            _applicationUnitOfWorkMock = _moq.Mock<IApplicationUnitOfWork>();
            _userRepositoryMock = _moq.Mock<IUserRepository>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationUnitOfWorkMock.Reset();
            _userRepositoryMock.Reset();
        }

        [Test]
        public void AssignToTrainer_WhenUserExists_AssignsTrainerAndSaves()
        {
            var userId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var user = new User
            {
                Id = userId
            };

            _applicationUnitOfWorkMock.SetupGet(x => x.UserRepository)
                                      .Returns(_userRepositoryMock.Object);

            _userRepositoryMock.Setup(x => x.GetById(userId))
                               .Returns(user)
                               .Verifiable();
            _userRepositoryMock.Setup(x => x.Update(user))
                               .Verifiable();

            _applicationUnitOfWorkMock.Setup(x => x.Save())
                                      .Verifiable();
            // Act
            _userService.AssignToTrainer(userId, trainerId);


            //Assert
            user.TrainerEmployeeId.ShouldBe(trainerId);
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void AssignToTrainer_WhenUserDoesNotExist_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .Returns((User?)null)
                .Verifiable();

            // Act & Assert
            Should.Throw<Exception>(
                () => _userService.AssignToTrainer(userId, trainerId)
            );

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }
        [Test]
        public void UnassignFromTrainer_WhenUserExists_RemovesTrainerAndSaves()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var trainerId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                TrainerEmployeeId = trainerId
            };

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .Returns(user)
                .Verifiable();

            _userRepositoryMock
                .Setup(x => x.Update(user))
                .Verifiable();

            _applicationUnitOfWorkMock
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            _userService.UnassignFromTrainer(userId);

            // Assert
            user.TrainerEmployeeId.ShouldBeNull();

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void UnassignFromTrainer_WhenUserDoesNotExist_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .Returns((User?)null)
                .Verifiable();

            // Act & Assert
            Should.Throw<Exception>(
                () => _userService.UnassignFromTrainer(userId)
            );

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void GetAvailableUsers_WhenOrderIsNull_UsesDefaultOrder()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            string? order = null;
            var search = new DataTablesSearch();

            var users = new List<User>();

            var expectedResult = (
                data: (IList<User>)users,
                total: 0,
                totalDisplay: 0
            );

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetPagedUnassignedUsers(
                    pageIndex,
                    pageSize,
                    "FullName asc",
                    search))
                .Returns(expectedResult)
                .Verifiable();

            // Act
            var result = _userService.GetAvailableUsers(
                pageIndex,
                pageSize,
                order,
                search
            );

            // Assert
            result.ShouldBe(expectedResult);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void GetAvailableUsers_WhenOrderIsProvided_UsesProvidedOrder()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var order = "Email desc";
            var search = new DataTablesSearch();

            var users = new List<User>();

            var expectedResult = (
                data: (IList<User>)users,
                total: 0,
                totalDisplay: 0
            );

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetPagedUnassignedUsers(
                    pageIndex,
                    pageSize,
                    order,
                    search))
                .Returns(expectedResult)
                .Verifiable();

            // Act
            var result = _userService.GetAvailableUsers(
                pageIndex,
                pageSize,
                order,
                search
            );

            // Assert
            result.ShouldBe(expectedResult);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }
        [Test]
        public void AddUser_WhenCalled_AddsUserAndSaves()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid()
            };

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.Add(user))
                .Verifiable();

            _applicationUnitOfWorkMock
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            _userService.AddUser(user);

            // Assert
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void Update_WhenCalled_UpdatesUserAndSaves()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid()
            };

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.Update(user))
                .Verifiable();

            _applicationUnitOfWorkMock
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            _userService.Update(user);

            // Assert
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void DeleteUser_WhenCalled_RemovesUserAndSaves()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.Remove(userId))
                .Verifiable();

            _applicationUnitOfWorkMock
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            _userService.DeleteUser(userId);

            // Assert
            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public void GetUser_WhenUserExists_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var expectedUser = new User
            {
                Id = userId
            };

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .Returns(expectedUser)
                .Verifiable();

            // Act
            var actualUser = _userService.GetUser(userId);

            // Assert
            actualUser.ShouldBe(expectedUser);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }
        [Test]
        public void GetUser_WhenUserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetById(userId))
                .Returns((User?)null)
                .Verifiable();

            // Act
            var actualUser = _userService.GetUser(userId);

            // Assert
            actualUser.ShouldBeNull();

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }
        //[Test]
        //public void GetTotalUsersCount_WhenCalled_ReturnsRepositoryCount()
        //{
        //    // Arrange
        //    var expectedCount = 25;

        //    _applicationUnitOfWorkMock
        //        .SetupGet(x => x.UserRepository)
        //        .Returns(_userRepositoryMock.Object);

        //    _userRepositoryMock
        //        .Setup(x => x.GetCount())
        //        .Returns(expectedCount)
        //        .Verifiable();

        //    // Act
        //    var actualCount = _userService.GetTotalUsersCount();

        //    // Assert
        //    actualCount.ShouldBe(expectedCount);

        //    this.ShouldSatisfyAllConditions(
        //        _applicationUnitOfWorkMock.VerifyAll,
        //        _userRepositoryMock.VerifyAll
        //    );
        //}

        [Test]
        public void GetUsers_WhenCalled_ReturnsPagedUsers()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            string? order = "FullName asc";
            var search = new DataTablesSearch();

            IList<User> expectedUsers = new List<User>
    {
        new User { Id = Guid.NewGuid() },
        new User { Id = Guid.NewGuid() }
    };

            var expectedResult = (
                data: expectedUsers,
                total: 2,
                totalDisplay: 2
            );

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetPagedusers(
                    pageIndex,
                    pageSize,
                    order,
                    search))
                .Returns(expectedResult)
                .Verifiable();

            // Act
            var actualResult = _userService.GetUsers(
                pageIndex,
                pageSize,
                order,
                search
            );

            // Assert
            actualResult.ShouldBe(expectedResult);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }
        [Test]
        public void GetAssignedUsers_WhenCalled_ReturnsTrainerUsers()
        {
            // Arrange
            var trainerId = Guid.NewGuid();

            var expectedUsers = new List<User>
    {
        new User
        {
            Id = Guid.NewGuid(),
            TrainerEmployeeId = trainerId
        },
        new User
        {
            Id = Guid.NewGuid(),
            TrainerEmployeeId = trainerId
        }
    };

            _applicationUnitOfWorkMock
                .SetupGet(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userRepositoryMock
                .Setup(x => x.GetByTrainerId(trainerId))
                .Returns(expectedUsers)
                .Verifiable();

            // Act
            var actualUsers = _userService.GetAssignedUsers(trainerId);

            // Assert
            actualUsers.ShouldBe(expectedUsers);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll,
                _userRepositoryMock.VerifyAll
            );
        }

        [Test]
        public async Task GetUsersSP_WhenCalled_ReturnsPagedResult()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            string? order = "FullName asc";
            var search = new DataTablesSearch();
            var searchItem = new UserSearchDto();

            IList<UserListDto> expectedUsers = new List<UserListDto>();

            var expectedResult = (
                data: expectedUsers,
                total: 0,
                totalDisplay: 0
            );

            _applicationUnitOfWorkMock
                .Setup(x => x.GetUsersSP(
                    pageIndex,
                    pageSize,
                    order,
                    search,
                    searchItem))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            // Act
            var actualResult = await _userService.GetUsersSP(
                pageIndex,
                pageSize,
                order,
                search,
                searchItem
            );

            // Assert
            actualResult.ShouldBe(expectedResult);

            this.ShouldSatisfyAllConditions(
                _applicationUnitOfWorkMock.VerifyAll
            );
        }

    }
}
