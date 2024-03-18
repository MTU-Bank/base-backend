namespace MTUBankBase.ServiceManager
{
    /// <summary>
    /// Use this attribute for methods that can only be accessed by authenticated users.
    /// </summary>
    public class RequiresAuthAttribute : Attribute
    {
    }
}