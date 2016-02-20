namespace Assets.Scripts.Services
{
    public class DependencyResolver
    {
        private static IDependencyResolver _dependencyResolver;

        public static IDependencyResolver Current
        {
            get
            {
                if (_dependencyResolver != null)
                {
                    return _dependencyResolver;
                }

                _dependencyResolver = new AutofacDependencyResolver();
                return _dependencyResolver;
            }
        }

        public static void SetResolver(IDependencyResolver resolver)
        {
            _dependencyResolver = resolver;
        }
    }
}