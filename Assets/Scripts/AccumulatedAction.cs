using MarkusSecundus.Utils.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class AccumulatedAction : MonoBehaviour
{
	[SerializeField] int Counter = 0;

	[SerializeField] SerializableDictionary<int, UnityEvent> Tiers;


	public void Increment(int amount)
	{
		Counter += amount;
		List<int> performed = null;
		foreach(var (i, action) in Tiers.Values)
		{
			if(i <= Counter)
			{
				performed ??= new();
				performed.Add(i);
				action?.Invoke();
			}
		}
		if(performed != null)
			foreach(var i in performed) (Tiers.Values as Dictionary<int, UnityEvent>).Remove(i);
	}

	public void Increment() => Increment(1);
}
