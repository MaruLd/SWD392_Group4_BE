namespace Domain
{
    public class EventTicket
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public bool IsValid { get; set; }
    }
}