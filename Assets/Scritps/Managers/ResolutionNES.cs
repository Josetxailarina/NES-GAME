using UnityEngine;

public class ResolutionNES : MonoBehaviour
{
    void Start()
    {
      Screen.SetResolution(256,240,false);
        Application.targetFrameRate = 60;
    }
}
