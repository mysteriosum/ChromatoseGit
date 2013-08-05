using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TActions  {

	public static void OpenClass(string classPath, int line) {

		#if UNITY_EDITOR

		string AssetPath = classPath.Substring(Application.dataPath.Length, classPath.Length - Application.dataPath.Length);
		AssetPath = "Assets" + AssetPath;


		string guid = AssetDatabase.AssetPathToGUID(AssetPath);

		//	AssetDatabase.OpenAsset()

		// Check is Script Inspector 2 installed first...
		// Should be in the same assembly as your CodeArchitectureHelper class (or any other C# Editor class)

		var si2WindowType = System.Type.GetType("FGCodeWindow");
		if (si2WindowType != null)  {

			var openAssetInTab = si2WindowType.GetMethod("OpenAssetInTab", new System.Type[] { typeof(string), typeof(int) });
			if (openAssetInTab != null)  {
				openAssetInTab.Invoke(null, new object[] { guid, -1 });
				return;
			}

		} 

		UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(classPath, line);

		#endif
	}
}

