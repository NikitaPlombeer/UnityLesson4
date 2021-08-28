using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform Target;
    public Rigidbody TargetRB;

    private readonly List<Vector3> _lastVelocities = new List<Vector3>();

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            _lastVelocities.Add(Vector3.zero);
        }
    }

    void Update()
    {
        AdjustLastVelocities();
        var direction = Quaternion.LookRotation(GetVelocitiesSum());
        
        transform.position = Target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, Time.deltaTime);
    }

    private void AdjustLastVelocities()
    {
        _lastVelocities.Add(TargetRB.velocity);
        _lastVelocities.RemoveAt(0);
    }

    private Vector3 GetVelocitiesSum()
    {
        Vector3 sumVelocity = Vector3.zero;
        foreach (var velocity in _lastVelocities)
        {
            sumVelocity += velocity;
        }

        return sumVelocity;
    }
    
    
}
