using Application.Common.Mappings;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using Application.Users.Behaviors;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>),
                typeof(RequestValidationBehavior<,>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            services.AddAllRequestValidators();

            return services;
        }

        // NOTE: My apologies! If I had more time,
        //       I would have created a shorter method. ;)
        private static IServiceCollection AddAllRequestValidators(
            this IServiceCollection services)
        {
            var validatorType = typeof(IValidator<>);

            var validatorTypes = Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var requestType = validator.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IValidator<>))
                    .Select(i => i.GetGenericArguments()[0])
                    .First();

                var validatorInterface = validatorType
                    .MakeGenericType(requestType);

                services.AddTransient(validatorInterface, validator);
            }

            return services;
        }
    }
}
