using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
#endif

// ReSharper disable once CheckNamespace
namespace VContainer.Unity
{
	public abstract class BaseLifetimeScope : LifetimeScope, IInstaller
    {
        protected override void Configure(IContainerBuilder builder)
        {
	        foreach (Installer installer in GetComponents<Installer>())
		        installer.Install(builder);
	        Install(builder);
        }

        public abstract void Install(IContainerBuilder builder);

#if UNITY_EDITOR
	    [ContextMenu(nameof(CollectConstructibles))]
	    private void CollectConstructibles() => 
		    autoInjectGameObjects = GetComponentsInChildren<Component>()
		                            .Where(HasConstructMethod)
		                            .Select(c => c.gameObject)
		                            .Distinct()
		                            .ToList();

	    private bool HasConstructMethod(Component component) =>
		    component.GetType().GetMethods().Any(m => m.GetCustomAttribute<InjectAttribute>() != null);
#endif

    }
}