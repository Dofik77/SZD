using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

// ReSharper disable once CheckNamespace
namespace R1noff.Extensions
{
    public static class MenuItems
    {
	    private static EditorWindow _activeProject;
 
	    // [MenuItem("Tools/Toggle Lock")]
	    // private static void ToggleInspectorLock()
	    // {
		   //  ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
		   //  ActiveEditorTracker.sharedTracker.ForceRebuild();
	    // }
    
	    [MenuItem("Tools/Toggle Project Lock &#Q")]
	    private static void ToggleProjectLock()
	    {
		    if (_activeProject == null)
		    {
			    Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.ProjectBrowser");
			    Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
			    _activeProject = (EditorWindow)findObjectsOfTypeAll[0];
		    }
      
		    if (_activeProject != null && _activeProject.GetType().Name == "ProjectBrowser")
		    {
			    Type type = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.ProjectBrowser");
			    PropertyInfo propertyInfo = type.GetProperty("isLocked", BindingFlags.Instance | 
				    BindingFlags.NonPublic |
				    BindingFlags.Public);
        
			    bool value = (bool)propertyInfo.GetValue(_activeProject, null);
			    propertyInfo.SetValue(_activeProject, !value, null);
			    _activeProject.Repaint();
		    }
	    }
	    
	    [MenuItem("Tools/Ping #&L")]
	    private static void Ping()
	    {
		    if (Selection.activeObject == null)
		    {
			    string path = SceneManager.GetActiveScene().path;
			    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
		    }
		    else if (PrefabUtility.IsPartOfAnyPrefab(Selection.activeObject))
		    {
			    string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(Selection.activeObject);
			    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
		    }
		    else if (PrefabStageUtility.GetCurrentPrefabStage().IsPartOfPrefabContents(Selection.activeGameObject))
		    {
			    PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
			    string path = stage.assetPath;
			    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
		    }
		    if(Selection.activeObject != null)
				EditorGUIUtility.PingObject(Selection.activeObject);
	    }
    }
}
