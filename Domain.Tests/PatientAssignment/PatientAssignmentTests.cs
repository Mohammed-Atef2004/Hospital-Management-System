using Domain.PatientAssignments;

namespace Domain.Tests.PatientAssignments
{
    public class PatientAssignmentTests
    {
        [Fact]
        public void Create_ShouldSetAllPropertiesCorrectly()
        {

            var before = DateTime.UtcNow;
            var assignment = PatientAssignment.Create(
                patientId: 10,
                nurseId: 5,
                assignedBy: 2,
                shiftId: 1);

            Assert.Equal(10, assignment.PatientId);
            Assert.Equal(5, assignment.NurseId);
            Assert.Equal(2, assignment.AssignedBy);
            Assert.Equal(1, assignment.ShiftId);
            Assert.True(assignment.IsActive);
            Assert.Null(assignment.EndedAt);
            Assert.True(assignment.AssignedAt >= before);
            Assert.True(assignment.CreatedAt >= before);
        }

        [Fact]
        public void Create_ShouldSetIsActiveTrue_ByDefault()
        {

            var assignment = PatientAssignment.Create(1, 1, 1, 1);
            Assert.True(assignment.IsActive);
        }

        [Fact]
        public void Create_ShouldNotSetEndedAt()
        {
            var assignment = PatientAssignment.Create(1, 1, 1, 1);

            Assert.Null(assignment.EndedAt);
        }

        [Fact]
        public void EndAssignment_ShouldSetIsActiveToFalse()
        {

            var assignment = PatientAssignment.Create(1, 1, 1, 1);
            assignment.EndAssignment();
            Assert.False(assignment.IsActive);
        }

        [Fact]
        public void EndAssignment_ShouldSetEndedAt()
        {

            var assignment = PatientAssignment.Create(1, 1, 1, 1);
            var before = DateTime.UtcNow;
            assignment.EndAssignment();

            Assert.NotNull(assignment.EndedAt);
            Assert.True(assignment.EndedAt >= before);
        }
    }
}
