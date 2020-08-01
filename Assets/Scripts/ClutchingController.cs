using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClutchingController : MonoBehaviour
{
    public GameObject hapticDevice;
    public GameObject avatar;

    private Vector3 oldStylusPosition;
    private Vector3 workspaceDispl;

    private bool clutchingStarted;

    // Start is called before the first frame update
    void Start()
    {
        clutchingStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        /* Cuando presiono el botón */
        if (hapticDevice.GetComponent<HapticPlugin>().Buttons[0] == 1)
        {
            /* Si es el primer update desde que se pulsa (no hay una maniobra de clutching en marcha) */
            if(!clutchingStarted)
            {
                /* Guardo la posicion del stylus cuando se empieza el clutching */
                oldStylusPosition = hapticDevice.GetComponent<HapticPlugin>().proxyPositionRaw;

                /* Guardo lo que se tiene que desplazar el workspace para centrarse en la posicion del avatar */
                workspaceDispl = avatar.transform.position - hapticDevice.transform.position;

                /* Desplazo el workspace para que se centre en el avatar */
                hapticDevice.transform.position += workspaceDispl;

                /* Escalo a 0 el workspace (para que aunque se mueva el stylus no se 'toque' de forma fantasma ningun objeto) */
                hapticDevice.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                /* Se indica que se ha iniciado la maniobra de clutching */
                clutchingStarted = true;
            }
        }
        else
        {
            /* Si el botón no está presionado pero es el primer update después de que estuviera presionado */
            if (clutchingStarted)
            {
                /* Guardo la posición del stylus */
                Vector3 stylusPositionWorld = hapticDevice.GetComponent<HapticPlugin>().proxyPositionRaw;

                /* Calculo la diferencia entre la posicion actual del stylus y la guardada al comienzo de la maniobra de clutching */
                Vector3 diff = (stylusPositionWorld - oldStylusPosition) / 100.0f;

                /* Reestablezco el escalado del workspace al original */
                hapticDevice.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                /* Devuelvo el workspace a su posicion original antes de iniciar la maniobra de clutching */
                hapticDevice.transform.position -= workspaceDispl;

                /* Muevo el workspace de tal manera que el stylus mantenga su posicion en el mundo antes y despues de la maniobra de clutching */
                /* Los ejes Z e Y no coinciden, por eso están cambiados, y la dirección del Y tampoco, por eso está multiplicada por -1 */
                hapticDevice.transform.position -= new Vector3(diff.x, (-1)*diff.z, diff.y);

                /* Se indica que ha finalizado la maniobra de clutching */
                clutchingStarted = false;
            }
        }
    }
}
