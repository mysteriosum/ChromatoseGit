using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TODOMainView: EditorWindow {
	
	private string search = string.Empty;
	private Vector2 scrollPos = Vector2.zero;
	private float maxYPos = 0f;



	public static float TAGS_PANLE_WIDTH = 250f;
	
	protected List<TagTemplate> _tags;
	
	private string selectedTagName = string.Empty;
	
	
	public Texture2D  selectionTexture = null;
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
   // [MenuItem("Window/TODO List")]
    static void init() {
        EditorWindow.GetWindow<TODOMainView>();
    }
	
	
	private void initializeView() {
		createSelectionTexture();
		
		_tags =  new List<TagTemplate>();
		TagTemplate tpl = new TagTemplate("#All", "#All");
		tpl.isAllTag = true;

		_tags.Add(tpl);
		
		TCore.view = this;
		initTags ();
		TCore.init();
		minSize = new Vector2(700, 320);
	}
	
	public void createSelectionTexture() {
		Color c;
		if(!TODOUtils.isLightMode) {
			c = new Color(53f/255f, 64f/255f, 53f/255f);
		} else {
			c = new Color(61f/255f, 128f/255f, 223f/255f);
			c = new Color(143f/255f, 143f/255f, 143f/255f);
		}
	


			
		selectionTexture = new Texture2D(1, 1, TextureFormat.ARGB32, true);
		selectionTexture.SetPixel(0, 1, c);
        selectionTexture.Apply();
		selectionTexture.hideFlags = HideFlags.HideAndDontSave;
	
	}
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------
	
		

	protected virtual void registerAssembly() {
		//ArchitectCore.curentAssembly = ArchitectAssemblyHelper.getCurentAssembly();
	}

	public void OnInspectorUpdate() {
		// This will only get called 10 times per second.
		Repaint();
	}
	
    void OnGUI() {

		//position.width = 400f;
		if(!TCore.isInited || _tags == null) {
			initializeView();
		}
		title = "TODO List";
		
		
		checkTagsSelection();
		renderTopMenu();
		drawLeftMenu ();
		RenderTODOList ();
		processInput ();

    }


	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	
	
	private void checkTagsSelection() {
		
		foreach(TagTemplate tpl in tags) {
			if(tpl.isSelected) {
				if(tpl.isAllTag) {
					selectedTagName = string.Empty;
				} else {
					selectedTagName = tpl.name;
				}
				
				return;
			}	
		}	
		
		foreach(TagTemplate tpl in tags) {
			if(tpl.isAllTag) {
				tpl.isSelected = true;
				selectedTagName = string.Empty;
			}
		}
	}
	
	private void  SubMenuCallBack (object obj) {
       TODOMenuItem item = (TODOMenuItem) obj;
		switch(item) {
		case TODOMenuItem.SIZE_LARGE:
			TCore.size = TODOListSize.LARGE;
			break;
		case TODOMenuItem.SIZE_SMALL:
			TCore.size = TODOListSize.SMALL;
			break;
		case TODOMenuItem.DOCUMENTATION:
			Application.OpenURL ("http://goo.gl/AU0Cf");
			break;
		}
    }
	
	private void RightMouseClick() {
		GenericMenu menu = new GenericMenu ();
		
		
		menu.AddItem (new GUIContent ("Size/Small"), TCore.isCurentSize(TODOListSize.SMALL), SubMenuCallBack, TODOMenuItem.SIZE_SMALL);
		menu.AddItem (new GUIContent ("Size/Large"), TCore.isCurentSize(TODOListSize.LARGE), SubMenuCallBack, TODOMenuItem.SIZE_LARGE);
		menu.AddSeparator ("");
		menu.AddItem (new GUIContent ("Documentation"), false, SubMenuCallBack, TODOMenuItem.DOCUMENTATION);
		
		menu.ShowAsContext ();
	}
	
	private void LeftMouseClick() {
		
		float RECT_H = 46f;
		if(TCore.size == TODOListSize.SMALL) {
			RECT_H = 29f;
		}
		
		TNode selectedNode = null;
		Vector2 pos = Event.current.mousePosition;
		pos.y += scrollPos.y;


		//pos.x = position.width - pos.x;
		pos.x -= TAGS_PANLE_WIDTH;

		foreach (TNode node in TCore.nodes) {
			Rect r = node.rect;
			r.y += RECT_H;
			if(r.Contains(pos)) {
				if(!node.isSelected) {
					selectedNode = node;
				} else {
					OpenClass (node.filePath, node.line);
				}
			}
		}

		foreach (TNode node in TCore.nodes) {
			node.isSelected = false;
		}

		if(selectedNode != null) {
			selectedNode.isSelected = true;
		}



		pos.x += TAGS_PANLE_WIDTH;
		TagTemplate selectedTag = null;

		foreach (TagTemplate tag in tags) {
			Rect r = tag.rect;
			r.y += RECT_H;

			if(r.Contains(pos)) {
				selectedTag = tag;
			}
		}

		

		if(selectedTag != null) {
			
			foreach (TagTemplate tag in tags)  {
				tag.isSelected = false;
			}
			
			selectedTag.isSelected = true;
		}


		Repaint();
	}





	public virtual void OpenClass(string classPath, int line) {

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

	}


	public virtual void initTags() {

	}

	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public List<TagTemplate> tags {
		get {
			return _tags;
		}
	}

	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------

	private void drawLeftMenu() {
		GUILayout.BeginArea(new Rect (TAGS_PANLE_WIDTH, 0, 2, position.height), ""); {
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.Width(1f), GUILayout.ExpandHeight(true)});
		} GUILayout.EndArea ();
	}

	private void renderTopMenu() {
		
		TAGS_PANLE_WIDTH = 250f;
		
		float TOP_PANEL_HEIGHT = 40;
		int HEADR_FONT_SIZE = 20;
		int SEARCH_FILED_FONT_SIZE = 15;
		int SEARCH_FILED_MRGING = 10;
		int SEARCH_FILED_MRGING2 = 11;
		
		
		
		if(TCore.size == TODOListSize.SMALL) {
			TOP_PANEL_HEIGHT = 25;
			
			HEADR_FONT_SIZE = 13;
			SEARCH_FILED_FONT_SIZE = 12;
			SEARCH_FILED_MRGING = 4;
			SEARCH_FILED_MRGING2 = 2;
			
			TAGS_PANLE_WIDTH = 150f;
		}
		

		GUILayout.BeginArea(new Rect (0, TOP_PANEL_HEIGHT, position.width, 2f), ""); {
			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(2f)});
		} GUILayout.EndArea();


		GUILayout.BeginArea(new Rect (position.width - 230, SEARCH_FILED_MRGING, 70, 30), ""); {
			GUIStyle sl = new GUIStyle(EditorStyles.label);
			sl.fontSize = SEARCH_FILED_FONT_SIZE;

			GUILayout.Label("Search: ", sl);	
			GUILayout.Label("", sl);
		} GUILayout.EndArea();



		GUILayout.BeginArea(new Rect (position.width - 170, SEARCH_FILED_MRGING2, 150, 30), ""); {
			GUIStyle s = new GUIStyle(EditorStyles.textField);
			s.fontSize = 13;
			s.padding.top = 2;
			search = EditorGUILayout.TextField ("", search, s, new GUILayoutOption[]{GUILayout.Width(150), GUILayout.Height(20)});



		} GUILayout.EndArea();



		GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
		titleStyle.fontSize = HEADR_FONT_SIZE;
		int mx = 15;
		if(TCore.size == TODOListSize.SMALL) {
			mx = 10;
		}
		
		GUILayout.BeginArea(new Rect (mx, 7, 300, 40), ""); {
			GUILayout.Label("Tags:", titleStyle);	
		} GUILayout.EndArea();


		
		GUILayout.BeginArea(new Rect (TAGS_PANLE_WIDTH + mx, 7, 300, 40), ""); {
			string name = "TODO list";

			if(search != string.Empty) {
				name = "Searching: " + search;
			}

			GUILayout.Label(name, titleStyle);	
		} GUILayout.EndArea();



	}

	private void RenderTODOList() {
		
		float TOP_PANEL_MARGING = 40;
		if(TCore.size == TODOListSize.SMALL) {
			TOP_PANEL_MARGING = 25;
		}
		
		int i = 1;
		GUILayout.BeginArea (new Rect (0, TOP_PANEL_MARGING + 2, TAGS_PANLE_WIDTH, position.height - TOP_PANEL_MARGING), ""); {
			foreach(TagTemplate tpl in tags) {
				TagRenderer.renderTag (tpl, i);
				i++;
			}

		} GUILayout.EndArea();


		GUILayout.BeginArea(new Rect (TAGS_PANLE_WIDTH, TOP_PANEL_MARGING + 2, position.width - TAGS_PANLE_WIDTH, position.height - TOP_PANEL_MARGING), ""); {

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height));
			GUILayout.Box("", GUIStyle.none, new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(maxYPos)});

			i = 1;

			Rect DrawArea = position;
			DrawArea.width -= TAGS_PANLE_WIDTH;


			foreach(TNode node in TCore.nodes) {
				
				if(!selectedTagName.Equals(string.Empty))  {
					if(node.tag != selectedTagName) {
						continue;
					}
				}
				
				if(search != string.Empty) {
					if(node.text.ToLower().Contains(search.ToLower())) {
						NodeRenderer.renderNode (node, i, DrawArea);
						i++;
					}
				} else {
					NodeRenderer.renderNode (node, i, DrawArea);
					i++;
				} 

			}

			maxYPos = i * 29 - 20;
			EditorGUILayout.EndScrollView();


		} GUILayout.EndArea();
	}


	private void processInput() {
		if (Event.current.type == EventType.MouseDown) {
			if(Event.current.button == 0) {
				LeftMouseClick ();
			}
			
			if(Event.current.button == 1) {
				RightMouseClick ();
			}
			
		}

	}
	
	

}
