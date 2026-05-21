using Fusion;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FollowPlayer : NetworkBehaviour
{
    [UnityEngine.Range(.1f,1f)]
    public float followDamping;
    public Transform playerTransform;
    public Camera mainCam;

    private void Start()
    {

        if (!this.gameObject.GetComponent<NetworkObject>().HasStateAuthority)
        {
            this.gameObject.SetActive(false);
        }

    }
    private void FixedUpdate()
    {
       
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, 1/followDamping * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, 1/followDamping * Time.fixedDeltaTime);
    }

}
