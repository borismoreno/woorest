using System.Text;
using Microsoft.AspNetCore.Mvc;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using woorest.Dtos;
using woorest.Entities;
using woorest.Repositories;

namespace woorest.Controllers;

[ApiController]
[Route("[controller]")]
public class WoocommerceController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IWoocommerceRepository _woocommercerepository;

    public WoocommerceController(IConfiguration configuration, IWoocommerceRepository woocommerceRepository)
    {
        _configuration = configuration;
        _woocommercerepository = woocommerceRepository;
    }

    [HttpGet(Name = "GetCustomer")]
    public async Task<ActionResult<Customer>> GetCustomer()
    {
        var customerKey = _configuration.GetValue<string>("Woocommerce:CustomerKey");
        var customerSecret = _configuration.GetValue<string>("Woocommerce:CustomerSecret");
        RestAPI rest = new RestAPI("https://thegeekstoreec.com/wp-json/wc/v3/", customerKey, customerSecret);
        WCObject wc = new WCObject(rest);
        var customer = await wc.Customer.Get(1);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> PostWeebhook()
    {
        var content = "";
        using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            content = await reader.ReadToEndAsync();
        }
        var weebhook = new Weebhook
        {
            // WeebhookId = weebhookInputDto.Id,
            // ParentId = weebhookInputDto.ParentId,
            // Number = weebhookInputDto.Number,
            OrderKey = content,
            // CreatedVia = weebhookInputDto.CreatedVia,
            // Status = weebhookInputDto.Status,
            // Currency = weebhookInputDto.Currency,
            // DateCreated = Convert.ToDateTime(weebhookInputDto.DateCreated),
            // Billing = new Billing
            // {
            //     FirstName = weebhookInputDto.Billing?.FirstName,
            //     LastName = weebhookInputDto.Billing?.LastName,
            //     Address_1 = weebhookInputDto.Billing?.Address_1,
            //     City = weebhookInputDto.Billing?.City,
            //     PostCode = weebhookInputDto.Billing?.PostCode
            // }
        };

        await _woocommercerepository.CreateAsync(weebhook);
        return Ok();
    }
}