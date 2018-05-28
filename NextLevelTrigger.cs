using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextLevelTrigger : 
	MonoBehaviour
{
	public LevelProgressionRunnerReference runner;

	public void Trigger()
	{
		if (runner.Get(this).HasNext())
		{
			runner.Get(this).LoadNext();
		}
		else
		{
			OnProgressionFinished.Invoke();
		}
	}
	
	public UnityEvent OnProgressionFinished = new UnityEvent();
}
