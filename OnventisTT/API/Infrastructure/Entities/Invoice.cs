namespace OnventisTT.Infrastructure.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? Supplier { get; set; }
        public List<InvoiceLine>? Lines { get; set; }
    }
}
