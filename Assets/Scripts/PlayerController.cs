using DG.Tweening;
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

	[SerializeField] Vector3 _gravity = Vector3.down* 9.81f;

	CharacterController _char;

	Vector3 _cameraPointDefaultLocalPosition;
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		_char = GetComponent<CharacterController>();
		_cameraPointDefaultLocalPosition = CameraToUse.transform.localPosition;
	}

	void Update()
	{
		HandleCrouch();
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
		targetMovement += _gravity;

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

	[SerializeField] Transform _holdPoint;
	[SerializeField] float ThrowForce = 1f;
	[SerializeField] float HoldingForce = 1f;
	[SerializeField] float ForceHoldingForce = 1f;
	[SerializeField] ForceMode ForceHoldingMode = ForceMode.Force;

	bool IsInteractionKeyPressed => Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Mouse0);
	bool IsInteractionKeyBeingPressed => Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Mouse0);
	bool IsCrouchKeyPressed => Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.C);

	Grabbable _currentlyBeingHeld;

	Vector3 _hitPointLocal;
	Vector3 getGrabPoint()
	{
		//if (_currentlyBeingHeld.GrabPoint) return _currentlyBeingHeld.GrabPoint.position;
		return _currentlyBeingHeld.transform.LocalToGlobal(_hitPointLocal);
	}
	void HandleInteraction(float delta)
	{
		var raycastBase = _raycastDirection.parent.position;
		var raycastDirection = _raycastDirection.position - raycastBase;

		bool canInteract = false;

		if (_currentlyBeingHeld) 
		{
			if (_currentlyBeingHeld.WhenHeld? (!IsInteractionKeyBeingPressed) : IsInteractionKeyPressed)
			{
				_currentlyBeingHeld.OnGrabEnd();
				_currentlyBeingHeld.Rigidbody.AddForce(raycastDirection * ThrowForce * _currentlyBeingHeld.ThrowForceMultiplier, ForceMode.Impulse);
				_currentlyBeingHeld = null;
			}
			else
			{
				var holdPoint = _holdPoint.parent.position + (_holdPoint.position - _holdPoint.parent.position) * _currentlyBeingHeld.HoldDistanceMultiplier;
				var moveDir = holdPoint - getGrabPoint();
				if(_currentlyBeingHeld.ByPhysics)
					_currentlyBeingHeld.Rigidbody.SteerToVelocity(moveDir, ForceHoldingForce, ForceHoldingMode);
				else
					_currentlyBeingHeld.transform.position += moveDir * HoldingForce * delta;
			}
		}
		else if (Physics.Raycast(raycastBase, raycastDirection, out var hitInfo, _raycastDistance, raycastMask)){
			var interactable = IInteractable.Get(hitInfo.collider);
			if (interactable && interactable.CanInteract())
			{
				canInteract = true;
				interactable.OnHover();
				if (IsInteractionKeyPressed)
				{
					if (interactable is IActionable actionable)
					{
						actionable.DoInteract();
					}
					else if (interactable is Grabbable grabbable)
					{
						_currentlyBeingHeld = grabbable;
						_currentlyBeingHeld.OnGrabStart();
						_hitPointLocal = _currentlyBeingHeld.transform.GlobalToLocal(hitInfo.point);
					}
				}
			}
		}
		else
		{
			//Debug.Log($"no hit");
		}
			PressF_Label.gameObject.SetActive(canInteract);
	}


	[SerializeField] Transform CrouchCameraPoint;
	[SerializeField] float CrouchAnimationDuration_seconds;
	Tween _crouchTween;
	bool _isInCrouch = false;
	void HandleCrouch()
	{
		if (IsCrouchKeyPressed)
		{
			if (_crouchTween.IsNotNil() && _crouchTween.IsPlaying()) 
				_crouchTween.Kill();
			Vector3 targetPos = _isInCrouch ? _cameraPointDefaultLocalPosition : CrouchCameraPoint.localPosition;
			_isInCrouch = !_isInCrouch;
			_crouchTween = CameraToUse.DOLocalMove(targetPos, CrouchAnimationDuration_seconds);
		}
	}
}
