using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public static CameraControler instance;
    public Transform target;

    public Vector3 decalLook;
    public Vector3 decalPos;

    public float posLerpSpeed = 0.02f;
    public float lookLerpSpeed = 0.1f;

    public Vector3 wantedPos;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            return;

        wantedPos = target.TransformPoint(decalPos);
        wantedPos.y = decalPos.y;
        transform.position = Vector3.Lerp(transform.position, wantedPos, posLerpSpeed);

        Quaternion wantedLook = Quaternion.LookRotation(target.TransformPoint(decalLook) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, wantedLook, lookLerpSpeed);
    }
}
