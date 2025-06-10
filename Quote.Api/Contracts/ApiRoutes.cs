namespace Quote.Api.Contracts
{
    public static class ApiRoutes
    {
        public static class Quotes
        {
            public const string GetAll = "";
            public const string GetById = "{quoteId:guid}";
            public const string Create = "quote";
            public const string Update = "{quoteId:guid}";
            public const string Delete = "{quoteId:guid}";
            public const string Random = "random";
        }
        public static class Subscriptions
        {
            public const string Subscribe = "subscriptions";
            public const string Unsubscribe = "subscriptions/{subscriptionId:guid}";
            public const string GetSentNotification = "sent";
            public const string GetMyNotifications = "my-notifications";
        }

    }
}