using System;
using System.Collections.Generic;
using System.Text;
using PoseDatabaseWebApi.Models;
using Xunit;


namespace PoseDatabaseWebApiXUnitTests
{
    public class PoseTests : IDisposable
    {
        Pose testPose;

        public PoseTests()
        {
            testPose = new Pose
            {
                PoseName = "Testing Name",
                PoseOriginName = "Testing Origin Name",
                PoseOriginStyle = "Test Origin Style"
            };
        }

        public void Dispose()
        {
            testPose = null;
        }

        [Fact]
        public void CanChangePoseName()
        {
            // Arrange

            // Act
            testPose.PoseName = "Something else";

            // Assert
            Assert.Equal("Something else", testPose.PoseName);
        }

        [Fact]
        public void CanChangePoseOriginStyle()
        {
            // Arrange

            // Act
            testPose.PoseOriginStyle = "Something else";

            // Assert
            Assert.Equal("Something else", testPose.PoseOriginStyle);
        }

        [Fact]
        public void CanChangePoseOriginName()
        {
            // Arrange

            // Act
            testPose.PoseOriginName = "Something else";
            // Assert
            Assert.Equal("Something else", testPose.PoseOriginName);
        }
    }
}
