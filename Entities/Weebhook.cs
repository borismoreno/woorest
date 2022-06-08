namespace woorest.Entities;

public class Weebhook
{
    public Guid Id { get; set; }
    public int WeebhookId { get; set; }
    public int ParentId { get; set; }
    public string? Number { get; set; }
    public string? OrderKey { get; set; }
    public string? CreatedVia { get; set; }
    public string? Status { get; set; }
    public string? Currency { get; set; }
    public DateTime? DateCreated { get; set; }
    public Billing? Billing { get; set; }
}

public class Billing
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address_1 { get; set; }
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? Phone { get; set; }
}