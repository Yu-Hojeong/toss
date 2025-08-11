using UnityEngine;

public class FallingAnimal : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private bool isFalling = false;
    private float stopThreshold = 0.1f;
    private float stopTime = 1f;
    private float currentStopTime = 0f;
    private PlayerController playerControllerInstance;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckBlockStop();
    }

    private void CheckBlockStop()
    {
        if (isFalling && rb2d != null)
        {
            if (rb2d.linearVelocity.magnitude < stopThreshold)
            {
                
                currentStopTime += Time.deltaTime;
                if (currentStopTime >= stopTime)
                {
                    isFalling = false;    
                    playerControllerInstance.OnBlockStopped(); // PlayerController에 추가할 함수 호출
                    this.enabled = false;
                }
            }
            else
            {
                currentStopTime = 0f;
            }
        }
    }

    public void SetKinematic(bool isKinematic)
    {
        if (isKinematic)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.linearVelocity = Vector2.zero;
            // rb2d.angularVelocity = 0f;
            isFalling = false;
            // this.enabled = false;
        }
        else
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;

            rb2d.angularVelocity = 0f;
            isFalling = true;
            currentStopTime = 0f;
            this.enabled = true;
        }
    }
    
    public void SetPlayerController(PlayerController controller)
    {
        playerControllerInstance = controller;
    }


}