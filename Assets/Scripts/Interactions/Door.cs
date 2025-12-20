using UnityEngine;

public class Door : IInteractable
{
	[SerializeField] Quaternion _openRotation;
	[SerializeField] float InteractionDuration = 1f;
	Quaternion _defaultRotation;

	ConfigurableJoint _joint;
	private void Start()
	{
		_joint = GetComponentInChildren<ConfigurableJoint>();
		_defaultRotation = _joint.targetRotation;
	}

	public override bool CanInteract() => true;

	double _lastInteractionTimestamp = float.NegativeInfinity;
	public override void DoInteract()
	{
		if((_lastInteractionTimestamp + InteractionDuration) <= Time.timeAsDouble)
		{
			_joint.targetRotation = (_joint.targetRotation == _defaultRotation) ? _openRotation : _defaultRotation;
			_lastInteractionTimestamp = Time.timeAsDouble;
		}
	}
}
