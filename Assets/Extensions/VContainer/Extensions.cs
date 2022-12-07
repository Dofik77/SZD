using System;
using Extensions.VContainer;
using VContainer.Internal;
using VContainer.Unity;

namespace VContainer
{
    public static class Extensions
    {
        public static void Decorate(this IContainerBuilder builder, Type interfaceType, Type decoratorType)
        {
	        if(VContainerSettings.DiagnosticsEnabled)
		        return;
	        
	        for (var i = 0; i < builder.Count; i++)
            {
                RegistrationBuilder entry = builder[i];
                bool isTarget = entry.ImplementationType.IsInterface && entry.ImplementationType == interfaceType;
                
                if (entry.InterfaceTypes is { } interfaceTypes)
                    foreach (Type t in interfaceTypes)
                        if (t == interfaceType)
                        {
                            isTarget = true;
                            break;
                        }

                if (isTarget)
	                builder[i] = new DecoratorRegistrationBuilder(builder[i], decoratorType);
            }
        }

        public static void Decorate<TInterface, TDecorator>(this IContainerBuilder builder)
            => builder.Decorate(typeof(TInterface), typeof(TDecorator));

        public static void RegisterFactoryFunc<TFactory, TOutput>(this IContainerBuilder builder) 
	        where TFactory : IFactory<TOutput>
        {
	        builder.Register<TFactory>(Lifetime.Singleton);
	        builder.RegisterFactory<TOutput>(
		        container => container.Resolve<TFactory>().Create, Lifetime.Singleton);
        }
        
        public static void RegisterFactoryFunc<TFactory, TInput, TOutput>(this IContainerBuilder builder) 
	        where TFactory : IFactory<TInput, TOutput>
        {
	        builder.Register<TFactory>(Lifetime.Singleton);
	        builder.RegisterFactory<TInput, TOutput>(
		        container => container.Resolve<TFactory>().Create, Lifetime.Singleton);
        }
    }
}
