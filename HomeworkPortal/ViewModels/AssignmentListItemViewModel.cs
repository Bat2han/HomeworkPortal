namespace HomeworkPortal.ViewModels
{
    public class AssignmentListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";

        public int TeacherId { get; set; }
    }
}
