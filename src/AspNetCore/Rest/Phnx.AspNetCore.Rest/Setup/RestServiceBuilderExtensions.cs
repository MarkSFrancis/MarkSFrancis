﻿using Microsoft.Extensions.DependencyInjection;

namespace Phnx.AspNetCore.Rest.Setup
{
    /// <summary>
    /// Extentions for <see cref="IServiceCollection"/> related to <see cref="Rest"/>
    /// </summary>
    public static class RestServiceBuilderExtensions
    {
        /// <summary>
        /// Setup all necessary components for <see cref="Rest"/>
        /// </summary>
        /// <param name="services">The service injector</param>
        public static void RegisterRest(this IServiceCollection services)
        {
            var builder = new RestServiceBuilder(services);

            builder.RegisterETags();
            builder.RegisterContextHelpers();
            builder.RegisterRestHelpers();
        }
    }
}