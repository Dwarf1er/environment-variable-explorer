using EnvironmentVariableExplorer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace EnvironmentVariableExplorer.Components
{
    public class LocalizedComponentBase<T> : LayoutComponentBase, IDisposable
    {
        [Inject] protected LanguageService LanguageService { get; set; }
        [Inject] private IServiceProvider ServiceProvider { get; set; }

        protected IStringLocalizer<T> Localizer { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Localizer = ServiceProvider.GetRequiredService<IStringLocalizer<T>>();
            LanguageService.OnLanguageChanged += StateHasChanged;
        }

        public void Dispose()
        {
            LanguageService.OnLanguageChanged -= StateHasChanged;
        }
    }
}
