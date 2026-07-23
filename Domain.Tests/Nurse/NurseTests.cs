using Domain.Nurses;
using Domain.SharedKernel;


namespace Domain.Tests.Nurses
{
    public class NurseTests
    {
        private static Nurse CreateNurse(
            string firstName = "Sara",
            string lastName = "Ahmed",
            string address = "Cairo St. 1",
            string city = "Cairo",
            string? phoneNumber = "01000000000",
            bool isHead = false)
        {
            return Nurse.Register(firstName, lastName, address, city, phoneNumber, isHead);
        }

        [Fact]
        public void Register_ShouldCreateNurse_WithGivenValues()
        {
            var nurse = CreateNurse(
                firstName: "Mona",
                lastName: "Khaled",
                address: "Nasr City",
                city: "Cairo",
                phoneNumber: "01111111111",
                isHead: true);

            Assert.Equal("Mona", nurse.FirstName);
            Assert.Equal("Khaled", nurse.LastName);
            Assert.Equal("Nasr City", nurse.Address);
            Assert.Equal("Cairo", nurse.City);
            Assert.Equal("01111111111", nurse.PhoneNumber);
            Assert.True(nurse.IsHeadNurse);
            Assert.Empty(nurse.PatientAssignments);
        }


        [Fact]
        public void Register_ShouldSetIsHeadNurseFalse_ByDefault_WhenExplicitlyPassedFalse()
        {

            var nurse = CreateNurse(isHead: false);


            Assert.False(nurse.IsHeadNurse);
        }

        [Fact]
        public void Update_ShouldChangeAddressCityAndPhoneNumber()
        {

            var nurse = CreateNurse();


            nurse.Update("New Address", "Alexandria", "01222222222");

            Assert.Equal("New Address", nurse.Address);
            Assert.Equal("Alexandria", nurse.City);
            Assert.Equal("01222222222", nurse.PhoneNumber);
        }

        [Fact]
        public void Update_ShouldNotChangeFirstNameOrLastName()
        {

            var nurse = CreateNurse(firstName: "Sara", lastName: "Ahmed");

            nurse.Update("New Address", "Alexandria", "01222222222");

            Assert.Equal("Sara", nurse.FirstName);
            Assert.Equal("Ahmed", nurse.LastName);
        }

        [Fact]
        public void Update_ShouldAllowNullValues()
        {
            var nurse = CreateNurse();

            nurse.Update(null, null, null);

            Assert.Null(nurse.Address);
            Assert.Null(nurse.City);
            Assert.Null(nurse.PhoneNumber);
        }

        [Fact]
        public void UpdateDepartment_ShouldSetDepartmentId()
        {
            var nurse = CreateNurse();
            nurse.UpdateDepartment(5);
            Assert.Equal(5, nurse.DepartmentId);
        }

        [Fact]
        public void UpdateDepartment_ShouldAllowNull()
        {
            var nurse = CreateNurse();
            nurse.UpdateDepartment(5);

            nurse.UpdateDepartment(null);

            Assert.Null(nurse.DepartmentId);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ChangeHeadNurseStatus_ShouldUpdateIsHeadNurse(bool newStatus)
        {
            var nurse = CreateNurse(isHead: !newStatus);

            nurse.ChangeHeadNurseStatus(newStatus);

            Assert.Equal(newStatus, nurse.IsHeadNurse);
        }

        [Fact]
        public void AssignPatient_ShouldCreateAssignment_WhenShiftIsActiveAndNotAlreadyAssigned()
        {
            var nurse = CreateNurse();
            var shift = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));

            var assignment = nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);

            Assert.NotNull(assignment);
            Assert.Equal(10, assignment.PatientId);
            Assert.Equal(shift.Id, assignment.ShiftId);
            Assert.Equal(99, assignment.AssignedBy);
            Assert.True(assignment.IsActive);
            Assert.Contains(assignment, nurse.PatientAssignments);
        }

        [Fact]
        public void AssignPatient_ShouldThrow_WhenShiftDoesNotExist()
        {
            var nurse = CreateNurse();

            var act = () => nurse.AssignPatient(patientId: 10, shiftId: 999, assignedById: 99);

            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void AssignPatient_ShouldThrow_WhenShiftWasRemoved()
        {

            var nurse = CreateNurse();
            var shift = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));
            nurse.RemoveShift(shift.Id);

            var act = () => nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);

            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void AssignPatient_ShouldThrow_WhenSamePatientAlreadyActivelyAssignedToSameShift()
        {
            var nurse = CreateNurse();
            var shift = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));
            nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);

            var act = () => nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);

            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void AssignPatient_ShouldAllow_DifferentPatientsOnSameShift()
        {
            var nurse = CreateNurse();
            var shift = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));
            nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);

            var secondAssignment = nurse.AssignPatient(patientId: 20, shiftId: shift.Id, assignedById: 99);

            Assert.Equal(20, secondAssignment.PatientId);
            Assert.Equal(2, nurse.PatientAssignments.Count);
        }

        [Fact]
        public void AssignPatient_ShouldAllow_SamePatientAgain_AfterPreviousAssignmentEnded()
        {
            var nurse = CreateNurse();
            var shift = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));
            var firstAssignment = nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);
            firstAssignment.EndAssignment();

            var secondAssignment = nurse.AssignPatient(patientId: 10, shiftId: shift.Id, assignedById: 99);
            Assert.True(secondAssignment.IsActive);
            Assert.Equal(2, nurse.PatientAssignments.Count);
        }

        [Fact]
        public void RemoveShift_ShouldEndActiveAssignments_ForThatShiftOnly()
        {

            var nurse = CreateNurse();
            var shiftToRemove = nurse.AddShift(DateTime.UtcNow, DateTime.UtcNow.AddHours(8));
            var assignment = nurse.AssignPatient(patientId: 10, shiftId: shiftToRemove.Id, assignedById: 99);
            nurse.RemoveShift(shiftToRemove.Id);

            Assert.False(assignment.IsActive);
            Assert.NotNull(assignment.EndedAt);
        }

        [Fact]
        public void RemoveShift_ShouldThrow_WhenShiftDoesNotExist()
        {
            var nurse = CreateNurse();

            var act = () => nurse.RemoveShift(999);
            Assert.Throws<DomainException>(act);
        }

    }
}