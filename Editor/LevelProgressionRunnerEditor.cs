using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelProgressionRunner))]
public class LevelProgressionRunnerEditor : 
	Editor
{
	public LevelProgressionRunner runner => target as LevelProgressionRunner;

	public ILevelActor actor => runner.actor.Get(runner);
	
	public FileInfo[] files {
		get {
			if (_files == null) {
				var dir = new DirectoryInfo(runner.filepath);
				if (dir.Exists)
					_files = dir.GetFiles("*" + Level.extension);
			}
			return _files;
		}
	}
	private FileInfo[] _files;

	public string[] fileNames {
		get {
			if ((_files == null || _fileNames == null) && files != null) {
				var names = System.Array.ConvertAll(files, (file) =>
				{
					return file.Name.Substring(0, file.Name.Length - Level.extension.Length);
				});
				var nlist = new List<string>(names);
				nlist.Insert(0, " ");
				_fileNames = nlist.ToArray();
			}
			return _fileNames;
		}
	}
	private string[] _fileNames;

	private string newLevelName;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		switch (runner.loadFrom)
		{
			case LevelProgressionRunner.LoadTarget.Level:
				DrawFileLevelChooser();
				break;
			case LevelProgressionRunner.LoadTarget.Progression:
				DrawProgressionLevelChooser();
				break;
		}
		
		if (Application.isPlaying && actor.hasLevel)
		{
			if (GUILayout.Button("Save Level"))
			{
				actor.Save();
			}
		}
	}

	private void DrawProgressionLevelChooser()
	{
		if (!Application.isPlaying)
		{
			if (runner.progression != null)
			{
				if (runner.level == null) runner.level = null;
				var array = runner.progression.levels.ToArray();
				runner.nextLevel = EditorGUILayout.Popup(
					"Level",
					runner.nextLevel,
					System.Array.ConvertAll(array, (a) => a.name)
				);
			}
		}
	}

	private void DrawFileLevelChooser()
	{
		if (fileNames != null)
		{
			using (new EditorGUILayout.HorizontalScope())
			{
				if (runner.level == null) runner.level = null;
                var index = EditorGUILayout.Popup(
                    "Level",
                    System.Array.IndexOf(fileNames, runner.level?.name),
                    fileNames
                );
                if (index > 0 && 
                    (runner.level == null ||
                     runner.level?.name != fileNames[index]
				 	))
                {
	                var name = $"{runner.filepath}{files[index - 1].Name}";
	                runner.level = AssetDatabase.LoadAssetAtPath<Level>(name);
                }
                else if (index <= 0)
                {
                    GUI.enabled = false;
                    runner.level = null;
                }

                if (GUILayout.Button("Load")) {
                    if (!Application.isPlaying)
                    {
                        actor.Clear();
                    }
	                actor.Load(runner.level);
                }
                GUI.enabled = true;

                if (GUILayout.Button("Clear"))
                {
	                actor.Clear();
                }
            }
			
			using (new EditorGUILayout.HorizontalScope())
			{
				newLevelName = EditorGUILayout.TextField("New Level:", newLevelName);
	
				GUI.enabled = (newLevelName != "");
	
				if (GUILayout.Button("Create", GUILayout.MaxWidth(50))) {
					runner.level = actor.Create(newLevelName, runner.filepath);
					_files = null;
					newLevelName = "";
				}
	
				GUI.enabled = true;
			}
		}
	}
}
