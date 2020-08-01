using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        GameObject.Find("/Path").GetComponent<PathGameController>().startGame();
    }
}
