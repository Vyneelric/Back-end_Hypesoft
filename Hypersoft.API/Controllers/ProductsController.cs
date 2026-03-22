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
            message = "Produto não encontrado"
        });

        return Ok(new {
        success = true,
        status_code = 200,
        data = product
    });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) 
            return NotFound(new{
            success = false,
            status_code = 404,
            message = "Produto não encontrado"
        });
        
        return Ok(new{
            success = true,
            status_code = 200,
            message = "Produto atualizado com sucesso"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(new{
            success = true,
            status_code = 200,
            total_products = products.Count(),
            data = products
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (!result){
            return NotFound(new{
                success = false,
                status_code = 404,
                message = $"Produto de ID: '{id}' não foi encontrado/não existe"
            });
        }
        return Ok(new{
            success = true,
            status_code = 204,
            message = $"Produto de ID: '{id}' deletado com sucesso"
        });
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var products = await _mediator.Send(new GetProductsByNameQuery(name));
        
        if (products == null || !products.Any())
        {
            return NotFound(new
            {
                success = false,
                status_code = 404,
                message = $"Nenhum produto encontrado com o nome '{name}'"
            });
        }
        
        return Ok(new{
            success = true,
            status_code = 200,
            data = products
        });
    }

    [HttpGet("categories/{categoria_id}")]
    public async Task<IActionResult> GetByCategory(string categoria_id)
    {
        var products = await _mediator.Send(new GetProductsByCategoryQuery(categoria_id));
        
        if (products == null || !products.Any())
        {
            return NotFound(new
            {
                success = false,
                status_code = 404,
                message = "Nenhum produto encontrado para esta categoria"
            });
        }
        
        return Ok(new
        {
            success = true,
            status_code = 200,
            data = products
        });
    }

}
