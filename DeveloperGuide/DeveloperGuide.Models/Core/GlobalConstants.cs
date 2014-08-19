
namespace DGuide.Infrastructure.Core
{
    public static class DGuideAuthorize
    {
        /// <summary>
        /// Get to update and delete data
        /// </summary>
        public const string Administrators = "Administrators";
        
        /// <summary>
        /// Get to ask and answer questions
        /// </summary>
        public const string Users = "Users";

        /// <summary>
        /// Get to maintain questions and articles
        /// </summary>
        public const string UsersAndAdministrators = "Users, Administrators";
    }
}