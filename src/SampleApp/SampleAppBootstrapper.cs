namespace SampleApp
{
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;
    using Nancy.ViewEngines;

    public class SampleAppBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            ResourceViewLocationProvider.RootNamespaces.Add(typeof (SampleAppBootstrapper).Assembly, "SampleApp.Views");
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get { return NancyInternalConfiguration.WithOverrides(x => x.ViewLocationProvider = typeof(ResourceViewLocationProvider)); }
        }
    }
}