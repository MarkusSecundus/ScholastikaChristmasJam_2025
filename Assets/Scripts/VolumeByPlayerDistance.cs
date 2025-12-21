using MarkusSecundus.Utils.Behaviors.GameObjects;
using MarkusSecundus.Utils.Primitives;
using UnityEngine;

public class VolumeByPlayerDistance : MonoBehaviour
{
    [SerializeField] Interval<float> DistanceRange;
    [SerializeField] AnimationCurve Curve;
    [SerializeField] string TargetTag;

    AudioSource _audio;
    Transform _target;
	private void Start()
	{
        _audio = GetComponent<AudioSource>();
        _target = GameObject.FindWithTag(TargetTag).transform;
	}
	void Update()
    {
        var distance = _target.position.Distance(transform.position);
        var t = distance.Normalize(DistanceRange);
        var volumeValue = Curve.Evaluate(t);
        _audio.volume = volumeValue;
        
    }
}
