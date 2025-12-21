using MarkusSecundus.Utils.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

internal class FinalBehavior : MonoBehaviour
{
	PlayerController _player;

	[SerializeField] Transform _direction;

	Vector3 Direction => (_direction.position - _direction.parent.position).normalized;
	[SerializeField] UnityEvent OnNotSeeingDoor; 
	[SerializeField] UnityEvent LaterOnSeeingDoor;
	[SerializeField] float mult = 1f;
	private void Start()
	{
		_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}

	bool didEmitOnNotSeeing = false;
	bool didEmitLaterOnSeeingDoor = false;
	private void Update()
	{
		if (!_playerIsHere) return;

		var dir = _player.transform.forward.Dot(Direction) * mult;
		if(!didEmitOnNotSeeing)
		{
			if(dir < 0f)
			{
				Debug.Log($"OnNotSeeing");
				didEmitOnNotSeeing = true;
				OnNotSeeingDoor?.Invoke();
			}
		}
		else if(!didEmitLaterOnSeeingDoor)
		{
			if(dir > 0f)
			{
				Debug.Log($"LaterOnSeeing");
				didEmitLaterOnSeeingDoor = true;
				LaterOnSeeingDoor?.Invoke();
			}
		}
	}



	bool _playerIsHere;
	private void OnTriggerEnter(Collider other)
	{
		if ((!other.gameObject.CompareTag("Player"))) return;
		Debug.Log($"Player is here");
		_playerIsHere = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if ((!other.gameObject.CompareTag("Player"))) return;
		Debug.Log($"NO Player here");
		_playerIsHere = false;
	}
}