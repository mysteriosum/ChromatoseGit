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

public class TFileParser {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public static List<TNode> ParseFile(string FileName, string FilePath) {
		
		List<TNode> nodes = new List<TNode>();
		
		string[] lines;
		if(FilePath.Equals(string.Empty)) {
			lines = new string[0];
		} else {
			lines = System.IO.File.ReadAllLines(FilePath); 
		}
		
		int index = 0;
		foreach(string line in lines) {
			index++;
			
			int ind;
			string comment = string.Empty;
			if(line.Contains("//")) {
				ind = line.IndexOf("//") + 2; 
				comment = line.Substring(ind, line.Length - ind);
			} else {
				//continue;
				if(line.Contains("/*")) {
					ind = line.IndexOf("/*") + 2; 
					comment = line.Substring(ind, line.Length - ind);
					
					if(comment.Contains("*/")) {
						ind = comment.IndexOf("*/"); 
						comment = comment.Substring(0, ind);
					}
				} else {
					continue;
				}
			}
			
			comment = comment.Trim();
			
			foreach(TagTemplate tag in TCore.view.tags) {
				if(tag.isAllTag) {
					continue;
				}
				
			
				if(comment.Length >= tag.patern.Length) {
					if(comment.Substring(0, tag.patern.Length).ToLower().Equals(tag.patern.ToLower())) {
						TNode tpl = new TNode();
						tpl.fileName = FileName;
						tpl.filePath = FilePath;
		
						tpl.tag = tag.name;
						tpl.text = comment.Substring(tag.patern.Length, comment.Length - tag.patern.Length);
						tpl.line = index;
						nodes.Add(tpl);
						break;
					}
				}
			}
		}
		
		return nodes;
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
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
