using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Doctor
{
    public class DoctorTests
    {
        [Fact]
        public void Create_WithValidData_ShouldReturnDoctorInstance()
        {
            // Arrange & Act
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Street 10", "Cairo", "Cardiology");

            // Assert
            doctor.Should().NotBeNull();
            doctor.FirstName.Should().Be("Ahmed");
            doctor.FullName.Should().Be("Ahmed Ali");
            doctor.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<Domain.Doctors.Events.DoctorRegisteredDomainEvent>();
        }

        [Fact]
        public void Create_ShouldTrimValuesAndRejectMissingSpecialization()
        {
            var doctor = Doctors.Doctor.Create(" Ahmed ", " Ali ", " Street 10 ", " Cairo ", " Cardiology ");

            doctor.FirstName.Should().Be("Ahmed");
            doctor.Address.Should().Be("Street 10");
            doctor.Specialization.Should().Be("Cardiology");
            Action act = () => Doctors.Doctor.Create("Ahmed", "Ali", "Address", "City", " ");
            act.Should().Throw<Domain.Doctors.Exceptions.InvalidSpecializationException>();
        }

        [Fact]
        public void UpdateSpecialization_ShouldRaiseDomainEvent()
        {
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Address", "Cairo", "Cardiology");
            doctor.ClearDomainEvents();

            doctor.UpdateSpecialization(" Neurology ");

            doctor.Specialization.Should().Be("Neurology");
            doctor.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<Domain.Doctors.Events.DoctorSpecializationChangedDomainEvent>();
        }

        [Theory]
        [InlineData("", "Ali")]
        [InlineData("Ahmed", " ")]
        public void Create_WithInvalidNames_ShouldThrowArgumentException(string firstName, string lastName)
        {
            // Act
            Action act = () => Doctors.Doctor.Create(firstName, lastName, "Address", "City", "Spec");

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
