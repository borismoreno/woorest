using Microsoft.AspNetCore.Mvc;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;

namespace woorest.Controllers;

[ApiController]
[Route("[controller]")]
public class WoocommerceController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public WoocommerceController(IConfiguration configuration)
    {
        _configuration = configuration;
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
}