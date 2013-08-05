using UnityEngine;
using UnityEditor;
using System.Collections;

public class TagRenderer : MonoBehaviour {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	//--------------------------------------
	// PUBLIC METHODS
	//--------------------------------------

	public static void renderTag(TagTemplate tpl, int index) {
		
		int FONT_SIZE = 12;
		
		
		int d = 30;
		int m = 40;
		int l = 25;
		int w = 40;
		int x = 15;
		int mx = 15;
		
		if(TCore.size == TODOListSize.SMALL) {
			FONT_SIZE = 9;
			m = 25;
			d = 20;
			w = 25;
			x = 5;
			l = 18;
			mx = 18;
		}

		tpl.rect = new Rect (0, index * m - d - x, TODOMainView.TAGS_PANLE_WIDTH, w);


		if(tpl.isSelected) {
			
			Rect r = tpl.rect;
			r.x += 1f;
			r.y += 1f;
			r.height -= 1f;
			
			if(TCore.view.selectionTexture == null) {
				TCore.view.createSelectionTexture();
			}
			
			GUI.DrawTexture(r, TCore.view.selectionTexture);

		}


		GUILayout.BeginArea(new Rect (mx, index * m - d, TODOMainView.TAGS_PANLE_WIDTH, 50), ""); {
			GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
			s.fontSize = FONT_SIZE;
			if(TCore.size == TODOListSize.SMALL) {
				s.fontStyle = FontStyle.Normal;
			}
			
			int count = TCore.getTagCount(tpl.name);
			
			if(tpl.isAllTag) {
				foreach(TagTemplate t in TCore.view.tags) {
					count += TCore.getTagCount(t.name);
				}
			} 
			
			GUILayout.Label(tpl.name + " (" +  count.ToString() + ")", s);	
		} GUILayout.EndArea();



		//Drawing.DrawLine(new Vector2(0, index * m - d + l) TODOMainView.TAGS_PANLE_WIDTH);
		Drawing.DrawDashLine(new Vector2(0, index * m - d + l), 10, TODOMainView.TAGS_PANLE_WIDTH);
	}
	
	//--------------------------------------
	// GET / SET
	//--------------------------------------
	
	//--------------------------------------
	// EVENTS
	//--------------------------------------
	
	//--------------------------------------
	// PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	// DESTROY
	//--------------------------------------
}
