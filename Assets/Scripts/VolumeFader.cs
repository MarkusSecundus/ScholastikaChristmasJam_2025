using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class VolumeFader : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] float FadeDuration_seconds = 1.0f;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] float MinValue = 0.0f;
    [SerializeField] float MaxValue = 1.0f;
    [SerializeField] UnityEvent OnComplete;
    void Start()
    {
        _audio ??= GetComponent<AudioSource>();
    }

    public void DoFadeOut()
    {
        foreach (var byPlayerDistance in _audio.GetComponents<VolumeByPlayerDistance>()) byPlayerDistance.enabled = false;
        var tw = _audio.DOFade(MinValue, FadeDuration_seconds).SetEase(ease).OnComplete(()=> { 
            OnComplete.Invoke();
            if (MinValue <= 0f) _audio.gameObject.SetActive(false);
        });
    }

    public void DoFadeIn()
	{
		_audio.volume = MinValue;
		_audio.gameObject.SetActive(true);
        _audio.DOFade(MaxValue, FadeDuration_seconds).SetEase(ease).OnComplete(OnComplete.Invoke);
    }
}
