namespace Domain.Enums
{
	public enum EventStateEnum
	{
		None = -1, // For Query
		
		Draft, // First Created
		Publish, // Allow To Buy Ticket

		Delay,

		// EarlyCheckin,
		// LateCheckin,
		CheckingIn,

		Ongoing,

		// EarlyCheckout,
		// LateCheckout,
		CheckingOut,

		Ended,

		Cancelled
	}
}