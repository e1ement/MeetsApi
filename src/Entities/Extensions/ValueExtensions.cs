using Entities.Models;

namespace Entities.Extensions
{
    public static class ValueExtensions
    {
        public static void Map(this ValueEntity dbValue, ValueEntity value)
        {
            dbValue.Name = value.Name;
        }
    }
}
