namespace TalentVerse.WebAPI.Common
{
    public class AppConstant
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Member = "Member";
            public const string Business = "Business";
        }

        public static class SuccessMessages
        {
            public const string RegistrationSuccessful = "Registration successful. Welcome to TalentVerse!";
            public const string LoginSuccessful = "Login successful!";
        }

        public static class ErrorMessages
        {
            public const string GenerricError = "An error occurred while processing your request, Please try again!";
            public const string InvalidLogin = "Invalid email or password.";
            public const string UserExists = "User with this email already exists.";
            public const string RoleAssignmentFailed = "Failed to assign role to the user.";
        }
    }
}
