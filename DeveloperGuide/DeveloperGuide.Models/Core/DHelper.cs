
namespace DGuide.Infrastructure.Core
{
    public static class DHelper
    {
        public static string GetAssignedRoles(params string[] roles)
        {
            return string.Join(",", roles);
        }
    }
}