﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moov2.OrchardCore.SEO.HostnameRedirects.Drivers;
using Moov2.OrchardCore.SEO.HostnameRedirects.Services;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using System;

namespace Moov2.OrchardCore.SEO.HostnameRedirects {
    [Feature("Moov2.OrchardCore.SEO.HostnameRedirects")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) {
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, HostnameRedirectsSettingsDisplayDriver>();
            services.AddScoped<IPermissionProvider, Permissions>();

            services.AddSingleton<IHostRedirectService, HostRedirectService>();
            services.AddSingleton<IRewriteOptionsSevice, RewriteOptionsService>();

        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider) {
            var rewriteOptionsService = app.ApplicationServices.GetService<IRewriteOptionsSevice>();

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.Add((IRule)rewriteOptionsService);

            app.UseRewriter(rewriteOptions);

        }
    }
}
