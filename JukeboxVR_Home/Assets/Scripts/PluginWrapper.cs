using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FromService
{
    public string command = "";
    public int duration;
    public bool test;
    

}


public class ToService
{
    public int timeremaining = 0;
    public string nameOfApp = "appplicationname";
    public bool readyToStart = false;
}

public class PluginWrapper : MonoBehaviour
{


    private AndroidJavaObject javaClass;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        javaClass = new AndroidJavaObject("com.JukeboxVR.broadcastunity.BroadCastUnityClass");
        Debug.LogError("JAVACLASS = " + javaClass.ToString());
        javaClass.CallStatic("createInstance");

        //javaClass.Call("LogNativeLogcatMessage");
        //string externalConfigurationRaw = javaClass.GetStatic<string>("text");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {

            if (javaClass != null)
            {

                javaClass.Call("LogNativeLogcatMessage");

            }
            //javaClass.Call("LogNativeLogcatMessage");
            //javaClass.Call("LogNumberFromUnity", Time.timeSinceLevelLoad);
            //Debug.LogError(javaClass.Call<int>("ReturnnumberToUnity", Time.timeSinceLevelLoad));
            timer = 0;
        }


    }

    public void Onmessagereceive(string message)
    {
        //string externalConfigurationRaw = javaClass.GetStatic<string>("text");
        //Debug.LogError("externalConfigurationRaw = " + externalConfigurationRaw);
        Debug.LogError("message received : " + message);
        FromService msg = JsonUtility.FromJson<FromService>(message);
        
        if ( msg.command == "CMD_LOAD_SESSION" )
        {
            sendReady();
        }

        
        //foreach ( string command in myObject.commands )
        //{
        //    Debug.LogError(command);
        //}

        //Debug.LogError(myObject.duration);
        //Debug.LogError(myObject.test);

    }

    public void sendReady()
    {
        ToService msgToService = new ToService();
        msgToService.readyToStart = true;
        SendMessageToService(msgToService);
    }


    public void SendMessageToService(ToService msgToService)
    {
        Debug.LogError("sendmessage back");
        msgToService.nameOfApp = Application.productName.ToString();
        string jsonMsg = JsonUtility.ToJson(msgToService);
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        javaClass.Call("sendIntent", new object[] { jsonMsg.ToString(), activity });
        
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
