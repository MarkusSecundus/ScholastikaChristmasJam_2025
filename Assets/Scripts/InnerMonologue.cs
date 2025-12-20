using System;
using TMPro;
using UnityEngine;

public class InnerMonologuePlayer : MonoBehaviour
{
	TMP_Text _text;
	AudioSource _audio;

	private void Start()
	{
		_text = GetComponent<TMP_Text>();
		_audio = GetComponent<AudioSource>();

	}

	public void DoPlay(string text, Action onFinished)
	{
		//text.
	}
}
