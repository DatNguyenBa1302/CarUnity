using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform carTransForm;
    public Transform cameraPointTransForm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(carTransForm);
        transform.position = cameraPointTransForm.position;
    }
}
