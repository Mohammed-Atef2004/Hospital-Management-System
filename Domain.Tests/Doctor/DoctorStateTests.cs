using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Doctor
{
    public class DoctorStateTests
    {
        [Fact]
        public void Deactivate_ShouldSetIsDeletedToTrue()
        {
            // Arrange
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");

            // Act
            doctor.Deactivate();

            // Assert
            doctor.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public void RelocateDoctorOffice_ShouldUpdateAddressAndTimestamp()
        {
            // Arrange
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Old Address", "Old City", "Spec");

            // Act
            doctor.RelocateDoctorOffice("New Address", "New City");

            // Assert
            doctor.Address.Should().Be("New Address");
            doctor.City.Should().Be("New City");
            doctor.UpdatedAt.Should().NotBeNull();
        }
    }
}
