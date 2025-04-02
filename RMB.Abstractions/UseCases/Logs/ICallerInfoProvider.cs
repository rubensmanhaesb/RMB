/// <summary>
/// Interface responsible for retrieving caller information (class and method names).
/// </summary>
public interface ICallerInfoProvider
{
    /// <summary>
    /// Retrieves the class name and method name of the caller.
    /// </summary>
    /// <returns>A tuple containing the class name and method name.</returns>
    (string? ClassName, string? MethodName) GetCallerInfo();
}
