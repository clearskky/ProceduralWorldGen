using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraFollow : MonoBehaviour
{
    public float tweenUpdateFrequency;
    private Player player;
    private float startingPosZ;

    private float yBoundryMin;
    private float yBoundryMax;
    private float xBoundryMin;
    private float xBoundryMax;

    void Start()
    {
        startingPosZ = transform.position.z;
        player = Player.Instance;

        yBoundryMin = WorldManager.tileLength * WorldManager.height * -1;
        yBoundryMax = WorldManager.tileLength * 30;
        xBoundryMin = 0;
        xBoundryMax = WorldManager.tileLength * WorldManager.width;

        Debug.Log(yBoundryMin);
        Debug.Log(yBoundryMax);
        Debug.Log(xBoundryMin);
        Debug.Log(xBoundryMax);
    }

    void LateUpdate()
    {
        FollowPlayer();
        ClampCameraPositionWithinBoundaries();
    }

    private void ClampCameraPositionWithinBoundaries()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, xBoundryMin, xBoundryMax),
            Mathf.Clamp(transform.position.y, yBoundryMin, yBoundryMax),
            startingPosZ
            );
    }

    private void FollowPlayer()
    {
        //transform.DOMove(new Vector3(player.transform.position.x, player.transform.position.y, startingPosZ), tweenUpdateFrequency);
        transform.position = Vector3.Lerp(transform.position, player.transform.position, tweenUpdateFrequency * Time.deltaTime);
    }
}
