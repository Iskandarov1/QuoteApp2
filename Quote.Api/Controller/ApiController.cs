using Quote.Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quote.Domain.Core.Primitives;

namespace Quote.Api.Controller
{
    [Route("api")]
    public class ApiController : ControllerBase
    {
        public ApiController(IMediator mediator) => Mediator = mediator;

        protected IMediator Mediator { get; }
        
        protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));

        protected new IActionResult Ok(object value) => base.Ok(value);
        protected IActionResult Ok<T>(T value) => base.Ok(value);


        protected new NotFoundResult NotFound() => base.NotFound();
    }
}