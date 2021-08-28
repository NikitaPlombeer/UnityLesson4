using UnityEngine;
using UnityEngine.Serialization;

public class BallController : MonoBehaviour
{

    [FormerlySerializedAs("CameraCenter")] public Transform cameraCenter;
    [FormerlySerializedAs("Speed")] public float speed;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = Mathf.PI * 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLose && transform.position.y < -1)
        {
            GameManager.instance.Lose();
        }

        if (GameManager.instance.isLose)
        {
            if (transform.position.y < -7)
            {
                transform.position = new Vector3(0f, 25f, 0f);
            } else if (transform.position.y > 0f && transform.position.y < 10f)
            {
                GameManager.instance.RestartGame();
            }
            
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddTorque(cameraCenter.right * Input.GetAxis("Vertical") * speed);
        _rigidbody.AddTorque(-cameraCenter.forward * Input.GetAxis("Horizontal") * speed);
    }
}
