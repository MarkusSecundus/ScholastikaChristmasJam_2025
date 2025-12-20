using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
	public abstract bool CanInteract();
	public abstract void DoInteract();

	public static IInteractable Get(Collider col)
	{
		if (!col) return null;
		if (!col.attachedRigidbody) return col.GetComponent<IInteractable>();
		return col.attachedRigidbody.GetComponent<IInteractable>();
	}
}

public abstract class IGrabbable : MonoBehaviour
{
	public abstract bool CanGrab();
}