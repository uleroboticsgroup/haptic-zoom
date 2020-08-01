using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public int pickUpCounter;

    public Text go;

    public GameObject marker;

    private Vector3[] spots;
    private Vector3[] markerSpots;

    private int[] markerErrors;
    private int[] markerTimers;

    private HapticEffect.EFFECT_TYPE[] effects;
    private float[] gains;
    private float[] magnitudes;

    private float goTimer;

    // Start is called before the first frame update
    void Start()
    {
        pickUpCounter = 0;

        spots = new Vector3[4];
        spots[0] = new Vector3(0.6f, 0.05f, 1.575f);
        spots[1] = new Vector3(-1.7f, 0.05f, -0.375f);
        spots[2] = new Vector3(0.0f, 0.05f, -0.375f);
        spots[3] = new Vector3(1.0f, 0.05f, 0.275f);

        markerSpots = new Vector3[4];
        markerSpots[0] = new Vector3(0.15f, -1.05f, 0.0f);
        markerSpots[1] = new Vector3(0.15f, -1.05f, 0.0f);
        markerSpots[2] = new Vector3(-0.15f, -1.05f, 0.0f);
        markerSpots[3] = new Vector3(-0.15f, -1.05f, 0.0f);

        markerErrors = new int[5];
        markerErrors[0] = 0;
        markerErrors[1] = 0;
        markerErrors[2] = 0;
        markerErrors[3] = 0;
        markerErrors[4] = 0;

        markerTimers = new int[5];

        effects = new HapticEffect.EFFECT_TYPE[4];

        gains = new float[4];

        magnitudes = new float[4];

        randomEffects();

        goTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        if(go.text != "")
        {
            goTimer += Time.deltaTime;

            if(goTimer > 1.0f)
            {
                go.text = "";
                goTimer = 0.0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Avatar"))
        {
            this.gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f);
            this.marker.gameObject.GetComponent<MarkerController>().show();
        }
    }

    public void addMarkerError()
    {
        this.markerErrors[pickUpCounter]++;
    }

    public void showAgain()
    {
        if(pickUpCounter > 3)
        {
            endGame();

            return;
        }

        go.text = "Go!";

        Vector3 newPosition = spots[pickUpCounter];

        this.gameObject.transform.position = newPosition;
        this.marker.transform.position = newPosition + markerSpots[pickUpCounter];

        this.marker.GetComponentInChildren<HapticEffect>().effectType = this.effects[pickUpCounter];
        this.marker.GetComponentInChildren<HapticEffect>().Gain = this.gains[pickUpCounter];
        this.marker.GetComponentInChildren<HapticEffect>().Magnitude = this.magnitudes[pickUpCounter];

        this.gameObject.GetComponent<Renderer>().enabled = true;

        pickUpCounter++;
    }

    private void endGame()
    {
        this.gameObject.transform.position -= new Vector3(0.0f, 1.0f, 0.0f);

        GameObject.Find("Path").GetComponent<PathGameController>().recordStats(markerErrors, markerTimers);
    }

    public void addMarkerTimer(int timer)
    {
        markerTimers[pickUpCounter] = timer;
    }

    private void randomEffects()
    {
        int random = Random.Range(1, 3);

        switch(random)
        {
            case 1:
                effects[0] = HapticEffect.EFFECT_TYPE.VIBRATE;
                effects[1] = HapticEffect.EFFECT_TYPE.VISCOUS;
                effects[2] = HapticEffect.EFFECT_TYPE.VIBRATE;
                effects[3] = HapticEffect.EFFECT_TYPE.FRICTION;
                gains[0] = 0.75f;
                gains[1] = 1.0f;
                gains[2] = 0.0f;
                gains[3] = 0.5f;
                magnitudes[0] = 0.5f;
                magnitudes[1] = 1.0f;
                magnitudes[2] = 0.0f;
                magnitudes[3] = 0.3f;
                break;
            case 2:
                effects[0] = HapticEffect.EFFECT_TYPE.VISCOUS;
                effects[1] = HapticEffect.EFFECT_TYPE.VIBRATE;
                effects[2] = HapticEffect.EFFECT_TYPE.VIBRATE;
                effects[3] = HapticEffect.EFFECT_TYPE.FRICTION;
                gains[0] = 1.0f;
                gains[1] = 0.75f;
                gains[2] = 0.0f;
                gains[3] = 0.5f;
                magnitudes[0] = 1.0f;
                magnitudes[1] = 0.5f;
                magnitudes[2] = 0.0f;
                magnitudes[3] = 0.3f;
                break;
            case 3:
                effects[0] = HapticEffect.EFFECT_TYPE.VIBRATE;
                effects[1] = HapticEffect.EFFECT_TYPE.FRICTION;
                effects[2] = HapticEffect.EFFECT_TYPE.VISCOUS;
                effects[3] = HapticEffect.EFFECT_TYPE.VIBRATE;
                gains[0] = 0.0f;
                gains[1] = 0.5f;
                gains[2] = 1.0f;
                gains[3] = 0.75f;
                magnitudes[0] = 0.0f;
                magnitudes[1] = 0.3f;
                magnitudes[2] = 1.0f;
                magnitudes[3] = 0.5f;
                break;
        }
    }
}
