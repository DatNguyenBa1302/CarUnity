using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBoundaryCheck : MonoBehaviour
{
    public static bool IsBoundary = false;
    public bool isBoundaryCheckEnabled = false;
    public Transform car;
    public Transform resetPosition;
    public Vector2 trackBounds;

    private void Start()
    {
        isBoundaryCheckEnabled=true;
    }
    private void Update()
    {
        IsBoundaryOn();
    }

    public void IsBoundaryOn()
    {
        if (isBoundaryCheckEnabled)
        {
            if (Mathf.Abs(car.position.x) > trackBounds.x || Mathf.Abs(car.position.z) > trackBounds.y)
            {
                // Reset vị trí và góc quay của xe
                car.position = resetPosition.position;
                car.rotation = resetPosition.rotation;
                Debug.Log("Xe đã rời khỏi đường đua! Reset lại vị trí.");
                IsBoundary = true;
            }
            else IsBoundary = false;
        }
    }
}
