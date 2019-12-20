using Logica.Decorators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logica.Utils
{
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services)
        {
            var handlerTypes = typeof(ICommand).Assembly.GetTypes()
                .Where(p => p.GetInterfaces().Any(q => IsHandlerInterface(q)))
                .Where(p => p.Name.EndsWith("Handler"))
                .ToList();

            foreach (var type in handlerTypes)
            {
                Addhandler(services, type);
            }
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType) return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>)
                || typeDefinition == typeof(IQueryHandler<,>);
        }

        private static void Addhandler(IServiceCollection services, Type type)
        {
            var attributes = type.GetCustomAttributes(false);

            var pipeline = attributes.Select(p => ToDecorator(p))
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            var interfaceType = type.GetInterfaces().Single(p => IsHandlerInterface(p));
            var factory = BuildPipeline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
        }

        public static Type ToDecorator(object attribute)
        {
            var type = attribute.GetType();

            if (type == typeof(DatabaseRetryAttribute))
                return typeof(DatabaseRetryDecorator<>);

            if (type == typeof(AuditLogAttribute))
                return typeof(AuditLoggingDecorator<>);

            throw new ArgumentException(attribute.ToString());
        }
        public static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline.Select(p =>
            {
                var type = p.IsGenericType ? p.MakeGenericType(interfaceType.GenericTypeArguments) : p;
                return type.GetConstructors().Single();
            }).ToList();

            return sp =>
            {
                object current = null;
                foreach (var ctor in ctors)
                {
                    var parameterInfos = ctor.GetParameters().ToList();
                    var parameters = GetParameters(parameterInfos, current, sp);
                    current = ctor.Invoke(parameters);
                }

                return current;
            };
        }
        private static object[] GetParameters(List<ParameterInfo> parametersInfo, object current, IServiceProvider serviceProvider)
        {
            var result = new object[parametersInfo.Count];

            for (int i = 0; i < parametersInfo.Count; i++)
            {
                result[i] = GetParameter(parametersInfo[i], current, serviceProvider);
            }

            return result;
        }
        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider serviceProvider)
        {
            var parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType)) return current;

            var service = serviceProvider.GetService(parameterType);

            if (service == null) throw new ArgumentException($"Type {parameterType} not found.");

            return service;
        }
    }
}
