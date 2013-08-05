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
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TCore  {
	
	private static List<TNode> _nodes = new List<TNode>();
	private static Dictionary<string, int> _tagsCount = new Dictionary<string, int>();
	
	public static bool isInited = false;
	public static TODOMainView view = null;
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public static void init()  {
		if(isInited) {
			return;
		}
		
		DirSearch("Assets/");
		
		isInited = true;
	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public static int getTagCount(string tagName) {
		if(_tagsCount.ContainsKey(tagName)) {
			return _tagsCount[tagName];
		} else {
			return 0;
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	public static List<TNode> nodes {
		get {
			return _nodes;
		}
	}
	
	public static TODOListSize size  {
		get {
			if(EditorPrefs.HasKey("TODOListSize")) {
				switch(EditorPrefs.GetInt("TODOListSize")) {
				case 1:
					return TODOListSize.SMALL;
				default:
					return TODOListSize.LARGE;
				}
			} else {
				return TODOListSize.LARGE;
			}
		}
		
		set {
			switch(value) {
			case TODOListSize.SMALL:
				EditorPrefs.SetInt("TODOListSize", 1);
				break;
			default:
				EditorPrefs.SetInt("TODOListSize", 0);
				break;
				
			}
		}
	}
	
	public static bool isCurentSize(TODOListSize _size) {
		if(_size == size) {
			return true;
		} else {
			return false;
		}
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	private static void DirSearch(string sDir) {
       try {
			DirectoryInfo dir = new DirectoryInfo(sDir);
			
           foreach (DirectoryInfo d in dir.GetDirectories()) {
               foreach (FileInfo f in d.GetFiles())   {
                   if(f.Extension.Equals(".cs") || f.Extension.Equals(".js")) {
						string name = f.Name.Substring(0, f.Name.Length - 3);
						if(name.Equals("TFileParser") || name.Equals("TConfig")) {
							continue;
						}
						
						List<TNode> tpls = TFileParser.ParseFile(f.Name, f.FullName);
						foreach(TNode node in tpls) {
							registerTga(node.tag);
							_nodes.Add(node);
						}

					}
               }
				
               DirSearch(d.FullName);
           }
			
		
       } catch (System.Exception excpt) {
           Debug.LogWarning(excpt.Message);
       }
   }
	
	
	private static void registerTga(string tagName) {
		if(_tagsCount.ContainsKey(tagName)) {
			_tagsCount[tagName]++;
		} else {
			_tagsCount.Add(tagName, 1);
		}
	}
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
