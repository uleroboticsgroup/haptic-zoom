using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class CubesGameController : MonoBehaviour
{
    public Text chrono;
    public Text userName;
    public Text methodText;
    public Text action;

    public GameObject hapticWorkspace;

    private float timer;
    private List<GameObject> bases;

    private int method;

    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        userName.text = "User name";
        gameStarted = false;

        methodText.text = "Escalado";
        action.text = "Presiona S para comenzar la tarea";
        method = 0;

        bases = new List<GameObject>();

        for(int i=0; i<this.gameObject.transform.childCount; i++)
        {
            bases.Add(this.gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetKeyUp(KeyCode.S))
        {
            timer = 0.0f;
            action.text = "Presiona E para pasar a la siguiente tarea";

            gameStarted = true;
        }

        if (gameStarted && Input.GetKeyUp(KeyCode.E))
        {
            saveStats();

            resetScene();

            method++;

            if (method == 1)
            {
                Camera.main.GetComponent<ClutchingController>().enabled = true;
                Camera.main.GetComponent<HapticZoomController>().enabled = false;

                methodText.text = "Clutching";

                hapticWorkspace.transform.localPosition = new Vector3(0.0f, 0.0f, -0.5f);
                hapticWorkspace.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
            else if (method == 2)
            {
                Camera.main.GetComponent<ClutchingController>().enabled = false;
                Camera.main.GetComponent<HapticZoomController>().enabled = true;

                methodText.text = "Zoom háptico";
            }
            else
            {
                GameObject.Find("End").GetComponent<Text>().enabled = true;
            }
        }

        if (gameStarted)
        {
            timer += Time.deltaTime;

            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = (timer % 60).ToString("00");

            chrono.text = minutes + ":" + seconds;
        }
    }

    private void saveStats()
    {
        string statsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\stats.csv";

        int seconds = (int)(timer % 60);

        float xDiff;
        float zDiff;
        float error = 0.0f;

        foreach (GameObject item in bases)
        {
            GameObject top = item.transform.GetChild(0).gameObject;
            xDiff = Mathf.Abs(top.transform.localPosition.x);
            zDiff = Mathf.Abs(top.transform.localPosition.z);
            error += Mathf.Sqrt(Mathf.Pow(xDiff, 2) + Mathf.Pow(zDiff, 2));
        }

        File.AppendAllText(statsPath, ";" + seconds + ";" + error);
    }

    private void resetScene()
    {
        foreach (GameObject item in bases)
        {
            GameObject top = item.transform.GetChild(0).gameObject;
            top.transform.localPosition = new Vector3(0.0f, 0.0f, -1.5f);
        }

        Camera.main.transform.position = new Vector3(0, 5.28f, 0);

        hapticWorkspace.transform.localPosition = new Vector3(0, 0, 0);
        hapticWorkspace.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);

        chrono.text = "Cronómetro";
        action.text = "Presiona S para comenzar la tarea";

        gameStarted = false;
    }
}
