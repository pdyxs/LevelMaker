using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelProgression", menuName = "Levels/Level Progression", order = 2)]
public class LevelProgression :
	ScriptableObject
{	
	public List<Level> levels;
}
