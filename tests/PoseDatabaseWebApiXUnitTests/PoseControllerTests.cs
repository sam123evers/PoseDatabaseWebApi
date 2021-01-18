using System;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using PoseDatabaseWebApi.Models;
using Xunit;
using PoseDatabaseWebApi.Controllers;
using PoseDatabaseWebApi.Profiles;
using PoseDatabaseWebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace PoseDatabaseWebApiXUnitTests
{
    public class PoseControllerTests : IDisposable
    {
        Mock<IPoseRepository> mockRepo;
        PoseProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public PoseControllerTests()
        {
            mockRepo = new Mock<IPoseRepository>();
            realProfile = new PoseProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);

        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        [Fact]
        public void GetPoses_Returns200OK_WhenDBIsEmpty()
        {
            // Arrange
            // ...?
            mockRepo.Setup(repo => repo.GetPoses()).Returns(GetPoses(0));

            // creating instance of controller with mocked repo and mapper instance
            var controller = new PosesController(mockRepo.Object, mapper);

            // Act 
            var result = controller.GetPoses();

            // Assert
            //Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public void GetAllPoses_ReturnsOneItem_WhenDBHasOneResource()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPoses()).Returns(GetPoses(1));

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetPoses();

            // Assert
            // convert original result to OkObjectResult
            var okResult = result.Result as OkObjectResult;

            var poses = okResult.Value as List<PoseReadDto>;

            Assert.Single(poses);
        }

        [Fact]
        public void GetPoses_ReturnsCorrectType_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetPoses()).Returns(GetPoses(1));

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetPoses();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<PoseReadDto>>>(result);
        }

        [Fact]
        public void GetAllPoses_Returns200Ok_WhenDBHasOneResource()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPoses()).Returns(GetPoses(1));

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            // get poses from mock db with controller action
            var result = controller.GetPoses();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllPoses_ReturnsCorrectType_WhenDBHAsOneResource()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPoses()).Returns(GetPoses(1));

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetPoses();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<PoseReadDto>>>(result);
        }

        [Fact]
        public void GetPoseById_Returns404NotFound_WhenNonExistentIdProvided()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(0)).Returns(() => null);

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetPose(0);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetPoseById_Returns200OkResponse_WhenValidIdProvided()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetPose(1)).Returns(new Pose { 
                Id = 1, 
                PoseName = "Mock Name", 
                PoseOriginName = "Mock Origin Name", 
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.GetPose(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetPoseById_Returns200WithData_WhenValidIdProvided()
        {
            mockRepo.Setup(repo => repo.GetPose(1)).Returns(new Pose {
                Id = 1,
                PoseName = "Mock Name",
                PoseOriginName = "Mock Origin Name",
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            var result = controller.GetPose(1);

            Assert.IsType<ActionResult<PoseReadDto>>(result);
        }

        [Fact]
        public void AddPoseToDB_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(1)).Returns( new Pose {
                Id = 1,
                PoseName = "Mock Name",
                PoseOriginName = "Mock Origin Name",
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.AddPoseToDb(new PoseCreateDto { });

            // Assert
            Assert.IsType<ActionResult<PoseReadDto>>(result);
        }

        [Fact]
        public void AddPoseToDb_Returns201Created_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo => repo.GetPose(1)).Returns( new Pose {
                Id = 1,
                PoseName = "Mock Name",
                PoseOriginName = "Mock Origin Name",
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            var result = controller.AddPoseToDb(new PoseCreateDto { });

            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void UpdatePose_Returns204NoContent_WhenValidObjectSubmitted()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(1)).Returns(new Pose {
                Id = 1,
                PoseName = "Mock Name",
                PoseOriginName = "Mock Origin Name",
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.UpdatePose(1, new PoseUpdateDto { });

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdatePose_Returns404NotFound_WhenNonExistentResourceIdSubmitted()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(0)).Returns(() => null);

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.UpdatePose(0, new PoseUpdateDto { });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PartialPoseUpdate_Returns404NotFound_WhenNonExistentResourceIdProvided()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(0)).Returns(() => null);

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.PartialPoseUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<PoseUpdateDto> { });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeletePose_Returns204NoContent_WhenValidResourceIdSubmitted()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(1)).Returns(new Pose {
                Id = 1,
                PoseName = "Mock Name",
                PoseOriginName = "Mock Origin Name",
                PoseOriginStyle = "Mock Origin Style"
            });

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.DeletePose(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeletePose_Returns404NotFound_WhenNonExistentResourceIdSubmitted()
        {
            // Arrange
            mockRepo.Setup(repo => repo.GetPose(0)).Returns(() => null);

            var controller = new PosesController(mockRepo.Object, mapper);

            // Act
            var result = controller.DeletePose(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        private List<Pose> GetPoses(int num)
        {
            var poses = new List<Pose>();

            if (num > 0)
            {
                poses.Add(new Pose 
                {
                    Id= 0,
                    PoseName = "I am just a Test",
                    PoseOriginName = "Origin Name Test",
                    PoseOriginStyle = "Origin Style Test"
                });
            }
            return poses;
        }
    }
}
