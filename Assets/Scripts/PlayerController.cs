using MarkusSecundus.Utils.Behaviors.Physics;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Physics;
using MarkusSecundus.Utils.Primitives;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[System.Serializable]
	public class InputMapping
	{
		public KeyCode Jump = KeyCode.None;
		public string WalkForwardBackward = "Vertical";
		public string StrafeLeftRight = "Horizontal";
		public string LookUpDown = "Mouse Y";
		public string RotateLeftRight = "Mouse X";
	}
	[System.Serializable]
	public class InputMultipliers
	{
		public float WalkSpeed = 1f;
		public float RotateSpeed = 1f;
		public float LookUpDownSpeed = 1f;
	}

	public Transform CameraToUse;

	//private new Rigidbody rigidbody;
	public InputMapping Mapping = new InputMapping();
	public InputMultipliers Tweaks = new InputMultipliers();

	public readonly struct MovementDirectionBasesList
	{
		readonly PlayerController self;

		internal MovementDirectionBasesList(PlayerController self) => this.self = self;

		public Vector3 WalkForwardBackwardBase => self.transform.forward;
		public Vector3 StrafeLeftRightBase => self.transform.right;
		public Vector3 RotateLeftRightAxis => self.transform.up;
	}
	public MovementDirectionBasesList MovementDirectionBases => new MovementDirectionBasesList(this);


	CharacterController _char;
	void Start()
	{
		_char = GetComponent<CharacterController>();
		InitLooking();
	}

	void LateUpdate()
	{
		DoHandleInputs(Time.deltaTime);
	}
	void FixedUpdate()
	{
		DoFixedMoveStep(Time.fixedDeltaTime);
	}


	float targetHorizontalRotation = 0f;
	float targetVerticalRotation = 0f;
	Vector3 targetMovement = Vector3.zero;
	void DoHandleInputs(float delta)
	{
		targetHorizontalRotation = Input.GetAxis("Mouse X") * Tweaks.RotateSpeed * delta;
		targetVerticalRotation = Input.GetAxis("Mouse Y") * Tweaks.LookUpDownSpeed * delta;
		targetMovement = MovementDirectionBases.WalkForwardBackwardBase * Input.GetAxis("Vertical") * Tweaks.WalkSpeed * delta;
		targetMovement += MovementDirectionBases.StrafeLeftRightBase * Input.GetAxis("Horizontal") * Tweaks.WalkSpeed * delta;

		_char.Move(targetMovement);
		_char.transform.localRotation *= Quaternion.AngleAxis(targetHorizontalRotation, _rotationBase);

		var camRotation = CameraToUse.localRotation.eulerAngles;
		camRotation.x -= targetVerticalRotation;
		CameraToUse.localRotation = Quaternion.Euler(camRotation);
	}

	[SerializeField] Vector3 _rotationBase = Vector3.up;


	void DoFixedMoveStep(float delta)
	{
	}



	Vector3 cameraRotation;
	void InitLooking()
	{
		cameraRotation = CameraToUse.transform.rotation.eulerAngles;
	}
}
