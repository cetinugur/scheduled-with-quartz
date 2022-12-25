using System.Text.Json;

namespace ScheduledJobs.Core.Extensions
{
    public static class GenericExtensions
    {
        public static bool IsEqual<T>(this T objA, T objB)
        {
            string myself = JsonSerializer.Serialize(objA);
            string other = JsonSerializer.Serialize(objB);

            return myself == other;
        }
    }
}
