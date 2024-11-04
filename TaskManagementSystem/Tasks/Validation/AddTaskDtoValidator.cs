using FluentValidation;
using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks.Validation
{
    public class AddTaskDtoValidator : AbstractValidator<AddTaskDto>
    {
        public AddTaskDtoValidator() 
        {
            RuleFor(task => task.Name)
                .NotEmpty().WithMessage("Task name is required");

            RuleFor(task => task.Description)
                .NotEmpty().WithMessage("Description is required");
        }
    }
}
