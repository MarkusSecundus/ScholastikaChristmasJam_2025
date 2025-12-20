using UnityEngine;



public abstract class IInteractable : MonoBehaviour
{
	public abstract bool CanInteract();

	public virtual void OnHover() { }

	public static IInteractable Get(Collider col)
	{
		if (!col) return null;
		if (!col.attachedRigidbody) return col.GetComponent<IInteractable>();
		return col.attachedRigidbody.GetComponent<IInteractable>();
	}
}

public abstract class IActionable : IInteractable
{
	public abstract void DoInteract();
}
