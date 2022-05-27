using Application.Events.DTOs;
using Domain;
using FluentValidation;

namespace Application.Events
{
	public class CreateEventDTOValidator : AbstractValidator<CreateEventDTO>
	{
		public CreateEventDTOValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.EventCategoryId).NotEmpty();
			RuleFor(x => x.StartTime).NotEmpty();
			RuleFor(x => x.EndTime).NotEmpty();
			RuleFor(x => x.Multiplier_Factor).NotEmpty().GreaterThan(0).LessThan(6);
		}
	}
}