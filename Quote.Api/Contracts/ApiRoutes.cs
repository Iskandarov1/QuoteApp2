namespace Quote.Api.Contracts
{
    public static class ApiRoutes
    {
        public static class Quotes
        {
            public const string GetAll = "";
            public const string GetById = "quote/{Id:guid}";
            public const string Create = "quote";
            public const string Update = "quote/{Id:guid}";
            public const string Delete = "quote/{Id:guid}";
            public const string Random = "random";
        }
        public static class Subscriptions
        {
            public const string Subscribe = "subscribe";
            public const string Unsubscribe = "subscriber/{id:Guid}";
            public const string GetActiveSubscribers = "active";
            
        }
        
        public static class Categories
        {
            public const string GetById = "category/{id:Guid}";
            public const string CreateCategory = "category";
            public const string Delete = "category/{id:Guid}";
            public const string Create = "category";
        }

    }
}