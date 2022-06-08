namespace Domain.Enums
{
    public enum EventStateEnum
    {
		Draft, // First Created
        Idle, // Allow Buying Ticket
		Delay, // Delay Event
		Ongoing, // Event Started
		Ended, // Event Ended
		Cancel // Event Canceled
    }
}