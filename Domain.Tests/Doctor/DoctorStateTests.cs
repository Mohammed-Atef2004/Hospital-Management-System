using FluentAssertions;
using Domain.SharedKernel;
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
            doctor.ClearDomainEvents();

            // Act
            doctor.Deactivate();

            // Assert
            doctor.IsDeleted.Should().BeTrue();
            doctor.DeletedAt.Should().NotBeNull();
            doctor.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<Domain.Doctors.Events.DoctorDeactivatedDomainEvent>();
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

        [Fact]
        public void Deactivate_WhenAlreadyInactive_ShouldThrowDomainException()
        {
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            doctor.ClearDomainEvents();
            doctor.Deactivate();

            Action act = doctor.Deactivate;

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Reactivate_ShouldRestoreDoctorAndRaiseDomainEvent()
        {
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            doctor.ClearDomainEvents();
            doctor.Deactivate();
            doctor.ClearDomainEvents();

            doctor.Reactivate();

            doctor.IsDeleted.Should().BeFalse();
            doctor.DeletedAt.Should().BeNull();
            doctor.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<Domain.Doctors.Events.DoctorReactivatedDomainEvent>();
        }
        [Fact]
        public void InactiveDoctor_CannotBeRelocatedOrRenamed()
        {
            var doctor = Doctors.Doctor.Create("Ahmed", "Ali", "Addr", "City", "Spec");
            doctor.Deactivate();

            Action relocate = () => doctor.RelocateDoctorOffice("New address", "New city");
            Action rename = () => doctor.CorrectName("New", "Name");

            relocate.Should().Throw<DomainException>();
            rename.Should().Throw<DomainException>();
        }

    }
}
