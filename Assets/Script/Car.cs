using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public class Car
    {
        public float FrontLeftSpeed { get; set; }
        public float FrontRightSpeed { get; set; }
        public float RearLeftSpeed { get; set; }
        public float RearRightSpeed { get; set; }
        public float Angle { get; set; }
        public float Duration { get; set; }

        public Car(float frontLeftSpeed, float frontRightSpeed, float rearLeftSpeed, float rearRightSpeed, float duration)
        {
            FrontLeftSpeed = frontLeftSpeed;
            FrontRightSpeed = frontRightSpeed;
            RearLeftSpeed = rearLeftSpeed;
            RearRightSpeed = rearRightSpeed;
            Duration = duration;
        }
        public Car(float angle)
        {
            Angle = angle;
        }

        public Car() { }

        public void SetDuration(float duration = Mathf.Infinity)
        {
            Duration = duration;
        }

    }
}
