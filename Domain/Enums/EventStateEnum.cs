namespace Domain.Enums
{
    public enum EventStateEnum
    {
		Draft, // First Created
        Publish, // Allow To Buy Ticket

		Delay,
		
		EarlyCheckin,
		LateCheckin,

		Ongoing, 
		
		EarlyCheckout,
		LateCheckout,

		Ended,

		Cancelled 
    }
}