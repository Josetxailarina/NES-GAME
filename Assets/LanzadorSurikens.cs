using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzadorSurikens : MonoBehaviour
{
    public Suriken[] surikenScript;



    public void LanzarSuriken(Vector3 posicionSuriken,bool derecha)
    {
        foreach (Suriken script in surikenScript)
        {
            if (!script.lanzado)
            {
                script.LanzarSuriken(posicionSuriken,derecha);
                break;
            }
        }

    }
}
