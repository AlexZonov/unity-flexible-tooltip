using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditorInternal;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace com.flexford.packages.tooltip.editor
{
	internal static class FlexibleTooltipMenuOptions
	{
		public const string CONTEXT_MENU_PATH = "Tools/Flexible Tooltip/";

		private const string INSTALL_MENU_PATH = CONTEXT_MENU_PATH + "Install";

		[MenuItem(INSTALL_MENU_PATH, false)]
		private static void InitAction(MenuCommand menuCommand)
		{
			Assembly currentAssembly = typeof(FlexibleTooltipMenuOptions).Assembly;
			PackageInfo currentPackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(currentAssembly);
			string installPackagePath = $"{currentPackageInfo.assetPath}/install.unitypackage";
			AssetDatabase.ImportPackage(installPackagePath, true);
			Debug.Log($"Install package '{currentPackageInfo.displayName}({currentPackageInfo.name})' completed");
		}
	}
}