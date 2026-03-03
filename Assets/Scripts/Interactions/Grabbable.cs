
using MarkusSecundus.Utils.Extensions;
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


	public void OnGrabStart(PlayerController player)
	{
		CallInteractionHooks();
		if (ByPhysics)
		{
			if (DisableGrabityWhenHeld) this.Rigidbody.useGravity = false;
		}
		else
		{
			_setIgnoreCollisionsRecursive(player, true);

			this.Rigidbody.isKinematic = true;
			foreach (var joint in GetComponents<Joint>())
			{
				Destroy(joint);
			}
		}
	}
	public void OnGrabEnd(PlayerController player)
	{
		if (ByPhysics) this.Rigidbody.useGravity = true;
		else
		{
			_setIgnoreCollisionsRecursive(player, false);
			this.Rigidbody.isKinematic = false;
		}
	}

	private void Start()
	{
		this.Rigidbody = GetComponent<Rigidbody>();
		if (!ByPhysics)
		{
			//this.transform.ForeachDescendant(ch => ch.gameObject.layer = 3); // Grabbable
		}
	}

	void _setIgnoreCollisionsRecursive(PlayerController player, bool ignoreCollisions)
	{
		var charController = player.GetComponent<CharacterController>();
		foreach (var col in this.GetComponentsInChildren<Collider>(true))
		{
			Physics.IgnoreCollision(col, charController, ignoreCollisions);

		}
	}
}