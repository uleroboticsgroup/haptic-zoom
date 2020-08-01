using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PegTransferController : MonoBehaviour
{
    public Text countdown;

    public Button goHome;

    public GameObject grabber;

    private bool gameStarted;

    private bool countdownAnimation;

    private float countdownTimer;

    private int step;

    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        countdownTimer = 5;
        countdown.text = "5";

        countdownAnimation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted)
        {
            updateCountdownTimer();

            return;
        }

        checkPegsOutOfSticks();

        if (grabber.GetComponent(typeof(FixedJoint)) as FixedJoint != null)
        {
            startPegMovement();
        }
        else
        {
            stopPegMovement();
        }

        if (step == 1 && checkPegsInRight())
        {
            step = 2;

            GetComponent<PegTransferStatsController>().pegsInRight();
        }

        if(step == 2 && checkPegsInLeft())
        {
            step = 0;

            GetComponent<PegTransferStatsController>().pegTransferFinished();

            goHome.gameObject.SetActive(true);
        }
    }

    private void startPegMovement()
    {
        FixedJoint attached = grabber.GetComponent(typeof(FixedJoint)) as FixedJoint;

        switch(attached.connectedBody.name)
        {
            case "Peg 1":
                GetComponent<PegTransferStatsController>().notifyPegMovement(1);
                break;
            case "Peg 2":
                GetComponent<PegTransferStatsController>().notifyPegMovement(2);
                break;
            case "Peg 3":
                GetComponent<PegTransferStatsController>().notifyPegMovement(3);
                break;
            case "Peg 4":
                GetComponent<PegTransferStatsController>().notifyPegMovement(4);
                break;
        }
    }

    private void stopPegMovement()
    {
        for(int i=1; i<=4; i++)
        {
            GetComponent<PegTransferStatsController>().notifyPegRelease(i);
        }
    }

    private void checkPegsOutOfSticks()
    {
        GameObject startSticks = GameObject.Find("Start Sticks");
        GameObject endSticks = GameObject.Find("End Sticks");
        GameObject pegBoard = GameObject.Find("Pegs");

        GameObject[] pegs = new GameObject[pegBoard.transform.childCount];

        GameObject[] sticks = new GameObject[startSticks.transform.childCount + endSticks.transform.childCount];

        for(int i=0; i<pegs.Length; i++)
        {
            pegs[i] = pegBoard.transform.GetChild(i).gameObject;
            sticks[i] = startSticks.transform.GetChild(i).gameObject;
            sticks[i + 4] = endSticks.transform.GetChild(i).gameObject;
        }

        for(int i=0; i<pegs.Length; i++)
        {
            float closestDistance = float.MaxValue;

            for(int j=0; j<sticks.Length; j++)
            {
                float distance = Vector3.Distance(new Vector3(sticks[j].transform.position.x, 0.16f, sticks[j].transform.position.z), pegs[i].transform.position);

                if(distance < closestDistance)
                {
                    closestDistance = distance;
                }
            }

            if(closestDistance >= 0.1)
            {
                GetComponent<PegTransferStatsController>().notifyPegError(i + 1);
            }
            else
            {
                GetComponent<PegTransferStatsController>().notifyPegNotError(i + 1);
            }
        }
    }

    private bool checkPegsInRight()
    {
        bool result = true;

        GameObject endSticks = GameObject.Find("End Sticks");
        GameObject pegBoard = GameObject.Find("Pegs");

        GameObject[] pegs = new GameObject[pegBoard.transform.childCount];

        for (int i = 0; i < pegs.Length; i++)
            pegs[i] = pegBoard.transform.GetChild(i).gameObject;

        foreach(Transform stick in endSticks.transform)
        {
            float closestDistance = float.MaxValue;
            GameObject closestPeg;

            for(int i=0; i<pegs.Length; i++)
            {
                float distance = Vector3.Distance(new Vector3(stick.transform.position.x, 0.16f, stick.transform.position.z), pegs[i].transform.position);

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPeg = pegs[i];
                }
            }

            if (closestDistance > 0.1)
                result = false;
        }

        return result;
    }

    private bool checkPegsInLeft()
    {
        bool result = true;

        GameObject startSticks = GameObject.Find("Start Sticks");
        GameObject pegBoard = GameObject.Find("Pegs");

        GameObject[] pegs = new GameObject[pegBoard.transform.childCount];

        for (int i = 0; i < pegs.Length; i++)
            pegs[i] = pegBoard.transform.GetChild(i).gameObject;

        foreach (Transform stick in startSticks.transform)
        {
            float closestDistance = float.MaxValue;
            GameObject closestPeg;

            for (int i = 0; i < pegs.Length; i++)
            {
                float distance = Vector3.Distance(new Vector3(stick.transform.position.x, 0.16f, stick.transform.position.z), pegs[i].transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPeg = pegs[i];
                }
            }

            if (closestDistance > 0.1)
                result = false;
        }

        return result;
    }

    private void updateCountdownTimer()
    {
        countdownTimer -= Time.deltaTime;

        countdown.transform.localScale = Vector3.Lerp(countdown.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), 2.0f * Time.deltaTime);

        if (countdown.text != (countdownTimer).ToString("0"))
        {
            countdown.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (countdownTimer < 1)
        {
            startGame();
            gameStarted = true;
        }
        else
        {
            countdown.text = (countdownTimer).ToString("0");
        }
    }



    private void startGame()
    {
        step = 1;

        countdown.text = "";

        GetComponent<PegTransferStatsController>().startGame();
    }

    public void onGoHomeButtonClick()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
