using Hypersoft.Application.Commands;
using Hypersoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypersoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new {
        success = true,
        status_code = 201,
        message = "Produto criado com sucesso"
    });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        
        if (product == null)
            return NotFound(new{
            success = false,
            status_code = 404,
            message = "Produto não encontrada"
        });

        return Ok(new {
        success = true,
        status_code = 200,
        data = product
    });
    }
}
