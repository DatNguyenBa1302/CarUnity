using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
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
    private bool isExecuting = false;
    // Start is called before the first frame update

    private void Start()
    {
        // Ví dụ: thêm các hành động vào hàng đợi
        // Di chuyển tới trong 2 giây
        movementQueue.Enqueue(new MovementCommand(100f, 100f, 100f, 100f, 2f));
        
        movementQueue.Enqueue(new MovementCommand(0f, 0f, 0f, 0f, 2f));         // Dừng trong 1 giây
        movementQueue.Enqueue(new MovementCommand(-100f, -100f, -100f, -100f, 2f));// Di chuyển lùi trong 2 giây
        movementQueue.Enqueue(new MovementCommand(0f, 0f, 0f, 0f, 2f));         // Dừng trong 1 giây

        // Bắt đầu thực hiện hàng đợi
        StartExecution();
    }

    public void StartExecution()
    {
        if (!isExecuting && movementQueue.Count > 0)
        {
            StartCoroutine(ExecuteMovementQueue());
        }
    }

    private void Update()
    {
        UpdateWheels();
    }
    private IEnumerator ExecuteMovementQueue()
    {
        isExecuting = true;

        while (movementQueue.Count > 0)
        {
            // Lấy hành động tiếp theo từ hàng đợi
            MovementCommand command = movementQueue.Dequeue();

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

    private void SetSpeed(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed)
    {
        if(frontLeftSpeed != 0f && frontRightSpeed != 0f && rearLeftSpeed != 0f && rearRightSpeed != 0f)
        {
            frontLeftWheelCollider.motorTorque = frontLeftSpeed;
            frontRightWheelCollider.motorTorque = frontRightSpeed;
            rearLeftWheelCollider.motorTorque = rearLeftSpeed;
            rearRightWheelCollider.motorTorque = rearRightSpeed;
            ClearSpeed(0f);
        }else
        {
            ClearSpeed(1000f);
        }
        // Thiết lập tốc độ của từng bánh xe
        
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
}
