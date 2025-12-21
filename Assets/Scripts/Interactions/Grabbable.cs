
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : IInteractable
{

	public override bool CanInteract() => true;

	public Transform GrabPoint;
	public float ThrowForceMultiplier = 1.0f;
	public float HoldDistanceMultiplier = 1.0f;
	public float HoldForceMultiplier = 1.0f;
	public bool ByPhysics = false;
	public bool WhenHeld = false;
	public bool DisableGrabityWhenHeld = false;

	public Rigidbody Rigidbody { get; private set; }

	public void OnGrabStart()
	{
		CallInteractionHooks();
		if (ByPhysics)
		{
			if (DisableGrabityWhenHeld) this.Rigidbody.useGravity = false;
		}
		else
		{
			this.Rigidbody.isKinematic = true;
			foreach (var joint in GetComponents<Joint>())
			{
				Destroy(joint);
			}
		}
	}
	public void OnGrabEnd()
	{
		if (ByPhysics) this.Rigidbody.useGravity = true;
		else this.Rigidbody.isKinematic = false;
	}

	private void Start()
	{
		this.Rigidbody = GetComponent<Rigidbody>();
	}
}