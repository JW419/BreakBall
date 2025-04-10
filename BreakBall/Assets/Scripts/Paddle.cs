using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    private void Awake()
    {
        if( _instance != null )
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion
    //Camera
    private Camera mainCamera;

    //Paddle Flick Movement Variables
    private float flickAngle = 90f;
    private float flickSpeed = 500f;
    //private float returnSpeed = 300f;

    private float targetAngle = 90f;
    private float currentAngle = 90f;
    //Paddle Positioning and Clamping
    private float paddleInitialY;
    private float defaultLeftClamp = 135;
    private float defaultRightClamp = 410;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
    }

    private void Update()
    {
        //float rotation = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            targetAngle = flickAngle + 180;
        }
        if (Input.GetKey(KeyCode.D))
        {
            targetAngle = -flickAngle;
        }

        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, flickSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

        if (Mathf.Approximately(currentAngle, targetAngle) && targetAngle != 90f)
        {
            targetAngle = 90f;
        }
        //paddle movement
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float leftClamp = defaultLeftClamp;
        float rightClamp = defaultRightClamp;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
