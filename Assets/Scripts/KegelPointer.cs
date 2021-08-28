using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KegelPointer : MonoBehaviour
{

    private Transform _ballTransform;
    
    void Start()
    {
        _ballTransform = FindObjectOfType<BallController>().transform;
    }

    void Update()
    {
        var ballPosition = _ballTransform.position;
        var ballPositionXZ = new Vector3(ballPosition.x, 0f, ballPosition.z);
        
        var targetPosition = GameManager.instance.FindClosestRatPositionTo(ballPosition);
        var targetPositionXZ = new Vector3(targetPosition.x, 0f, targetPosition.z);
        
        var direction = targetPositionXZ - ballPositionXZ;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
    }
}
