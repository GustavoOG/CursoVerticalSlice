﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.
                RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            });
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
