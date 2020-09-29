using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class AutoLoadImg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        DirectoryInfo info = new DirectoryInfo(GetLogoPath());
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            Debug.Log("Using " + file + " as logo");
            if( file.Extension == ".png" || file.Extension == ".PNG")
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                Texture2D texture = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);
                texture.LoadImage(data);
                texture.name = Path.GetFileNameWithoutExtension(file.FullName);
                this.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);
            }
            

        }


    }


    /// <summary>
    /// Get the download folder on android or windows
    /// </summary>
    /// <returns>return download folder</returns>
    public static string GetLogoPath() // get the downlod folder on windows or android
    {
        string[] temp = (Application.persistentDataPath.Replace("Android", "")).Split(new string[] { "//" }, System.StringSplitOptions.None);
        return (temp[0] + "/Download/logo");
    }

}
