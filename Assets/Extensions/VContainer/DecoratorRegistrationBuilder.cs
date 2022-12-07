using System;

// ReSharper disable once CheckNamespace
namespace VContainer.Internal
{
    public sealed class DecoratorRegistrationBuilder : RegistrationBuilder
    {
	    private readonly RegistrationBuilder _inner;
        private readonly Type _decorateType;
        
        public DecoratorRegistrationBuilder(RegistrationBuilder inner, Type decoratorType) 
	        : base(decoratorType, inner.Lifetime)
        {
            _inner = inner;
            _decorateType = inner.InterfaceTypes != null ? inner.InterfaceTypes[0] : inner.ImplementationType;
            InterfaceTypes = inner.InterfaceTypes;
            As(_decorateType);
        }
    
        public override Registration Build()
        {
	        IInjector injector = InjectorCache.GetOrBuild(ImplementationType);
            Registration innerRegistration = _inner.Build();
            
            var provider = new FuncInstanceProvider(container =>
            {
	            object innerInstance = container.Resolve(innerRegistration);
	            var parameters = new IInjectParameter[Parameters?.Count ?? 1];
                Parameters?.CopyTo(parameters);
                parameters[^1] = new TypedParameter(_decorateType, innerInstance);
                return injector.CreateInstance(container, parameters);
            });
            
            return new Registration(ImplementationType, Lifetime, InterfaceTypes, provider);
        }
    }
}