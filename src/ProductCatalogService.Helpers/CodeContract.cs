using System;

namespace ProductCatalogService.Helpers
{
    public static class Contract
    {
        public static void Requires<TException>(bool condition, string message) where TException : Exception
        {
            if (!condition) throw Activator.CreateInstance(typeof(TException), message) as TException;
        }
    }
}