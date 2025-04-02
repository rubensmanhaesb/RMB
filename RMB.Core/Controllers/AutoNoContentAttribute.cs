using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections;


namespace RMB.Core.Controllers
{
    /// <summary>
    /// Automatically converts null or empty collection responses to HTTP 204 No Content.
    /// Can be applied at method, controller level, or configured globally for all controllers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Global setup (applies to ALL controllers):</b>
    /// Add to Program.cs:
    /// <code>
    /// builder.Services.AddControllers(options => 
    /// {
    ///     options.Filters.Add&lt;AutoNoContentAttribute&gt;();
    /// });
    /// </code>
    /// </para>
    /// <para>
    /// <b>Behavior examples:</b>
    /// <list type="bullet">
    /// <item><description>Null object → 204 No Content</description></item>
    /// <item><description>Empty collection → 204 (when TreatEmptyCollectionsAsNoContent=true)</description></item>
    /// <item><description>Empty collection → 200 OK (when TreatEmptyCollectionsAsNoContent=false)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// When applied globally, individual controllers/methods can override the behavior
    /// by declaring their own [AutoNoContent] attribute with custom settings.
    /// </para>
    /// </remarks>
    /// <example>
    /// <b>Method-level usage:</b>
    /// <code>
    /// [HttpGet]
    /// [AutoNoContent]
    /// public IActionResult Get() => Ok(_service.GetItems());
    /// </code>
    /// 
    /// <b>Controller-level usage:</b>
    /// <code>
    /// [ApiController]
    /// [AutoNoContent(TreatEmptyCollectionsAsNoContent = false)]
    /// public class ProductsController : ControllerBase
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class AutoNoContentAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Determines whether empty collections should be converted to 204 No Content.
        /// Default: true
        /// </summary>
        public bool TreatEmptyCollectionsAsNoContent { get; init; } = true;

        /// <summary>
        /// Inspects the action result and converts to 204 No Content when appropriate
        /// </summary>
        /// <param name="context">The action execution context</param>
        /// <remarks>
        /// Conversion rules:
        /// 1. Always converts null results to 204
        /// 2. Converts empty collections to 204 when TreatEmptyCollectionsAsNoContent = true
        /// </remarks>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value == null ||
                    (TreatEmptyCollectionsAsNoContent && IsEmptyCollection(objectResult.Value)))
                {
                    context.Result = new NoContentResult();
                }
            }
        }

        /// <summary>
        /// Checks if an object is an empty collection
        /// </summary>
        /// <param name="value">The object to check</param>
        /// <returns>True if the object is an empty collection</returns>
        /// <example>
        /// IsEmptyCollection(new List<string>()) // returns true
        /// IsEmptyCollection(null) // returns false
        /// IsEmptyCollection(new List<string> { "a" }) // returns false
        /// </example>
        private static bool IsEmptyCollection(object value)
        {
            return value is IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext();
        }
    }
}
