using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnventisTT.Messaging.Abstraction;
using OnventisTT.Services.Abstraction;
using OnventisTT.WebModels;

namespace OnventisTT.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController : ControllerBase
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IMapper _mapper;
        private readonly IInvoiceService _service;
        private readonly IMessageSender _messageSender;

        public InvoiceController(
            ILogger<InvoiceController> logger,
            IMapper mapper,
            IInvoiceService service,
            IMessageSender messageSender)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _messageSender = messageSender;
        }

        [HttpPost]
        public IActionResult Post(InvoiceModel invoice)
        {
            try
            {
                var serviceModel = _mapper.Map<Services.Models.InvoiceModel>(invoice);
                _service.Create(serviceModel);

                _messageSender.SendMessage(invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not process invoice creation");

                return BadRequest();
            }

            return Ok();
        }
    }
}