#pragma strict



function OrderByName(tag) : GameObject[]{
	var _gos : GameObject[];
	var _names = new Array();
	var _final = new Array();
	
	_gos = GameObject.FindGameObjectsWithTag(tag);
	
	for (var go : GameObject in _gos){
		_names.Push(go.name);
	}
	
	_names.Sort();
	for(var name: String in _names){
		for (var go: GameObject in _gos){
			if(go.name == name){
			_final.Push(go);
			}
		}
	}
	return _final.ToBuiltin(GameObject);
}