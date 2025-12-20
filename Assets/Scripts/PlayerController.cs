using MarkusSecundus.Utils.Behaviors.Physics;
using MarkusSecundus.Utils.Extensions;
using MarkusSecundus.Utils.Physics;
using MarkusSecundus.Utils.Primitives;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[System.Serializable]
	public class InputMultipliers
	{
		public float WalkSpeed = 1f;
		public float RotateSpeed = 1f;
		public float LookUpDownSpeed = 1f;
	}

	public Transform CameraToUse;

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
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		_char = GetComponent<CharacterController>();
	}

	void Update()
	{
		DoHandleInputs(Time.deltaTime);
		HandleInteraction(Time.deltaTime);
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

	[SerializeField] Transform _raycastDirection;
	[SerializeField] float _raycastDistance = 1f;
	[SerializeField] int raycastMask = 0;
	[SerializeField] RectTransform PressF_Label;
	void HandleInteraction(float delta)
	{
		var raycastBase = _raycastDirection.parent.position;
		var raycastDirection = _raycastDirection.position - raycastBase;

		bool canInteract = false;

		
		if (Physics.Raycast(raycastBase, raycastDirection, out var hitInfo, _raycastDistance, raycastMask)){
			var interactable = IInteractable.Get(hitInfo.collider);
			//if(hitInfo.collider) Debug.Log($"Raycast hit: {hitInfo.collider}", hitInfo.collider);
			if (interactable && interactable.CanInteract())
			{
				canInteract = true;
				if (Input.GetKeyDown(KeyCode.F))
				{
					interactable.DoInteract();
				}
			}
		}
		else
		{
			//Debug.Log($"no hit");
		}
			PressF_Label.gameObject.SetActive(canInteract);
	}
}
