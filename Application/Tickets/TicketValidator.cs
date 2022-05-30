using Domain;
using FluentValidation;

namespace Application.Tickets
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Cost).NotEmpty();
        }
    }
}