using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestController : MonoBehaviour
{
    private int mode;

    public Camera camera;

    public GameObject hapticWorkspace;

    public Text currentMode;
    public Text nextMode;

    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            resetScene();

            mode = (mode + 1) % 3;

            switch (mode)
            {
                case 0:
                    camera.GetComponent<ClutchingController>().enabled = false;
                    camera.GetComponent<HapticZoomController>().enabled = false;

                    currentMode.text = "Escalado";
                    nextMode.text = "Presiona C para cambiar a clutching";

                    break;
                case 1:
                    camera.GetComponent<ClutchingController>().enabled = true;
                    camera.GetComponent<HapticZoomController>().enabled = false;

                    currentMode.text = "Clutching";
                    nextMode.text = "Presiona C para cambiar a Zoom háptico";

                    hapticWorkspace.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                    break;
                case 2:
                    camera.GetComponent<ClutchingController>().enabled = false;
                    camera.GetComponent<HapticZoomController>().enabled = true;

                    currentMode.text = "Zoom háptico";
                    nextMode.text = "Presiona C para cambiar a escalado";

                    break;
                default:
                    camera.GetComponent<ClutchingController>().enabled = false;
                    camera.GetComponent<HapticZoomController>().enabled = false;

                    currentMode.text = "Escalado";
                    nextMode.text = "Presiona C para cambiar a clutching";

                    break;
            }
        }
    }

    private void resetScene()
    {
        camera.gameObject.transform.position = new Vector3(0, 5.28f, 0);

        hapticWorkspace.transform.localPosition = new Vector3(0, 0, 0);
        hapticWorkspace.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
    }
}
