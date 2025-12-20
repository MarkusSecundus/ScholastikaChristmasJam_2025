using UnityEngine;



public abstract class IInteractable : MonoBehaviour
{
	protected void CallInteractionHooks()
	{
		foreach(var hook in GetComponentsInChildren<InteractionHook>())
		{
			if (hook.isActiveAndEnabled)
				hook.Hook?.Invoke();
		}
	}
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
	public void DoInteract()
	{
		CallInteractionHooks();
		DoInteract_impl();
	}
	protected abstract void DoInteract_impl();
}
