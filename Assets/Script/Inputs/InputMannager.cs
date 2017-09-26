﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMannager : MonoBehaviour 
{
	private static List<InputSet> inputSet = new List<InputSet>();

	public static void AddSet (InputSet set)
	{
		inputSet.Add(set);
	}

	public static void RemoveSet (InputSet set)
	{
		inputSet.Remove(set);
	}

	public static void Clear ()
	{
		inputSet.Clear();
	}

	void Update ()
	{
		foreach (InputSet set in inputSet)
		{
			if (set.isActive)
			{
				List<InputBinder> inputList = set.GetInput();
				foreach (InputBinder input in inputList)
				{
					if (Input.GetButtonDown(set.GetName() + " " + input.GetName()))
						input.Play(InputType.DOWN);
					else if (Input.GetButtonUp(set.GetName() + " " + input.GetName()))
						input.Play(InputType.UP);
				}
			}
		}
	}

}