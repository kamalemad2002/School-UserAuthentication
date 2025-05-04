using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace School.Data
{

    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            // Verifica si el usuario está autenticado y si tiene los roles necesarios
            if (!user.Identity.IsAuthenticated)
            {
                // Si no está autenticado, devuelve un 401 (Unauthorized)
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "No autenticado",
                    Detail = "El usuario no está autenticado.",
                    Instance = context.HttpContext.Request.Path
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else if (!user.IsInRole("Admin")) // Aquí puedes poner la lógica de roles que necesites
            {
                // Si el usuario no tiene el rol adecuado, devuelve un 403 (Forbidden)
                context.Result = new ObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Acceso prohibido",
                    Detail = "El usuario no tiene permisos suficientes para acceder a este recurso.",
                    Instance = context.HttpContext.Request.Path
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}