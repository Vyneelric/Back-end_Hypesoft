using MediatR;
using Hypersoft.Domain.Repositories;
using Hypersoft.Application.Queries;
public class GetTotalValueStockHandler : IRequestHandler<GetTotalValueStockQuery, decimal>
{
    private readonly IProductRepository _repository;

    public GetTotalValueStockHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(GetTotalValueStockQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();
        return products.Sum(p => p.preco * p.quantidade_estoque);
    }
}