using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticZoomController : MonoBehaviour
{
    public Camera camera;
    public GameObject hapticDevice;
    public GameObject avatar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hapticDevice.GetComponent<HapticPlugin>().Buttons[0] == 1 && camera.transform.position.y > 0.4f)
        {
            Vector3 A = hapticDevice.transform.localPosition;

            Vector3 B = avatar.transform.localPosition;

            Vector3 C = A - B; // diff from object pivot to desired pivot/origin

            Vector3 newScale = new Vector3(hapticDevice.transform.localScale.x - 0.0001f, hapticDevice.transform.localScale.y - 0.0001f, hapticDevice.transform.localScale.z - 0.0001f);

            float RS = newScale.x / hapticDevice.transform.localScale.x; // relataive scale factor

            // calc final position post-scale
            Vector3 FP = B + C * RS;

            // finally, actually perform the scale/translation
            hapticDevice.transform.localScale = newScale;
            hapticDevice.transform.localPosition = FP;
        }

        if (hapticDevice.GetComponent<HapticPlugin>().Buttons[1] == 1)
        {
            Vector3 A = hapticDevice.transform.localPosition;

            Vector3 B = avatar.transform.localPosition;

            Vector3 C = A - B; // diff from object pivot to desired pivot/origin

            Vector3 newScale = new Vector3(hapticDevice.transform.localScale.x + 0.0001f, hapticDevice.transform.localScale.y + 0.0001f, hapticDevice.transform.localScale.z + 0.0001f);

            float RS = newScale.x / hapticDevice.transform.localScale.x; // relataive scale factor

            // calc final position post-scale
            Vector3 FP = B + C * RS;

            // finally, actually perform the scale/translation
            hapticDevice.transform.localScale = newScale;
            hapticDevice.transform.localPosition = FP;
        }
    }

    private void LateUpdate()
    {
        float verticalFOVInRads = camera.fieldOfView * Mathf.Deg2Rad;
        float horizontalFOVInRads = 2 * Mathf.Atan(Mathf.Tan(verticalFOVInRads / 2) * camera.aspect);
        float horizontalFOVInDegrees = horizontalFOVInRads * Mathf.Rad2Deg;

        float anchuraWorkspace = (hapticDevice.transform.localScale.x * 5.5f) / 0.025f;

        float alturaCamara = anchuraWorkspace / Mathf.Tan(horizontalFOVInRads / 2.0f);

        float centradoVerticalCamara = (hapticDevice.transform.localScale.z * 1.25f) / 0.025f;

        camera.transform.position = new Vector3(hapticDevice.transform.position.x, alturaCamara, hapticDevice.transform.position.z + centradoVerticalCamara);
    }
}
