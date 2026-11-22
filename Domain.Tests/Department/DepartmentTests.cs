using Domain.Departments;
using Domain.Departments.Exceptions;
using FluentAssertions;

namespace Domain.Tests.Departments
{
    public class DepartmentTests
    {
        // ===============================
        // CREATE
        // ===============================

        [Fact]
        public void Register_Should_Create_Department()
        {
            // Arrange
            var name = "Cardiology";
            var desc = "Heart Department";

            // Act
            var dept = Department.Register(name, desc);

            // Assert
            dept.Name.Should().Be(name);
            dept.Description.Should().Be(desc);
            dept.IsActive.Should().BeTrue();
            dept.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<Domain.Departments.Events.DepartmentRegisteredDomainEvent>();
        }

        [Fact]
        public void Register_Should_Throw_When_Name_Is_Empty()
        {
            var act = () => Department.Register("", "desc");

            act.Should().Throw<DepartmentInvalidNameException>();
        }

        [Fact]
        public void Register_Should_Trim_Name()
        {
            var dept = Department.Register("  ICU  ", "desc");

            dept.Name.Should().Be("ICU");
        }

        // ===============================
        // UPDATE
        // ===============================

        [Fact]
        public void UpdateInfo_Should_Update_Name_And_Description()
        {
            var dept = Department.Register("Old", "Old Desc");

            dept.UpdateInfo("New", "New Desc");

            dept.Name.Should().Be("New");
            dept.Description.Should().Be("New Desc");
        }

        [Fact]
        public void UpdateInfo_Should_Throw_When_Name_Invalid()
        {
            var dept = Department.Register("ICU", "Desc");

            var act = () => dept.UpdateInfo("", "desc");

            act.Should().Throw<DepartmentInvalidNameException>();
        }

        // ===============================
        // ACTIVATE / DEACTIVATE
        // ===============================

        [Fact]
        public void Deactivate_Should_Set_IsActive_False()
        {
            var dept = Department.Register("ICU", "Critical");

            dept.Deactivate();

            dept.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Deactivate_Should_Throw_When_Already_Inactive()
        {
            var dept = Department.Register("ICU", "Critical");
            dept.Deactivate();

            var act = () => dept.Deactivate();

            act.Should().Throw<DepartmentAlreadyInactiveException>();
        }

        [Fact]
        public void Activate_Should_Set_IsActive_True()
        {
            var dept = Department.Register("ICU", "Critical");
            dept.Deactivate();

            dept.Activate();

            dept.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Activate_Should_Throw_When_Already_Active()
        {
            var dept = Department.Register("ICU", "Critical");

            var act = () => dept.Activate();

            act.Should().Throw<DepartmentAlreadyActiveException>();
        }

        [Fact]
        public void UpdateInfo_ShouldRaiseEventAndTrimFields()
        {
            var dept = Department.Register("ICU", "Critical care");
            dept.ClearDomainEvents();

            dept.UpdateInfo("  Cardiology  ", "  Heart care  ");

            dept.Name.Should().Be("Cardiology");
            dept.Description.Should().Be("Heart care");
            dept.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<Domain.Departments.Events.DepartmentInformationUpdatedDomainEvent>();
        }

        [Fact]
        public void ChangeDescription_ShouldRaiseEventWithPreviousAndNewValues()
        {
            var department = Department.Register("ICU", "Old description");
            department.ClearDomainEvents();

            department.ChangeDescription(" New description ");

            var domainEvent = department.DomainEvents
                .OfType<Domain.Departments.Events.DepartmentDescriptionChangedDomainEvent>()
                .Single();
            domainEvent.PreviousDescription.Should().Be("Old description");
            domainEvent.NewDescription.Should().Be("New description");
        }

    }
}
