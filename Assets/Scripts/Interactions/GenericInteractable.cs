using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GenericInteractable : IActionable
{
	[SerializeField] public bool _canInteract = true;
	public void SetCanInteract(bool value) => _canInteract = value;
	public override bool CanInteract() => _canInteract;

	protected override void DoInteract_impl() { }
}
