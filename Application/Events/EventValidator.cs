
using Domain;
using FluentValidation;

namespace Application.Events
{
	public class EventValidator : AbstractValidator<Event>
	{
		public EventValidator()
		{
			RuleFor(x => x.Title).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			// RuleFor(x => x.EventCategoryId).NotEmpty();
			RuleFor(x => x.StartTime).NotEmpty();
			RuleFor(x => x.EndTime).NotEmpty();
			RuleFor(x => x.MultiplierFactor).NotEmpty().GreaterThan(0).LessThan(6);
		}
	}
}