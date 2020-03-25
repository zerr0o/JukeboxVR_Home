using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head_Follower : MonoBehaviour
{


    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        this.transform.localRotation = Quaternion.Lerp( this.transform.localRotation , head.localRotation , Time.deltaTime*3.0f) ;

    }



}
