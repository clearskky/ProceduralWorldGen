using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private Camera mainCam;

    public BoxCollider2D topCameraBorder;
    public BoxCollider2D bottomCameraBorder;
    public BoxCollider2D leftCameraBorder;
    public BoxCollider2D rightCameraBorder;

    private void Start()
    {
        mainCam = GetComponent<Camera>();

        topCameraBorder.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0,0)).x, 1f);
        topCameraBorder.size = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 0.5f);

        bottomCameraBorder.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0, 0)).x, 1f);
        bottomCameraBorder.size = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - 0.5f);

        leftCameraBorder.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height * 2f, 0)).y);
        leftCameraBorder.size = new Vector2(0f, -0.5f);

        rightCameraBorder.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0, Screen.height * 2f, 0)).y);
        rightCameraBorder.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 0.5f, 0f);
    }

    private void LateUpdate()
    {
        
    }
}
