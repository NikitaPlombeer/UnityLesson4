using UnityEngine;
using UnityEngine.Serialization;

public class FearVoice : MonoBehaviour
{
    [FormerlySerializedAs("FearVoice")] public AudioSource fearVoiceSound;
    [FormerlySerializedAs("Player")] public Transform player;
    private float _timeFromLastVoice;
    private float _minTimeBetweenVoices = 60;

    private Vector3 PlayerPosition => player.position;

    void Update()
    {
        _timeFromLastVoice += Time.deltaTime;

        if (_timeFromLastVoice < _minTimeBetweenVoices)
        {
            return;
        }

        var closestRat = GameManager.instance.FindClosestRatPositionTo(PlayerPosition);
        var distanceToRat = Vector3.Distance(closestRat, PlayerPosition);
        if (distanceToRat < 16f)
        {
            fearVoiceSound.Play();
            _timeFromLastVoice = 0;
        }
        
    }
}
