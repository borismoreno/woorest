namespace woorest.Dtos;

public record WeebhookInputDto(
    int Id,
    int Parent_Id,
    string? Number,
    string? Order_Key,
    string? Created_Via,
    string? Status,
    string? Currency,
    string? Date_Created,
    BillingDto? Billing
);

public record BillingDto(
    string? First_Name,
    string? Last_Name,
    string? Address_1,
    string? City,
    string? PostCode,
    string? State
);