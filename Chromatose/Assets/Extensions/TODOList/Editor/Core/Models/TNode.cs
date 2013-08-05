////////////////////////////////////////////////////////////////////////////////
//
//  CRYSTAL CLEAR SOFTWARE
//  Copyright 2012 Crystal Clear Software. http://ccsoft.ru 
// All Rights Reserved. "$(ProjectName)" Project
// @author Osipov Stanislav lacost.20@gmail.com
// 
//
//  NOTICE: Crystal Soft does not allow to use, modify, or distribute this file
//  for any purpose
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TNode {
	
	public string fileName;
	public string filePath;
	
	private string _text;
	public int line;

	public Rect rect;

	public bool isSelected;

	public string tag;


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public string text {
		get {
			return _text;
		}
		
		set {
			_text = value;
			_text = "* " + _text.Trim();
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
