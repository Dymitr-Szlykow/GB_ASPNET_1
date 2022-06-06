namespace GB.ASPNET.WebStore.Interfaces;

public static class WebApiRoutes
{
    public class V1
    {
        public const string CatalogRoute = "api/v1/catalog";
        public const string EmployeesRoute = "api/v1/employees";
        public const string IdentityRolesRoute = "api/v1/roles";
        public const string IdentityUsersRoute = "api/v1/usersmutual";
        public const string IdentityUserRoleStoreRoute = "api/v1/users/role";
        public const string IdentityUserPasswordStoreRoute = "api/v1/users/password";
        public const string IdentityUserEmailStoreRoute = "api/v1/users/email";
        public const string IdentityPhoneNumberStoreRoute = "api/v1/users/phone";
        public const string IdentityTwoFactorStoreRoute = "api/v1/users/tf";
        public const string IdentityUserLoginStoreRoute = "api/v1/users/login";
        public const string IdentityUserClaimStoreRoute = "api/v1/users/claim";
        public const string OrdersRoute = "api/v1/orders";
        public const string ValuesRoute = "api/v1/values";
    }
}