using System.Text;
using Microsoft.AspNetCore.Mvc;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using woorest.Dtos;
using woorest.Entities;
using woorest.Repositories;
using System.Text.Json;
using System.Text.Json.Nodes;
using woorest.Models;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace woorest.Controllers;

[ApiController]
[Route("[controller]")]
public class WoocommerceController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IWoocommerceRepository _woocommercerepository;

    private readonly IHttpClientFactory _httpClientFactory;

    private static readonly HttpClient client = new HttpClient();

    public WoocommerceController(IConfiguration configuration, IWoocommerceRepository woocommerceRepository, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _woocommercerepository = woocommerceRepository;
        _httpClientFactory = httpClientFactory;
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
    public async Task<ActionResult> PostWeebhook(WeebhookInputDto weebhookInputDto)
    {
        var cloudApiToken = _configuration.GetValue<string>("CloudApi:Token");
        var cloudApiPhoneId = _configuration.GetValue<string>("CloudApi:PhoneId");
        var cloudApiTemplate = _configuration.GetValue<string>("CloudApi:Template");
        var weebhook = new Weebhook
        {
            WeebhookId = weebhookInputDto.Id,
            ParentId = weebhookInputDto.Parent_Id,
            Number = weebhookInputDto.Number,
            OrderKey = weebhookInputDto.Order_Key,
            CreatedVia = weebhookInputDto.Created_Via,
            Status = weebhookInputDto.Status,
            Currency = weebhookInputDto.Currency,
            DateCreated = Convert.ToDateTime(weebhookInputDto.Date_Created),
            Billing = new Billing
            {
                FirstName = weebhookInputDto.Billing?.First_Name,
                LastName = weebhookInputDto.Billing?.Last_Name,
                Address_1 = weebhookInputDto.Billing?.Address_1,
                City = weebhookInputDto.Billing?.City,
                PostCode = weebhookInputDto.Billing?.PostCode,
                Phone = weebhookInputDto.Billing?.Phone?.Replace('+', ' ').Trim()
            }
        };

        await _woocommercerepository.CreateAsync(weebhook);

        if (weebhook.Status == "completed")
        {
            var contenido = new CloudApi();
            List<Parameter> parametros = new List<Parameter>();
            parametros.Add(new Parameter
            {
                type = "text",
                text = weebhook.Billing.FirstName
            });
            parametros.Add(new Parameter
            {
                type = "text",
                text = weebhook.Number
            });
            contenido.messaging_product = "whatsapp";
            contenido.to = weebhook.Billing.Phone;
            contenido.type = "template";
            contenido.template = new Template();
            contenido.template.name = cloudApiTemplate;
            contenido.template.language = new Language();
            contenido.template.language.code = "es";
            contenido.template.components = new List<Component>();
            contenido.template.components.Add(new Component
            {
                type = "body",
                parameters = parametros
            });

            var todoItemJson = new StringContent(
            JsonSerializer.Serialize(contenido),
            Encoding.UTF8,
            Application.Json); // using static System.Net.Mime.MediaTypeNames;

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {cloudApiToken}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var httpResponseMessage = await httpClient.PostAsync($"https://graph.facebook.com/v13.0/{cloudApiPhoneId}/messages", todoItemJson);
        }

        return Ok();
    }
}