using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzadorFuego : MonoBehaviour
{
    public FireBall[] bolasScript;



    public void LanzarFuego(Vector3 posicionFuego, Vector2 direccion)
    {
        foreach (FireBall script in bolasScript)
        {
            if (!script.isLaunched)
            {
                script.LanzarFuego(posicionFuego, direccion);
                break;
            }
        }

    }
}
