namespace Script.Networking
{
    public static class API
    {
        public static Networking.Request<Schemas.ChatResponse> Chat(Schemas.ChatRequest request)
        {
            return new Networking.Request<Schemas.ChatResponse>(request);
        }
    }
}