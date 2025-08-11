using UnityEngine;

public class CameraUpTrigger : MonoBehaviour
{
    public GameObject mainCam;
    public PlayerController playerController;
    public float upOffset = 2f;




    void OnTriggerStay2D(Collider2D collision)
    {
        if (playerController.GetComponentInChildren<FallingAnimal>() && collision.CompareTag("Block"))
        {
            Vector2 cameraPos = mainCam.transform.position;
            cameraPos = new Vector2(cameraPos.x, cameraPos.y + upOffset);
        }
        
    }
}
