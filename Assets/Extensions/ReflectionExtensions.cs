#if UNITY_EDITOR
using System;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace R1noff.Extensions
{
	public static class ReflectionExtensions
	{
		public static void SetField<T>(this object obj, string fieldName, T value)
		{
			FieldInfo fieldInfo = GetFieldInfo(obj, fieldName);
			fieldInfo.SetValue(obj, value);
		}

		public static T GetField<T>(this object obj, string fieldName)
		{
			FieldInfo fieldInfo = GetFieldInfo(obj, fieldName);
			object value = fieldInfo.GetValue(obj);;
			return value != null ? (T)value : default;
		}

		private static FieldInfo GetFieldInfo(object obj, string propName)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));
			Type type = obj.GetType();
			FieldInfo fieldInfo = null;
			while (fieldInfo == null && type != null)
			{
				fieldInfo = type.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				type = type.BaseType;
			}
			if (fieldInfo == null)
				throw new ArgumentOutOfRangeException(nameof(propName),
					$"Field {propName} was not found in Type {obj.GetType().FullName}");

			return fieldInfo;
		}
	}
}
#endif