
using Unity.VisualScripting;
using UnityEngine;

public class Grabbable : IInteractable
{

	public override bool CanInteract() => true;

	public Transform GrabPoint;

	[DoNotSerialize]public Rigidbody Rigidbody;

	public void OnGrabStart()
	{
		this.Rigidbody.isKinematic = true;
	}
	public void OnGrabEnd()
	{
		this.Rigidbody.isKinematic = false;
	}

	private void Start()
	{
		this.Rigidbody = GetComponent<Rigidbody>();
	}
}