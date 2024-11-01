using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCommand
{
    public float FrontLeftSpeed { get; private set; }
    public float FrontRightSpeed { get; private set; }
    public float RearLeftSpeed { get; private set; }
    public float RearRightSpeed { get; private set; }
    public float Duration { get; private set; }

    public MovementCommand(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed, float duration)
    {
        FrontLeftSpeed = frontLeftSpeed;
        FrontRightSpeed = frontRightSpeed;
        RearLeftSpeed = rearLeftSpeed;
        RearRightSpeed = rearRightSpeed;
        Duration = duration;
    }
}
public class CarController : MonoBehaviour
{
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontRightWheelTransForm;
    public Transform frontLeftWheelTransForm;
    public Transform rearRightWheelTransForm;
    public Transform rearLeftWheelTransForm;

    private Queue<MovementCommand> movementQueue = new Queue<MovementCommand>();
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
       
        SetSpeed(100f, 100f, 100f, 100f);
        StartCoroutine(WaitAndStop(2f));

        UpdateWheels();

        // Thiết lập tốc độ khác và chờ dừng sau 2 giây
        SetSpeed(-100f, -100f, -100f, 100f);
        StartCoroutine(WaitAndStop(2f));

        UpdateWheels();
    }

    private void FixedUpdate()
    {
        /*SetSpeed(100f, 100f, 100f, 100f);
        StartCoroutine(WaitAndStop(2f));
        UpdateWheels();
        SetSpeed(-100f, -100f, -100f, 100f);
        StartCoroutine(WaitAndStop(2f));
        UpdateWheels();*/
        UpdateWheels();
    }

    public void MoveCar(string command)
    {
        // Tách chuỗi thành các tham số tốc độ và thời gian
        string[] parameters = command.Split(',');
        float frontLeftSpeed = float.Parse(parameters[0]);
        float frontRightSpeed = float.Parse(parameters[1]);
        float rearLeftSpeed = float.Parse(parameters[2]);
        float rearRightSpeed = float.Parse(parameters[3]);
        float duration = float.Parse(parameters[4]);

        // Thiết lập tốc độ cho các bánh
        SetSpeed(frontLeftSpeed, frontRightSpeed, rearLeftSpeed, rearRightSpeed);

        // Chạy coroutine để dừng xe sau khoảng thời gian `duration`
       // StartCoroutine(WaitAndStop(duration));
    }

    private IEnumerator WaitAndStop(float duration)
    {
        // Chờ trong khoảng thời gian `duration` giây
        yield return new WaitForSeconds(duration);

        // Dừng xe sau khi thời gian hoàn tất
        SetSpeed(0f, 0f, 0f, 0f);
    }

    void Streering(float angle)
    {
        frontLeftWheelCollider.steerAngle = angle;
        frontRightWheelCollider.steerAngle = angle;
    }
    void SetSpeed(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed)
    {
        frontLeftWheelCollider.motorTorque = frontLeftSpeed;
        frontRightWheelCollider.motorTorque = frontRightSpeed;
        rearLeftWheelCollider.motorTorque = rearLeftSpeed;
        rearRightWheelCollider.motorTorque = rearRightSpeed;

        UpdateWheels();

    }

    void UpdateWheels()
    {
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransForm);
        RotateWheel(frontRightWheelCollider, frontRightWheelTransForm);
        RotateWheel(rearLeftWheelCollider, rearLeftWheelTransForm);
        RotateWheel(rearRightWheelCollider, rearRightWheelTransForm);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos ,out rot);
        transform.position = pos;
        transform.rotation = rot;

    }
}
