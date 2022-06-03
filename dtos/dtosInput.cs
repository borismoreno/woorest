namespace woorest.Dtos;

public record WeebhookInputDto(
    int Id,
    int ParentId,
    string? Number,
    string? OrderKey,
    string? CreatedVia,
    string? Status,
    string? Currency,
    string? DateCreated,
    BillingDto? Billing
);

public record BillingDto(
    string? FirstName,
    string? LastName,
    string? Address_1,
    string? City,
    string? PostCode
);