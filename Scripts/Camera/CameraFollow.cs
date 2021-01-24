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
    private float halfScreenWidthInWorldSpace;

    void Start()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Camera.main.aspect;
        halfScreenWidthInWorldSpace = width / 2;

        startingPosZ = transform.position.z;
        player = Player.Instance;

        yBoundryMin = WorldManager.tileLength * WorldManager.height * -1;
        yBoundryMax = WorldManager.tileLength * 20;
        xBoundryMin = halfScreenWidthInWorldSpace;
        xBoundryMax = WorldManager.tileLength * WorldManager.width - halfScreenWidthInWorldSpace - 5;
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void LateUpdate()
    {
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
