using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerController : MonoBehaviour
{
    public GameObject avatar;
    public GameObject pickUp;

    public Text targetTimer;

    private float timer;
    private float markerTimer;

    private bool inside;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        markerTimer = 0.0f;

        inside = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!this.gameObject.GetComponent<Renderer>().enabled)
        {
            return;
        }

        if(GameObject.Find("HapticDevice").GetComponent<HapticPlugin>().Buttons[0] == 1
            || GameObject.Find("HapticDevice").GetComponent<HapticPlugin>().Buttons[1] == 1)
        {
            timer = 0.0f;
            targetTimer.text = "";
            return;
        }

        markerTimer += Time.deltaTime;

        double distanceToAvatar = Mathf.Sqrt(Mathf.Pow(avatar.transform.position.x - this.gameObject.transform.position.x, 2) + 
            Mathf.Pow(avatar.transform.position.z - this.gameObject.transform.position.z, 2));

        if (distanceToAvatar < 0.005)
        {
            timer += Time.deltaTime;
            inside = true;
        }
        else
        {
            timer = 0.0f;

            if(inside)
            {
                inside = false;
                this.pickUp.GetComponent<PickUpController>().addMarkerError();
            }
        }

        if(timer > 3.0f)
        {
            this.pickUp.GetComponent<PickUpController>().addMarkerTimer((int)markerTimer);
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f);
            targetTimer.text = "";
            this.pickUp.GetComponent<PickUpController>().showAgain();
            timer = 0.0f;
            markerTimer = 0.0f;
        }

        if(timer > 0.0f)
        {
            targetTimer.transform.localScale = Vector3.Lerp(targetTimer.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), 1.5f * Time.deltaTime);

            if (targetTimer.text != ((int)timer).ToString())
            {
                targetTimer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            targetTimer.text = ((int)timer).ToString();
        }
        else
        {
            targetTimer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            targetTimer.text = "";
        }
    }

    public void show()
    {
        this.gameObject.GetComponent<Renderer>().enabled = true;
        this.gameObject.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
    }
}
