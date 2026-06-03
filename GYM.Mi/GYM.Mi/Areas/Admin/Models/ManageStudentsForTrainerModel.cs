namespace GYM.Mi.Areas.Admin.Models
{
    public class ManageStudentsForTrainerModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
       
        public List<ManageStudentsForAvailableStudentsModel> AvailableStudents { get; set; } = new();
        public List<ManageStudentsForAvailableStudentsModel> AssignedStudents { get; set; } = new();
    }
    
    public class ManageStudentsForAvailableStudentsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string MembershipText { get; set; } = "No Membership";

        public string MembershipBadgeClass { get; set; } = "bg-secondary";
    }
}


