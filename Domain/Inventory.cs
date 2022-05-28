namespace Domain
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        
        public int Name { get; set; }
        
        
        public ICollection<Ticket> Tickets { get; set; }
    }
}