using Hypersoft.Application.Commands;
using Hypersoft.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypersoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new {
        success = true,
        status_code = 201,
        message = "Categoria foi criada com sucesso"
    });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        
        if (category == null)
            return NotFound(new{
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
}
