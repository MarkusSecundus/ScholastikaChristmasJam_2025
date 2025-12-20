using DG.Tweening;
using MarkusSecundus.Utils.Primitives;
using MarkusSecundus.Utils.Randomness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBubble : MonoBehaviour
{
	[SerializeField] float perCharacter_seconds = 0.01f;
	[SerializeField] float endWait_seconds = 1.0f;
	//[SerializeField] float destroyFadeout_seconds = 1f;

	[SerializeField] AudioClip[] typingSounds;
	[SerializeField] int typingSoundsInterval = 5;

	[SerializeField] KeyCode[] skipPrintoutKeys = new KeyCode[] { KeyCode.Space, KeyCode.Mouse0 };

	[SerializeField] TMP_Text _textField_fld;
	TMP_Text _textField => _textField_fld ??= GetComponentInChildren<TMP_Text>();
	AudioSource _audioSource_fld;
	AudioSource _audioSource => _audioSource_fld ??= GetComponent<AudioSource>();

	[SerializeField] float _fadeDuration_seconds = 0.4f;

	event Action InterruptHandler;

	Image[] _renderers_fld;
	Image[] _renderers
	{
		get
		{
			if(_renderers_fld == null)
			{
				_renderers_fld = GetComponentsInChildren<Image>(true);
				foreach (var r in _renderers_fld) _initialAlphas[r] = r.color.a;
			}
			return _renderers_fld;
		}
	}
	Dictionary<Image, float> _initialAlphas = new();
	private void Start()
	{
	}

	public void StartPrintout(string text, bool shouldAutoclose, Action onClosed, float? durationOverride, float? charPerSecondsOverride)
	{
		var endWait_seconds = durationOverride ?? this.endWait_seconds;
		var perCharacter_seconds = charPerSecondsOverride ?? this.perCharacter_seconds;
		_textField.text = "";
		_textField.color = _textField.color.WithAlpha(1f);

		Tween lastTween = null;
		foreach (var r in _renderers)
		{
			if(! r.gameObject.activeInHierarchy) r.color = r.color.WithAlpha(0f);
			lastTween = r.DOFade(_initialAlphas[r], _fadeDuration_seconds);
		}
		gameObject.SetActive(true);
		if (lastTween != null)
			lastTween.OnComplete(() => StartCoroutine(printoutCoroutine()));
		else StartCoroutine(printoutCoroutine());

		IEnumerator printoutCoroutine()
		{
			var cameraTag = new object();
			yield return null;
			Debug.Log($"Initiating printout '{text}'");

			bool printoutFinished = false;
			bool displayFinished = false;
			bool endReguested = false;
			InterruptHandler += Interrupt;
			_textField.text = "";
			Debug.Log($"Starting printout", this);
			try
			{
				for (int t = 1; t <= text.Length; ++t)
				{
					yield return new WaitForSeconds(perCharacter_seconds);
					if (printoutFinished) t = text.Length;

					_textField.text = $@"{text[0..t]}<color=#00000000>{text[t..]}</color>";
					if (_audioSource && typingSounds.Length > 0 && typingSoundsInterval > 0 && t % typingSoundsInterval == 0)
						_audioSource.PlayOneShot(typingSounds.RandomElement());
				}
				printoutFinished = true;
				if (endWait_seconds > 0f)
					yield return new WaitForSeconds(endWait_seconds);
			}
			finally
			{
				displayFinished = true;
			}
			while (!(endReguested || shouldAutoclose)) yield return null;

			InterruptHandler -= Interrupt;

			if (_fadeDuration_seconds <= 0f)
				DoFinish();
			else
			{
				foreach (var r in _renderers) r.DOFade(0f, _fadeDuration_seconds);
				_textField.DOFade(0f, _fadeDuration_seconds).OnComplete(DoFinish);
			}

			void DoFinish()
			{
				if (shouldAutoclose)
				{
					Debug.Log($"Autoclosing!", this);
					gameObject.SetActive(false);
				}
				else { }

				onClosed?.Invoke();
			}

			void Interrupt()
			{
				printoutFinished = true;
				if (displayFinished)
					endReguested = true;
			}
		}
	}

	void Update()
	{
		foreach (var k in skipPrintoutKeys)
			if (Input.GetKeyDown(k))
			{
				Debug.Log($"Requesting end by keyboard!", this);
				InterruptHandler?.Invoke();
			}
	}

	public void InterruptRequest()
	{
		Debug.Log($"Requesting end!", this);
		InterruptHandler?.Invoke();
	}
}