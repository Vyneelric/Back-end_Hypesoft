using Hypersoft.Application.Commands;
using Hypersoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hypersoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a category")]
    [SwaggerResponse(201, "Category created successfully")]
    [SwaggerResponse(400, "Validation error")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return StatusCode(201, new {
            success = true,
            status_code = 201,
            message = "Categoria criada com sucesso",
            data = new { id }
        });
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get category by ID")]
    [SwaggerResponse(200, "Category found")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<IActionResult> GetById(string id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        
        if (category == null)
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Categoria não encontrada"
            });

        return Ok(new {
            success = true,
            status_code = 200,
            data = category
        });
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all categories")]
    [SwaggerResponse(200, "List of categories")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _mediator.Send(new GetAllCategoryQuery());
        return Ok(new {
            success = true,
            status_code = 200,
            total_categories = categories.Count(),
            data = categories
        });
    }
    
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a category")]
    [SwaggerResponse(204, "Category deleted successfully")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));
        if (!result)
        {
            return NotFound(new {
                success = false,
                status_code = 404,
                message = "Categoria não encontrada"
            });
        }
        return StatusCode(204);
    }
}
