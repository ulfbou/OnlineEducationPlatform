using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationPlatform.Shared.Entities;
using OnlineEducationPlatform.Shared.Interfaces;

namespace OnlineEducationPlatform.Shared.Services
{
    public abstract class BaseService
    {
        protected virtual IActionResult Ok() => new OkResult();
        protected virtual IActionResult NoContent() => new NoContentResult();
        protected virtual IActionResult Ok<T>(T value) => new OkObjectResult(value);
        protected virtual IActionResult Created(string uri, object value) => new CreatedResult(uri, value);
        protected virtual IActionResult Unauthorized()
            => new UnauthorizedResult();
        protected virtual IActionResult Unauthorized(string errorMessage)
        {
            // Generate a 401 error response with a custom error message
            return new ObjectResult(new IdentityError
            {
                Code = "Unauthorized",
                Description = errorMessage
            })
            {
                StatusCode = 401
            };
        }

        protected IActionResult BadRequest(IdentityResult result) =>
            new BadRequestObjectResult(result.Errors);
        protected virtual IActionResult BadRequest(string code, string description) =>
            new BadRequestObjectResult(new IdentityError
            {
                Code = code,
                Description = description
            });

        protected virtual IActionResult NotFound(string code, string description) =>
            new NotFoundObjectResult(new IdentityError
            {
                Code = code,
                Description = description
            });

        protected IActionResult InternalServerError(
                string code = "Internal server error",
                string description = "Internal Server Error")
            => new ObjectResult(new IdentityError
            {
                Code = code,
                Description = description
            })
            {
                StatusCode = 500
            };
        protected virtual IdentityResult Fail(string code, string description) =>
            IdentityResult.Failed(new IdentityError
            {
                Code = code,
                Description = description
            });
    }
}