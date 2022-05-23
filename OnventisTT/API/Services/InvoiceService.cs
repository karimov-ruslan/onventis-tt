using AutoMapper;
using OnventisTT.Infrastructure.Entities;
using OnventisTT.Services.Abstraction;
using OnventisTT.Services.Models;

namespace OnventisTT.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            MyDbContext dbContext, 
            IMapper mapper,
            ILogger<InvoiceService> logger)
        {
            _myDbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public void Create(InvoiceModel invoice)
        {
            var entity = _mapper.Map<InvoiceModel, Invoice>(invoice);

            _myDbContext.Invoices.Add(entity);

            _myDbContext.SaveChanges();
        }
    }
}
