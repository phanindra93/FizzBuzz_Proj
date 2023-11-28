using FizzBuzz3.Controllers;
using FizzBuzz3.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace FizzBuzz.Tests
{
    public class HomeControllerTests
    {
        public HomeControllerTests() { 
        }
        [Fact]
        public void GenerateFizzBuzz_RandomNumberWithinRange_ChecksFizzAndBuzz()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var divisibleByThreeMock = new Mock<IDivisibleByThree>();
            var divisibleByFiveMock = new Mock<IDivisibleByFive>();
            var divisibleByThreeAndFiveMock = new Mock<IDivisibleByThreeAndFive>();

            divisibleByThreeMock.Setup(x => x.IsDivisibleByThree(It.IsAny<int>())).Returns<int>(n => n % 3 == 0);
            divisibleByFiveMock.Setup(x => x.IsDivisibleByFive(It.IsAny<int>())).Returns<int>(n => n % 5 == 0);

            var homeController = new HomeController(
                loggerMock.Object,
                divisibleByThreeMock.Object,
                divisibleByFiveMock.Object,
                divisibleByThreeAndFiveMock.Object);

            // Act
            List<string> fizzBuzzList = homeController.GenerateFizzBuzz(44);

            // Assert
            Assert.NotNull(fizzBuzzList);

            // Choose a random index from the generated list
            Random random = new Random();
            int randomIndex = random.Next(0, fizzBuzzList.Count);

            // Check if the randomly selected number at the index is "Fizz" or "Buzz"
            int randomNumber = randomIndex + 1; // Adjust index to match 1-based FizzBuzz counting
            bool isFizz = divisibleByThreeMock.Object.IsDivisibleByThree(randomNumber);
            bool isBuzz = divisibleByFiveMock.Object.IsDivisibleByFive(randomNumber);

            // Assert that the result in the list matches the expected condition (case-insensitive)
            if (isFizz && isBuzz)
            {
                Assert.Equal("Fizz Buzz", fizzBuzzList[randomIndex], ignoreCase: true);
            }
            else if (isFizz)
            {
                Assert.Equal("Fizz", fizzBuzzList[randomIndex], ignoreCase: true);
            }
            else if (isBuzz)
            {
                Assert.Equal("Buzz", fizzBuzzList[randomIndex], ignoreCase: true);
            }
            else
            {
                Assert.Equal(randomNumber.ToString(), fizzBuzzList[randomIndex]);
            }
        }
    }
}

