namespace woorest.Models;

public class CloudApi
{
    public string? messaging_product { get; set; }
    public string? to { get; set; }
    public string? type { get; set; }
    public Template template { get; set; }
    public CloudApi()
    {
        template = new Template();
    }
}

public class Template
{
    public string? name { get; set; }
    public Language language { get; set; }
    public List<Component> components { get; set; }
    public Template()
    {
        components = new List<Component>();
        language = new Language();
    }
}

public class Language
{
    public string? code { get; set; }
}

public class Component
{
    public string? type { get; set; }
    public List<Parameter> parameters { get; set; }
    public Component()
    {
        parameters = new List<Parameter>();
    }
}

public class Parameter
{
    public string? type { get; set; }
    public string? text { get; set; }
}