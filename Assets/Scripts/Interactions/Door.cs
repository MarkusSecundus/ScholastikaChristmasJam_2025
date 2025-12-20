using UnityEngine;

public class Door : IActionable
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
	protected override void DoInteract_impl()
	{
		if((_lastInteractionTimestamp + InteractionDuration) <= Time.timeAsDouble)
		{
			_joint.targetRotation = (_joint.targetRotation == _defaultRotation) ? _openRotation : _defaultRotation;
			_lastInteractionTimestamp = Time.timeAsDouble;
		}
	}
}
