using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzadorFuego : MonoBehaviour
{
    public BolaFuego[] bolasScript;



    public void LanzarFuego(Vector3 posicionFuego, Vector2 direccion)
    {
        foreach (BolaFuego script in bolasScript)
        {
            if (!script.lanzado)
            {
                script.LanzarFuego(posicionFuego, direccion);
                break;
            }
        }

    }
}
