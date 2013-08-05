using UnityEngine;
using UnityEditor;
using System.Collections;

public class NodeRenderer  {
	
	
	public static void renderNode(TNode node, int index, Rect position) {

		int FONT_SIZE = 12;
		
		int d = 33;
		int w = 40;
		int m = 40;
		int l = 28;
		int x = 12;
		
		if(TCore.size == TODOListSize.SMALL) {
			FONT_SIZE = 11;
			m = 25;
			d = 20;
			w = 25;
			x = 5;
			l = 18;
		}


		node.rect = new Rect (0, index * m - d - x, position.width, w);


		
		if(index % 2 == 0 && !node.isSelected) {
			GUILayout.BeginArea(node.rect, ""); {	
				if(!TODOUtils.isLightMode) {
					GUILayout.Box("",  new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)});
				} else {
					Color GUIColor = GUI.color;
					float p = 233f;
					GUI.color = new Color(p/255f, p/255f, p/255f);
					GUILayout.Box("",  new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)});
					GUI.color = GUIColor;
				}

			} GUILayout.EndArea();
		}


		if(node.isSelected) {
			
			Rect r = node.rect;
			r.x += 1f;
			r.y += 1f;
			r.height -= 1f;
			
			if(TCore.view.selectionTexture == null) {
				TCore.view.createSelectionTexture();
			}
			
			GUI.DrawTexture(r, TCore.view.selectionTexture);

		}



	

		GUILayout.BeginArea(new Rect (15, index * m - d, position.width, 50), ""); {
			GUIStyle s;
			
			if(TCore.size == TODOListSize.SMALL) {
				s = new GUIStyle(EditorStyles.label);
			} else {
				s = new GUIStyle(EditorStyles.boldLabel);
			}
			
			s.fontSize = FONT_SIZE;
			GUILayout.Label(node.text, s);	
		} GUILayout.EndArea();


		GUILayout.BeginArea(new Rect (-20, index * m - d + l - 14, position.width, 50), ""); {
			GUIStyle s = new GUIStyle(EditorStyles.boldLabel);
			s.fontSize = 10;
			s.fontStyle = FontStyle.Italic;
			s.alignment = TextAnchor.LowerRight;
			GUILayout.Label(node.fileName, s);	
		} GUILayout.EndArea();



		Drawing.DrawDashLine(new Vector2(0, index * m - d + l), 10, position.width + 10);

	}
	
}

