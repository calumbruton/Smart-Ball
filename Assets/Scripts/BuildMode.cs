using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour {

	//turn on wall building mode
	public void TurnOnWalls(){
		CreateWalls.wall_build_mode = true;
		CreateStartLine.stln_build_mode = false;
	}

	//Turn on start line choice mode
	public void TurnOnStLn (){
		CreateWalls.wall_build_mode = false;
		CreateStartLine.stln_build_mode = true;
	}
}
