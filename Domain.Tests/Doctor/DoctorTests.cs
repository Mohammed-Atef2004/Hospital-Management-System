using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Tests.Doctor
{
    public class DoctorTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnDoctorInstance()
        {
            // Arrange & Act
            var doctor = Domain.Entities.Doctor.Create("Ahmed", "Ali", "Street 10", "Cairo", "Cardiology");

            // Assert
            doctor.Should().NotBeNull();
            doctor.FirstName.Should().Be("Ahmed");
            doctor.FullName.Should().Be("Ahmed Ali");
        }

        [Theory]
        [InlineData("", "Ali")]
        [InlineData("Ahmed", " ")]
        public void Create_WithInvalidNames_ShouldThrowArgumentException(string firstName, string lastName)
        {
            // Act
            Action act = () => Domain.Entities.Doctor.Create(firstName, lastName, "Address", "City", "Spec");

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
