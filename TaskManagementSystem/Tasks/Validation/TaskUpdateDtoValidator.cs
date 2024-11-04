using FluentValidation;
using TaskManagementSystem.Tasks.Resources;

namespace TaskManagementSystem.Tasks.Validation
{
    public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
    {
        public TaskUpdateDtoValidator() 
        {
            RuleFor(x => x.TaskId)
                .NotEmpty()
                .WithMessage("TaskId is required.");
            RuleFor(x => x.NewStatusId)
                .NotEmpty()
                .WithMessage("NewStatusId is required.")
                .InclusiveBetween(0, 2).WithMessage("NewStatusId should be in range from 0 to 2");
                
        }
    }
}
