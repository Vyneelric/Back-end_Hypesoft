using Hypersoft.Application.Commands;
using Hypersoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hypersoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a product", Description = "Creates a new product in the database.")]
    [SwaggerResponse(201, "Product created successfully")]
    [SwaggerResponse(400, "Validation error")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var id = await _mediator.Send(command);
        return StatusCode(201, new {
            success = true,
            status_code = 201,
            message = "Produto criado com sucesso",
            data = new { id }
        });
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get product by ID")]
    [SwaggerResponse(200, "Product found")]
    [SwaggerResponse(404, "Product not found")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        
        if (product == null)
            return NotFound(new {
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
    [SwaggerOperation(Summary = "Update a product")]
    [SwaggerResponse(200, "Product updated successfully")]
    [SwaggerResponse(400, "Validation error")]
    [SwaggerResponse(404, "Product not found")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result) 
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Produto não encontrado"
            });
        
        return Ok(new {
            success = true,
            status_code = 200,
            message = "Produto atualizado com sucesso"
        });
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all products", Description = "Returns all products. Optionally filter by stock lower than a value.")]
    [SwaggerResponse(200, "List of products")]
    public async Task<IActionResult> GetAll([FromQuery] int? estoqueMenorQue)
    {
        var products = await _mediator.Send(new GetAllProductsQuery { EstoqueMenorQue = estoqueMenorQue });
        return Ok(new {
            success = true,
            status_code = 200,
            total_products = products.Count(),
            data = products
        });
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a product")]
    [SwaggerResponse(204, "Product deleted successfully")]
    [SwaggerResponse(404, "Product not found")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (!result)
        {
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Produto não encontrado"
            });
        }
        return StatusCode(204);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Search products by name")]
    [SwaggerResponse(200, "Products found")]
    [SwaggerResponse(404, "No products found")]
    public async Task<IActionResult> SearchByName([FromQuery] string name)
    {
        var products = await _mediator.Send(new GetProductsByNameQuery(name));
        
        if (products == null || !products.Any())
        {
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Nenhum produto encontrado com este nome"
            });
        }
        
        return Ok(new {
            success = true,
            status_code = 200,
            total_products = products.Count(),
            data = products
        });
    }

    [HttpGet("categories/{categoria_id}")]
    [SwaggerOperation(Summary = "Get products by category")]
    [SwaggerResponse(200, "Products found")]
    [SwaggerResponse(404, "No products found for this category")]
    public async Task<IActionResult> GetByCategory(string categoria_id)
    {
        var products = await _mediator.Send(new GetProductsByCategoryQuery(categoria_id));
        
        if (products == null || !products.Any())
        {
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Nenhum produto encontrado para esta categoria"
            });
        }
        
        return Ok(new {
            success = true,
            status_code = 200,
            total_products = products.Count(),
            data = products
        });
    }

    [HttpGet("total_value_stock")]
    [SwaggerOperation(Summary = "Get total stock value", Description = "Returns the total monetary value of all products in stock.")]
    [SwaggerResponse(200, "Total stock value")]
    public async Task<IActionResult> GetTotalStockValue()
    {
        var totalValue = await _mediator.Send(new GetTotalValueStockQuery());
        return Ok(new
        {
            success = true,
            status_code = 200,
            total_stock_value = totalValue
        });
    }
}
