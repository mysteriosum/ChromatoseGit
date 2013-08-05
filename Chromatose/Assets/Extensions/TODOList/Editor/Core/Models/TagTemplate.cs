using UnityEngine;
using System.Collections;

public class TagTemplate  {
	
	private string _patern;
	private string _name;

	public int id;
	public Rect rect;
	public bool isSelected = false;
	
	public bool isAllTag;


	public TagTemplate(string patern, string name) {
		_patern = patern;
		_name = name;
	}



	public string patern {
		get {
			return _patern;
		}
	}

	public string name {
		get {
			return _name;
		}
	}

}


