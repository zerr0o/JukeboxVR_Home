using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginWrapper : MonoBehaviour
{

    
    private AndroidJavaObject javaClass;
    private AndroidJavaObject javaReceiver;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        javaClass = new AndroidJavaObject("com.JukeboxVR.broadcastunity.BroadCastUnityClass");
        javaReceiver = new AndroidJavaObject("com.JukeboxVR.broadcastunity.BroadCastUnityClass.Receiver");
        javaClass.Call("LogNativeLogcatMessage");
        javaReceiver.Call("onReceive");
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer > 2)
        //{
        //    //javaClass.Call("LogNativeLogcatMessage");
        //    //javaClass.Call("LogNumberFromUnity", Time.timeSinceLevelLoad);
        //    //Debug.LogError(javaClass.Call<int>("ReturnnumberToUnity", Time.timeSinceLevelLoad));
        //    timer = 0;
        //}


    }



    public void LogaRandomNumber()
    {
        Debug.LogWarning(Mathf.Round(Random.value * 10000));
    }


    public void LogMessage()
    {
        Debug.LogWarning("Function unity OK !!");
    }
}
