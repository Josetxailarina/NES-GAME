using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionNES : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      Screen.SetResolution(256,240,false);
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
