using System.Runtime.Serialization;

namespace Marketplace.Domain.Shared;

public static class DomainExceptions
{
    public class InvalidEntityStateException : Exception
    {
        public InvalidEntityStateException(object entity, string message)
         : base($"Entity {entity.GetType().Name} state change rejected, {message}")
        {
        }
    }

    public class ProfanityFound : Exception
    {
        public ProfanityFound(string text)
            : base($"Profantiy found in text: {text}")
        {
        }
    }
}
