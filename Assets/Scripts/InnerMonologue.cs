using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InnerMonologuePlayer : MonoBehaviour
{
	[SerializeField] bool OnlyOnce = true;
	[SerializeField] DialogBubble _target;
	[Multiline][SerializeField] string _text;
	[SerializeField] UnityEvent _onFinished;
	[SerializeField] float _bubbleDuration = 0.0f;
	[SerializeField] float _secondsPerChar = 0.0f;

	float? ifNonzero(float f) => f <= 0 ? null : f;

	bool _didPlay = false;
	public void DoPlay()
	{
		if (OnlyOnce && _didPlay) return;
		_didPlay = true;
		_target.StartPrintout(_text, true, _onFinished.Invoke,
			ifNonzero(_bubbleDuration),
			ifNonzero(_secondsPerChar)
		);
	}
}
