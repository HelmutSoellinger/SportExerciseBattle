﻿using SportExerciseBattle.Models;

namespace NUnitTestsForSEB
{

        [TestFixture]
        public class UserModelTests
        {
            [Test]
            public void User_Initialization_ShouldSetDefaultValues()
            {
                // Arrange
                var user = new User();

                // Assert
                Assert.That(user.Username, Is.EqualTo(""));
                Assert.That(user.Password, Is.EqualTo(""));
                Assert.That(user.Bio, Is.EqualTo(""));
                Assert.That(user.Image, Is.EqualTo(""));
                Assert.That(user.Name, Is.EqualTo(""));
            }

            [Test]
            public void User_SetterAndGetter_ShouldWorkCorrectly()
            {
                // Arrange
                var user = new User
                {
                    Username = "testuser",
                    Password = "testpass",
                    Bio = "Test Bio",
                    Image = "Test Image",
                    Name = "Test User"
                };

                // Assert
                Assert.That(user.Username, Is.EqualTo("testuser"));
                Assert.That(user.Password, Is.EqualTo("testpass"));
                Assert.That(user.Bio, Is.EqualTo("Test Bio"));
                Assert.That(user.Image, Is.EqualTo("Test Image"));
                Assert.That(user.Name, Is.EqualTo("Test User"));
            }
        }
    }

