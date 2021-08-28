using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RatController : MonoBehaviour
{
    [FormerlySerializedAs("BloodMarkPrefab")] public GameObject bloodMarkPrefab;
    [FormerlySerializedAs("VisionRadius")] public float visionRadius = 16f;
    [FormerlySerializedAs("Speed")] public float speed = 5f;
    
    private Transform _ballTransform;
    private bool _isSaved;
    
    private void Start()
    {
        _ballTransform = FindObjectOfType<BallController>().transform;
    }

    private void Update()
    {
        if (ShouldRun())
        {
            Run();
        }
    }


    private bool ShouldRun()
    {
        return Vector3.Distance(_ballTransform.position, transform.position) < visionRadius;
    }

    private void Run()
    {
        if (!_isSaved)
        {
            Vector3 ballPositionXZ = new Vector3(_ballTransform.position.x, 0f, _ballTransform.position.z);
            Vector3 ratPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        
            Vector3 runDirection = ratPositionXZ - ballPositionXZ;
            transform.rotation = Quaternion.LookRotation(runDirection);
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

            Vector2 ratPosition2d = new Vector2(ratPositionXZ.x, ratPositionXZ.z);
            if (!GameManager.instance.boundaries.Contains(ratPosition2d))
            {
                _isSaved = true;
                GameManager.instance.SaveRat(gameObject);
            }
        }
        else
        {
            FlyUp();
        }
    }

    private void MoveFromPlayer()
    {
        
    }

    private void FlyUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isSaved) return;
        
        var ball = other.attachedRigidbody.GetComponent<BallController>();
        if (!ball) return;
        
        Instantiate(bloodMarkPrefab, transform.position, transform.rotation);
        GameManager.instance.KillRat(gameObject);
    }
}