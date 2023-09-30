using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileStartStop : MonoBehaviour
{

    public void StartReading()
    {
        SN.@this.FILEReader.StartReading();
    }
    public void StopReading()
    {
        SN.@this.FILEReader.StopReading();
    }

}
