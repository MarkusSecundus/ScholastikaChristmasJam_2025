using UnityEngine;

public class LustrDisabler : MonoBehaviour
{
	[SerializeField] Transform root;

	public void DoDisableAllLights()
	{
		foreach(var light in root.GetComponentsInChildren<Light>())
		{
			light.gameObject.SetActive(false);
		}
	}

}
