using Microsoft.AspNetCore.Mvc;
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
        Customer respuesta = new Customer();
        respuesta.email = customerKey;
        respuesta.first_name = customerSecret;
        return Ok(respuesta);
    }
}