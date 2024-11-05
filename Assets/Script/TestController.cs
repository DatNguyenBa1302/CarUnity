using Assets.Script;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestController : MonoBehaviour
{
    private Rigidbody rb;

    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider; 
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontRightWheelTransForm;
    public Transform frontLeftWheelTransForm;
    public Transform rearRightWheelTransForm;
    public Transform rearLeftWheelTransForm;

    //Khai báo các render texture cho camera
    public RenderTexture cameraTextureLeft;
    public RenderTexture cameraTextureCenter;
    public RenderTexture cameraTextureRight;

    private Queue<Car> movementQueue = new Queue<Car>();
    private bool isExecuting = false;
    // Start is called before the first frame update

    public void MoveCar(string dataFrontEnd)
    {
        Debug.Log("Received command: " + dataFrontEnd); // Xác nhận lệnh từ WebGL

        string[] parameters = dataFrontEnd.Split(',');
        if (parameters.Length == 5)
        {
            float frontLeftSpeed = float.Parse(parameters[0]);
            float frontRightSpeed = float.Parse(parameters[1]);
            float rearLeftSpeed = float.Parse(parameters[2]);
            float rearRightSpeed = float.Parse(parameters[3]);
            float duration = float.Parse(parameters[4]);

            AddQueue(frontLeftSpeed, frontRightSpeed, rearLeftSpeed, rearRightSpeed, duration);
        }else if(parameters.Length == 1)
        {
            Steering(float.Parse(parameters[0]));
        }

    }

    public void ReceiveCommand(string json)
    {
        // Phân tích chuỗi thành các phần
        Car[] commands = JsonConvert.DeserializeObject<Car[]>(json);

        foreach (var command in commands)
        {
            movementQueue.Enqueue(command);
        }

        if (!isExecuting)
        {
            StartCoroutine(ExecuteMovementQueue());
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementQueue.Enqueue(new Car(80, 80, 80, 80, Mathf.Infinity));

        //Steering(30);
        StartExecution();

    }
    public void AddQueue(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed, float duration)
    {
        movementQueue.Enqueue(new Car(frontLeftSpeed, frontRightSpeed, rearLeftSpeed, rearRightSpeed,duration));
        StartExecution();
    }

    public void StartExecution()
    {
        if (!isExecuting && movementQueue.Count > 0)
        {
            StartCoroutine(ExecuteMovementQueue());
        }
    }
    private void LateUpdate()
    {
        DetectLinePosition();
    }
    private void Update()
    {
        if (TrackBoundaryCheck.IsBoundary)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            SetSpeed(0, 0, 0, 0);
            movementQueue.Clear();
            
        }
        
    }



    private IEnumerator ExecuteMovementQueue()
    {
        isExecuting = true;

        while (movementQueue.Count > 0)
        {
            // Lấy hành động tiếp theo từ hàng đợi
            Car command = movementQueue.Dequeue();

            // Thiết lập tốc độ cho các bánh xe
            SetSpeed(command.FrontLeftSpeed, command.FrontRightSpeed, command.RearLeftSpeed, command.RearRightSpeed);
            UpdateWheels(); // Cập nhật hình ảnh bánh xe

            // Chờ trong khoảng thời gian `Duration` của hành động
            yield return new WaitForSeconds(command.Duration);

            // Dừng xe sau khi hoàn tất hành động, nếu có hành động tiếp theo
            UpdateWheels();
        }

        isExecuting = false;
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    private void SetSpeed(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed)
    {
        if (frontLeftSpeed != 0f && frontRightSpeed != 0f && rearLeftSpeed != 0f && rearRightSpeed != 0f)
        {
            frontLeftWheelCollider.motorTorque = frontLeftSpeed;
            frontRightWheelCollider.motorTorque = frontRightSpeed;
            rearLeftWheelCollider.motorTorque = rearLeftSpeed;
            rearRightWheelCollider.motorTorque = rearRightSpeed;
            ClearSpeed(0f);
        }
        else
        {
            ClearSpeed(1000f);
        }
        // Thiết lập tốc độ của từng bánh xe

    }

    private void Steering(float angle)
    {
        if (angle > 35f)
        {
            frontLeftWheelCollider.steerAngle = 35f;
            frontRightWheelCollider.steerAngle = 35f;
        }
        else if (angle < -35f)
        {
            frontLeftWheelCollider.steerAngle = -35f;
            frontRightWheelCollider.steerAngle = -35f;
        }
        else
        {
            frontLeftWheelCollider.steerAngle = angle;
            frontRightWheelCollider.steerAngle = angle;
        }
    }
    private void ClearSpeed(float barkTorque)
    {
        frontLeftWheelCollider.brakeTorque = barkTorque;
        frontRightWheelCollider.brakeTorque = barkTorque;
        rearLeftWheelCollider.brakeTorque = barkTorque;
        rearRightWheelCollider.brakeTorque = barkTorque;

    }
    private void UpdateWheels()
    {
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransForm);
        RotateWheel(frontRightWheelCollider, frontRightWheelTransForm);
        RotateWheel(rearLeftWheelCollider, rearLeftWheelTransForm);
        RotateWheel(rearRightWheelCollider, rearRightWheelTransForm);
    }

    private void RotateWheel(WheelCollider wheelCollider, Transform transform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        transform.position = pos;
        transform.rotation = rot;
    }

    void DetectLinePosition()
    {
        // Lấy màu từ ba camera
        bool leftOnLine = IsOnLine(cameraTextureLeft);
        bool centerOnLine = IsOnLine(cameraTextureCenter);
        bool rightOnLine = IsOnLine(cameraTextureRight);

        Debug.Log("center: " + centerOnLine);
        Debug.Log("left: " + leftOnLine);
        Debug.Log("right: " + rightOnLine);
        // Kiểm tra màu đen và điều chỉnh hướng
        if (!centerOnLine)
        {
            SetSpeed(0f, 0f, 0f, 0f);
        }
    }
    bool IsOnLine(RenderTexture cameraTexture)
    {
        Texture2D tex = new Texture2D(16, 16, TextureFormat.RGB24, false);
        RenderTexture.active = cameraTexture;
        tex.ReadPixels(new Rect(0, 0, 1, 1), 0, 0);
        tex.Apply();

        Color color = tex.GetPixel(0, 0);
        //Debug.Log($"Color detected: {color}");
        RenderTexture.active = null;

        // Kiểm tra nếu màu gần đen (một ngưỡng cho phép để nhận diện)
        return (color.r < 0.1f && color.g < 0.1f && color.b < 0.1f);
    }
}
