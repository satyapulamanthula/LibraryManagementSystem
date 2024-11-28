namespace LibraryManagementSystem.Data.Entities
{
    public class Semesters
    {
        public int SemesterId { get; set; }
        public string? SemesterName { get; set; }

        // Navigation property
        public ICollection<Book>? Books { get; set; }
    }
}
