using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private void Awake()
    {
        if (FindObjectsOfType<PlayerController>().Length < 2)
        {
            Instantiate(player1Prefab, new Vector3(-2, 0, 0), Quaternion.identity);
            Instantiate(player2Prefab, new Vector3(2, 0, 0), Quaternion.identity);
        }
    }
}
