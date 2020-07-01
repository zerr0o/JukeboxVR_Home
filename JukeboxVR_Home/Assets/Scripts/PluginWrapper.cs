using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Message FROM service
/// </summary>
public class FromService
{
    public string command = "";
    public List<string> parameters = new List<string>();
    public int duration;
    public bool test;
}

/// <summary>
/// Message FOR service
/// </summary>
public class ToService
{
    public int timeremaining = 0;
    public string nameOfApp = "appplicationname";
    public bool readyToStart = false;
    public int isPlaying = 0;
    public bool isFinished = false;
}

/// <summary>
/// Define constants commands
/// </summary>
public class constant
{
    public static string CMD_LOAD_SESSION = "CMD_LOAD_SESSION";
    public static string CMD_STOP_SESSION = "CMD_STOP_SESSION";
    public static string CMD_START_SESSION = "CMD_START_SESSION";
    public static string CMD_CHECK_FILES = "CMD_CHECK_FILES";
    public static string CMD_COMMAND_SESSION = "CMD_COMMAND_SESSION";
}

/// <summary>
/// UnityEvent with parameters
/// </summary>
[System.Serializable]
public class OnReady : UnityEvent<List<string>>
{
}

[System.Serializable]
public class OnCommand : UnityEvent<List<string>>
{
}

public class PluginWrapper : MonoBehaviour
{

    private AndroidJavaObject javaClass;
    public OnReady onReady;
    public OnCommand onCommand;
    public UnityEvent onStart;
    public UnityEvent onStop;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        DontDestroyOnLoad(this);
        javaClass = new AndroidJavaObject("com.JukeboxVR.broadcastunity.BroadCastUnityClass");
        Debug.LogError("JAVACLASS = " + javaClass.ToString());
        javaClass.CallStatic("createInstance");
#endif
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Called when a message is received from service
    /// </summary>
    /// <param name="message"></param>
    public void Onmessagereceive(string message)
    {
        Debug.LogError("message received : " + message);
        FromService msg = JsonUtility.FromJson<FromService>(message);
        if (msg.command == constant.CMD_LOAD_SESSION)
        {
            sendReady();
            onReady.Invoke(msg.parameters);
        }
        else if (msg.command == constant.CMD_START_SESSION)
        {
            onStart.Invoke();
        }
        else if (msg.command == constant.CMD_STOP_SESSION)
        {
            onStop.Invoke();
        }
        else if ( msg.command == constant.CMD_COMMAND_SESSION )
        {
            onCommand.Invoke(msg.parameters);
        }


    }

    /// <summary>
    /// Return ready message to service 
    /// </summary>
    public void sendReady()
    {
        ToService msgToService = new ToService();
        Debug.LogError("sendmessage READY from app");
        msgToService.readyToStart = true;
        SendMessageToService(msgToService);
    }

    /// <summary>
    /// Mettre à jour les infos sur la régie
    /// </summary>
    /// <param name="timeremaining">Temps à afficher sur la régie en seconde </param>
    /// <param name="isPlaying">1 si en cours de séance , 0 si on ne sait pas , -1 si en attente </param>
    /// <param name="finished">true si la séance est terminée , false sinon </param>
    public void sendInfo(int timeremaining = 0 , int isPlaying = 0 , bool finished = false)
    {
        
        ToService msgToService = new ToService();
        msgToService.readyToStart = false;
        msgToService.timeremaining = timeremaining;
        msgToService.isPlaying = isPlaying;
        msgToService.isFinished = finished;
        
        SendMessageToService(msgToService);
        
    }

    /// <summary>
    /// Send a message to service
    /// </summary>
    /// <param name="msgToService">message to send</param>
    public void SendMessageToService(ToService msgToService)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //Debug.Log("sendmessage back");
        msgToService.nameOfApp = Application.productName.ToString();
        string jsonMsg = JsonUtility.ToJson(msgToService);
        AndroidJavaClass androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        javaClass.Call("sendIntent", new object[] { jsonMsg.ToString(), activity });
#endif
    }


    /// <summary>
    /// Log a random number using unity log // TEST ONLY //
    /// </summary>
    public void LogaRandomNumber()
    {
        Debug.LogWarning(Mathf.Round(Random.value * 10000));
    }


    /// <summary>
    /// Log a String using unity log //TEST ONLY //
    /// </summary>
    public void LogMessage()
    {
        Debug.LogWarning("Function unity OK !!");
    }



}
