using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FromService
{
    public string command = "";
    public List<string> parameters = new List<string>();
    public int duration;
    public bool test;
}


public class ToService
{
    public int timeremaining = 0;
    public string nameOfApp = "appplicationname";
    public bool readyToStart = false;
    public int isPlaying = 0;
    public bool isFinished = false;
}


public class constant
{
    public static string CMD_LOAD_SESSION = "CMD_LOAD_SESSION";
    public static string CMD_STOP_SESSION = "CMD_STOP_SESSION";
    public static string CMD_START_SESSION = "CMD_START_SESSION";
    public static string CMD_CHECK_FILES = "CMD_CHECK_FILES";
}

public class PluginWrapper : MonoBehaviour
{


    private AndroidJavaObject javaClass;
    private float timer = 0;
    public UnityEvent OnReady;
    public UnityEvent OnStart;
    public UnityEvent OnStop;

    public List<string> parametersReceived = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        javaClass = new AndroidJavaObject("com.JukeboxVR.broadcastunity.BroadCastUnityClass");
        Debug.LogError("JAVACLASS = " + javaClass.ToString());
        javaClass.CallStatic("createInstance");

        //javaClass.Call("LogNativeLogcatMessage");
        //string externalConfigurationRaw = javaClass.GetStatic<string>("text");
    }

    // Update is called once per frame
    void Update()
    {
        /*timer += Time.deltaTime;
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
        */


    }

    public void Onmessagereceive(string message)
    {
        //string externalConfigurationRaw = javaClass.GetStatic<string>("text");
        //Debug.LogError("externalConfigurationRaw = " + externalConfigurationRaw);
        Debug.LogError("message received : " + message);
        FromService msg = JsonUtility.FromJson<FromService>(message);
        if (msg.command == constant.CMD_LOAD_SESSION)
        {
            parametersReceived = msg.parameters;
            OnReady.Invoke();
            sendReady();
        }
        else if (msg.command == constant.CMD_START_SESSION)
        {
            OnStart.Invoke();
        }
        else if (msg.command == constant.CMD_STOP_SESSION)
        {
            OnStop.Invoke();
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

    public void sendInfo(int timeremaining, int isPlaying, bool finished)
    {
        ToService msgToService = new ToService();
        msgToService.readyToStart = false;
        msgToService.timeremaining = timeremaining;
        msgToService.isPlaying = isPlaying;
        msgToService.isFinished = finished;
        Debug.LogError("timeremaining = " + msgToService.timeremaining);
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
