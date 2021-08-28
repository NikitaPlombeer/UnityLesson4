using UnityEngine;
using UnityEngine.Serialization;

public class MainLightController : MonoBehaviour
{
 
    [FormerlySerializedAs("Player")] public Transform player;
    [FormerlySerializedAs("Light")] public Light light;
    [FormerlySerializedAs("BackgroundSound")] public AudioSource backgroundSound;
    [FormerlySerializedAs("GoodColor")] public Color goodColor; 
    [FormerlySerializedAs("BadColor")] public Color badColor;


    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.position;
        transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

        Rect boundaries = GameManager.instance.boundaries;
        float maxDistance = boundaries.height / 2f;
        float distanceToBoundary = shortestDistanceToSide(boundaries);

        if (distanceToBoundary > maxDistance / 2f)
        {
            light.color = goodColor;
            backgroundSound.volume = 0.2f;
        }
        else
        {
            var t = 1 - distanceToBoundary / (maxDistance / 2f);
            light.color = Color.Lerp(goodColor, badColor, t);
            backgroundSound.volume = Mathf.Lerp(0.2f, 0.6f, t);
        }
    }

    private float shortestDistanceToSide(Rect boundaries)
    {
        var targetPosition = player.position;
        var minDistance = boundaries.xMax - targetPosition.x;
        
        var distance = targetPosition.x - boundaries.xMin;
        if (distance < minDistance) minDistance = distance;
        
        distance = boundaries.yMax - targetPosition.z;
        if (distance < minDistance) minDistance = distance;
        
        distance = targetPosition.z - boundaries.yMin;
        if (distance < minDistance) minDistance = distance;

        return minDistance;
    }
}
