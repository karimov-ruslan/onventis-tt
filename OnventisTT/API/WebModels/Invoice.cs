namespace OnventisTT.WebModels
{
    public class InvoiceModel
    {
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? Supplier { get; set; }
        public List<InvoiceLineModel>? Lines { get; set; }
    }
}
