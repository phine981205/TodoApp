using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Models.Validators
{
    public class UpsertTodoItemValidator: AbstractValidator<UpsertTodoItemRequest>
    {
        public UpsertTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255)
                .WithMessage("Title must not exceed 255 characters.");
        }
    }
}
