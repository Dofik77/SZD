using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.Assertions;

namespace SuperUnityBuild.BuildTool
{
	public static class CLIExtension
	{
		private const string ReleaseTypeArgumentName = "-type";

		private const string VersionArgumentName = "-updateVersion";
		
		public static void PerformBuild()
		{
			string[] args = Environment.GetCommandLineArgs();
			if (!args.Contains(ReleaseTypeArgumentName))
				throw new ArgumentException("Should provide release type with -type ... argument");
			
			string releaseName = args[Array.IndexOf(args, ReleaseTypeArgumentName) + 1];
			if (args.Contains(VersionArgumentName))
			{
				string version = args[Array.IndexOf(args, VersionArgumentName) + 1];
				if(!Regex.IsMatch(version, @"^(\d+\.)(\d+\.)0$"))
					throw new ArgumentException("Version should be in *.*.0 format");
				
				BuildSettings.productParameters.versionTemplate = $"{version.Remove(version.Length - 2)}.$BUILD";
				BuildSettings.productParameters.buildCounter = 0;
			}

			foreach (string keychain in BuildSettings.projectConfigurations.BuildAllKeychains())
				if(Regex.IsMatch(keychain, $"^({releaseName}/).*"))
					BuildProject.BuildSingle(keychain, BuildOptions.None);
		}
	}
}