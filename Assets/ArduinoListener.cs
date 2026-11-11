using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArduinoListener : MonoBehaviour {
    public playerStateManager playerStateManager;
    public KazooProcessor kazooBreathDetector;

    // Use this for initialization
    
    const string KAZOO_MESSAGE = "kazoo sensor, ";
    const string BELT_MESSAGE = "belt sensor, ";

    float kazooValue;
    float beltValue;
    void Start ()
    {
        if (playerStateManager != null) return;
        playerStateManager = GetComponent<playerStateManager>();
        if (playerStateManager == null)Debug.LogError("playerStateManager script not found in scene.");
    }
    // Update is called once per frame
    void Update () {
    }
    // Invoked when a line of data is received from the serial device.
    void OnMessageArrived(string msg)
    {
        //Debug.Log("Arrived: " + msg);

        if (msg.StartsWith(KAZOO_MESSAGE))
        {
            float.TryParse(msg.Substring(KAZOO_MESSAGE.Length), out kazooValue);
            playerStateManager.kazooValue = kazooValue;
            if (kazooBreathDetector != null)
            {
                kazooBreathDetector.ProcessTemperature(kazooValue, Time.deltaTime);
            }
        }
        else if (msg.StartsWith(BELT_MESSAGE))
        {
            float.TryParse(msg.Substring(BELT_MESSAGE.Length), out beltValue);
            playerStateManager.beltValue = beltValue;
        }
    }
    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        //Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}
