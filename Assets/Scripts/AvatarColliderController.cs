using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarColliderController : MonoBehaviour
{
    public GameObject pathGame;

    public float distanceCounter;
    private Vector3 lastPosition;

    void Start()
    {
        distanceCounter = 0.0f;
    }

    void Update()
    {
        if(pathGame.GetComponent<PathGameController>().gameStarted)
        {
            distanceCounter += Vector3.Distance(lastPosition, this.gameObject.transform.position);
            lastPosition = this.gameObject.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.parent.name == pathGame.transform.name)
        {
            pathGame.GetComponent<PathGameController>().addWallError();
        }
    }
}
