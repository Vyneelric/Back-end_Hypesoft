using MediatR;

namespace Hypersoft.Application.Queries;

public record GetTotalValueStockQuery() : IRequest<decimal>
{
    
}
