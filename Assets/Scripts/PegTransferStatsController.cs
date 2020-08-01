using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PegTransferStatsController : MonoBehaviour
{
    public GameObject grabber;
    public GameObject hapticDevice;

    public Text chrono;
    public Text nextStep;

    private float timer;
    private float grabbingTimer;
    private int grabbingErrorCounter;
    private int grabberTouches;

    private int movements;

    private bool gameStarted;

    private string statsPath;

    private int step;

    private string lastStickTouched;

    private bool[] pegsInMovement;
    private bool[] pegsInFloor;

    private float[] pegsToRightTimers;
    private float[] pegsToLeftTimers;
    private float[] pegsErrorTimers;


    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;

        statsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\stats.csv";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timer += Time.deltaTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            chrono.text = minutes + ":" + seconds;

            if(grabber.GetComponent(typeof(FixedJoint)) as FixedJoint != null) {
                grabbingTimer += Time.deltaTime;
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    grabbingErrorCounter++;
                }
            }

            updateTimers();

            checkTouches();

            Debug.Log(grabberTouches);
        }
    }

    private void checkTouches()
    {
        if(hapticDevice.GetComponent<HapticPlugin>().touching != null)
        {
            string touchingObject = hapticDevice.GetComponent<HapticPlugin>().touching.name;

            if(touchingObject != lastStickTouched)
            {
                switch (touchingObject)
                {
                    case "Stick 1":
                    case "Stick 2":
                    case "Stick 3":
                    case "Stick 4":
                        grabberTouches++;
                        lastStickTouched = touchingObject;
                        break;
                    default:
                        lastStickTouched = "";
                        break;
                }
            }
        }
        else
        {
            lastStickTouched = "";
        }
    }

    private void updateTimers()
    {
        for(int i=0; i<4; i++)
        {
            if(pegsInMovement[i])
            {
                if(step == 1)
                {
                    pegsToRightTimers[i] += Time.deltaTime;
                }
                else
                {
                    pegsToLeftTimers[i] += Time.deltaTime;
                }
            }

            if(pegsInFloor[i])
            {
                Debug.Log("Peg in floor " + i);
                pegsErrorTimers[i] += Time.deltaTime;
            }
        }
    }

    public void notifyPegMovement(int peg)
    {
        if(!pegsInMovement[peg - 1])
        {
            pegsInMovement[peg - 1] = true;
            movements++;
        }
    }

    public void notifyPegRelease(int peg)
    {
        pegsInMovement[peg - 1] = false;
    }

    public void notifyPegError(int peg)
    {
        if(!pegsInMovement[peg - 1])
        {
            pegsInFloor[peg - 1] = true;
        }
        else
        {
            pegsInFloor[peg - 1] = false;
        }
    }

    public void notifyPegNotError(int peg)
    {
        pegsInFloor[peg - 1] = false;
    }

    public void pegsInRight()
    {
        nextStep.text = "Lleva de vuelta las fichas a los sticks de la izquierda";

        File.AppendAllText(statsPath, ";" + ((int)timer).ToString());

        step = 2;
    }

    public void pegTransferFinished()
    {
        nextStep.text = "Tarea finalizada";

        File.AppendAllText(statsPath, ";" + ((int)timer).ToString() + ";" + ((int)grabbingTimer).ToString());

        for(int i=0; i<4; i++)
        {
            File.AppendAllText(statsPath, ";" + ((int)pegsToRightTimers[i]).ToString() + ";" + ((int)pegsToLeftTimers[i]).ToString() + ";" + ((int)pegsErrorTimers[i]).ToString());
        }

        File.AppendAllText(statsPath, ";" + grabbingErrorCounter.ToString() + ";" + movements.ToString());
    }

    public void startGame()
    {
        nextStep.text = "Lleva las fichas a los sticks de la derecha";

        timer = 0.0f;
        grabbingTimer = 0.0f;
        grabbingErrorCounter = 0;
        movements = 0;
        grabberTouches++;

        step = 1;

        pegsInMovement = new bool[4];
        pegsInFloor = new bool[4];
        pegsToRightTimers = new float[4];
        pegsToLeftTimers = new float[4];
        pegsErrorTimers = new float[4];

        for(int i=0; i<pegsInMovement.Length; i++)
        {
            pegsInMovement[i] = false;
            pegsInFloor[i] = false;
            pegsToRightTimers[i] = 0.0f;
            pegsToLeftTimers[i] = 0.0f;
            pegsErrorTimers[i] = 0.0f;
        }

        gameStarted = true;
    }
}
