using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class GA_Menu : MonoBehaviour
{
	[MenuItem ("Window/GameAnalytics/Select GA_Settings", false, 0)]
	static void SelectGASettings ()
	{
		Selection.activeObject = GA.SettingsGA;
	}
	
	[MenuItem ("Window/GameAnalytics/GA Setup Wizard", false, 100)]
	static void SetupAndTour ()
	{
		GA_SetupWizard wizard = ScriptableObject.CreateInstance<GA_SetupWizard> ();
		wizard.ShowUtility ();
		wizard.position = new Rect (150, 150, 420, 340);
	}
	
	[MenuItem ("Window/GameAnalytics/GA Example Tutorial", false, 101)]
	static void ExampleTutorial ()
	{
		GA_ExampleTutorial tutorial = ScriptableObject.CreateInstance<GA_ExampleTutorial> ();
		tutorial.ShowUtility ();
		tutorial.position = new Rect (150, 150, 420, 340);
	}
	
	[MenuItem ("Window/GameAnalytics/Create GA_SystemTracker", false, 200)]
	static void AddGASystemTracker ()
	{
		if (FindObjectOfType (typeof(GA_SystemTracker)) == null) {
			GameObject systemTracker = Instantiate (Resources.Load ("Prefabs/GA_SystemTracker", typeof(GameObject))) as GameObject;
			systemTracker.name = "GA_SystemTracker";
			Selection.activeObject = systemTracker;
		} else {
			GA.LogWarning ("A GA_SystemTracker already exists in this scene - you should never have more than one per scene!");
		}
	}
	
	[MenuItem ("Window/GameAnalytics/Create GA_Heatmap", false, 201)]
	static void AddHeatMap ()
	{
		GameObject heatmap = Instantiate (Resources.Load ("Prefabs/GA_HeatMap", typeof(GameObject))) as GameObject;
		heatmap.name = "GA_HeatMap";
		Selection.activeObject = heatmap;
	}
	
	[MenuItem ("Window/GameAnalytics/Add GA_Tracker to Object", false, 202)]
	static void AddGATracker ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<GA_Tracker> () == null) {
				Selection.activeGameObject.AddComponent<GA_Tracker> ();
			} else {
				GA.LogWarning ("That object already contains a GA_Tracker component.");
			}
		} else {
			GA.LogWarning ("You must select the gameobject you want to add the GA_Tracker component to.");
		}
	}
	
	[MenuItem ("Window/GameAnalytics/Open GA_Status Window", false, 300)]
	static void OpenGAStatus ()
	{
		GA_Status status = ScriptableObject.CreateInstance<GA_Status> ();
		status.Show ();
	}
	
	[MenuItem ("Window/GameAnalytics/PlayMaker/Enable (or Disable) Scripts", false, 400)]
	static void TogglePlayMaker ()
	{
		bool enabled = false;
		bool fail = false;
		
		string searchText = "#if false";
		string replaceText = "#if true";
		
		string filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendBusinessEvent.cs";
		string filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendBusinessEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendDesignEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendDesignEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendQualityEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendQualityEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		filePath = Application.dataPath + "/GameAnalytics/Plugins/Playmaker/SendUserEvent.cs";
		filePathJS = Application.dataPath + "/Plugins/GameAnalytics/Playmaker/SendUserEvent.cs";
		try {
			enabled = ReplaceInFile (filePath, searchText, replaceText);
		} catch {
			try {
				enabled = ReplaceInFile (filePathJS, searchText, replaceText);
			} catch {
				fail = true;
			}
		}
		
		AssetDatabase.Refresh();
		
		if (fail)
			Debug.Log("Failed to toggle PlayMaker Scripts.");
		else if (enabled)
			Debug.Log("Enabled PlayMaker Scripts.");
		else
			Debug.Log("Disabled PlayMaker Scripts.");
	}
	
	private static bool ReplaceInFile (string filePath, string searchText, string replaceText)
	{
		bool enabled = false;
		
		StreamReader reader = new StreamReader (filePath);
		string content = reader.ReadToEnd ();
		reader.Close ();
		
		if (content.StartsWith(searchText))
		{
			enabled = true;
			content = Regex.Replace (content, searchText, replaceText);
		}
		else
		{
			enabled = false;
			content = Regex.Replace (content, replaceText, searchText);
		}

		StreamWriter writer = new StreamWriter (filePath);
		writer.Write (content);
		writer.Close ();
		
		return enabled;
	}
	
	[MenuItem ("Window/GameAnalytics/Folder Structure/Switch to JS", false, 600)]
	static void JsFolders ()
	{
		if (!Directory.Exists(Application.dataPath + "/GameAnalytics/Plugins/"))
		{
			Debug.LogWarning("Folder structure incompatible, did you already switch to JS folder structure, or have you manually changed the folder structure?");
			return;
		}
		
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/Components", Application.dataPath + "/GameAnalytics/Components");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/Examples", Application.dataPath + "/GameAnalytics/Examples");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/Framework", Application.dataPath + "/GameAnalytics/Framework");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/iOS", Application.dataPath + "/GameAnalytics/iOS");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/Playmaker", Application.dataPath + "/GameAnalytics/Playmaker");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/Resources", Application.dataPath + "/GameAnalytics/Resources");
		Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/WebPlayer", Application.dataPath + "/GameAnalytics/WebPlayer");
		
		if (!Directory.Exists(Application.dataPath + "/Plugins/"))
			Directory.Move(Application.dataPath + "/GameAnalytics/Plugins/", Application.dataPath + "/Plugins");
		else
			Directory.Delete(Application.dataPath + "/GameAnalytics/Plugins/");
		
		if (!Directory.Exists(Application.dataPath + "/Editor/"))
			Directory.CreateDirectory(Application.dataPath + "/Editor/");
		
		Directory.Move(Application.dataPath + "/GameAnalytics/Editor", Application.dataPath + "/Editor/GameAnalytics");
		
		Directory.Move(Application.dataPath + "/GameAnalytics/", Application.dataPath + "/Plugins/GameAnalytics");
		
		AssetDatabase.Refresh();
	}
	
	[MenuItem ("Window/GameAnalytics/Folder Structure/Revert to original", false, 601)]
	static void RevertFolders ()
	{
		if (!Directory.Exists(Application.dataPath + "/Plugins/GameAnalytics/"))
		{
			Debug.LogWarning("Folder structure incompatible, are you already using original folder structure, or have you manually changed the folder structure?");
			return;
		}
		
		if (!Directory.Exists(Application.dataPath + "/Plugins/GameAnalytics/Plugins/"))
			Directory.CreateDirectory(Application.dataPath + "/Plugins/GameAnalytics/Plugins/");
		
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/Components", Application.dataPath + "/Plugins/GameAnalytics/Plugins/Components");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/Examples", Application.dataPath + "/Plugins/GameAnalytics/Plugins/Examples");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/Framework", Application.dataPath + "/Plugins/GameAnalytics/Plugins/Framework");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/iOS", Application.dataPath + "/Plugins/GameAnalytics/Plugins/iOS");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/Playmaker", Application.dataPath + "/Plugins/GameAnalytics/Plugins/Playmaker");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/Resources", Application.dataPath + "/Plugins/GameAnalytics/Plugins/Resources");
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics/WebPlayer", Application.dataPath + "/Plugins/GameAnalytics/Plugins/WebPlayer");
		
		Directory.Move(Application.dataPath + "/Plugins/GameAnalytics", Application.dataPath + "/GameAnalytics");
		
		Directory.Move(Application.dataPath + "/Editor/GameAnalytics", Application.dataPath + "/GameAnalytics/Editor");
		
		AssetDatabase.Refresh();
	}
}