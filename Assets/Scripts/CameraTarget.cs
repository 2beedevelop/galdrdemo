using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float smoothSpeed = 5f;

    private void Update()
    {
        if (player1 == null || player2 == null) return;
        Vector3 midpoint = (player1.position + player2.position) / 2f;
        transform.position = Vector3.Lerp(transform.position, midpoint, smoothSpeed * Time.deltaTime);
    }
}