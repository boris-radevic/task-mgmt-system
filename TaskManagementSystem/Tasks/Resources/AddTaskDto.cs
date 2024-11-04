using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Tasks.Resources
{
    public class AddTaskDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
    }
}
