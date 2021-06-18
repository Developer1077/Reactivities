using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private  IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        protected ActionResult HandleResult<T>(Result<T> result) {

            if (result == null) return NotFound(new AppResponse(StatusCodes.Status404NotFound));
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound(new AppResponse(StatusCodes.Status404NotFound));

            return BadRequest(new AppResponse(StatusCodes.Status400BadRequest, result.Error));

        }
    }
}
