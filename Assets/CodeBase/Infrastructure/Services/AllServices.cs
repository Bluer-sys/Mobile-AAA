namespace CodeBase.Infrastructure.Services
{
    public class AllServices
    {
        private static AllServices instance;
        public static AllServices Container => instance ?? (instance = new AllServices()); // Singleton

        public void RegisterSingle<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }

        public TService Single<TService>() where TService : IService
        {
            return Implementation<TService>.ServiceInstance;
        }

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}