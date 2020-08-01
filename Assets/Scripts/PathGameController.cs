using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PathGameController : MonoBehaviour
{
    public GameObject hapticDevice;
    public GameObject pickUp;
    public Text chrono;

    private float timer;

    public bool gameStarted;

    private int touchWallCounter;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;

        gameStarted = false;

        touchWallCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {        
        if(gameStarted)
        {
            timer += Time.deltaTime;
            chrono.text = ((int)timer).ToString("000");
        }
    }

    public void startGame()
    {
        this.gameStarted = true;
    }

    public void addWallError()
    {
        touchWallCounter++;
    }

    public void recordStats(int[] markerErrors, int[] markerTimers)
    {
        this.gameStarted = false;

        string statsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\stats.csv";

        File.AppendAllText(statsPath, ((int)timer).ToString("000") + ";" + touchWallCounter);

        for(int i=0; i<markerErrors.Length; i++)
        {
            File.AppendAllText(statsPath, ";" + markerErrors[i] + ";" + markerTimers[i]);
        }

        File.AppendAllText(statsPath, ";" + ((int)GameObject.Find("Avatar").GetComponent<AvatarColliderController>().distanceCounter));

        File.AppendAllText(statsPath, "\n");
    }
}
