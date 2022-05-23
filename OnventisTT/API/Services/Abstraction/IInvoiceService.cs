using OnventisTT.Services.Models;

namespace OnventisTT.Services.Abstraction
{
    public interface IInvoiceService
    {
        void Create(InvoiceModel invoice);
    }
}
