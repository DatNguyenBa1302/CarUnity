using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CarController : MonoBehaviour
{
    private Queue<Car> movementQueue = new Queue<Car>();
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    { }

    private void Update()
    {
    }

    public void MoveCar(string command)
    {
       
    }
}
