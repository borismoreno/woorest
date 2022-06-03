using woorest.Entities;

namespace woorest.Repositories;

public interface IWoocommerceRepository
{
    Task CreateAsync(Weebhook weebhook);
}