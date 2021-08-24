using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;


namespace Volo.Abp.MultiLingualObjects
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpAutoMapperModule))]
    public class AbpMultiLingualObjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(typeof(AbpMultiLingualMapperAction<,,>));
        }
    }
}
