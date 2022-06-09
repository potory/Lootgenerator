namespace LootGenerator.Contracts;

public static class ApiRoutes
{
    public const string Api = "api";
    public const string Version = "v1";

    public const string Root = $"{Api}/{Version}";
    
    public static class Collection
    {
        public const string Get = $"{Root}/collection";
        public const string Post = $"{Root}/collection";
        public const string Put = $"{Root}/collection";
        public const string Delete = $"{Root}/collection";
        public const string List = $"{Root}/collection/list";
    }
    
    public static class Item
    {
        public const string Get = $"{Root}/collection";
        public const string Post = $"{Root}/collection";
        public const string Put = $"{Root}/collection";
        public const string Delete = $"{Root}/collection";
        public const string List = $"{Root}/collection/list";
    }
}