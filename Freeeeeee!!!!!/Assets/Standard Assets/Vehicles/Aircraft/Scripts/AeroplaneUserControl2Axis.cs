using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Aeroplane
{
	[RequireComponent(typeof(AeroplaneController))]
	public class AeroplaneUserControl2Axis : MonoBehaviour
	{
		// these max angles are only used on mobile, due to the way pitch and roll input are handled
		public float maxRollAngle = 80;
		public float maxPitchAngle = 80;

		// reference to the aeroplane that we're controlling
		private AeroplaneController m_Aeroplane;


		private void Awake()
		{
			// Set up the reference to the aeroplane controller.
			m_Aeroplane = GetComponent<AeroplaneController>();
		}


		private void FixedUpdate()
		{
			// Read input for the pitch, yaw, roll and throttle of the aeroplane.
			float roll;
			float pitch;
			float yow;
			float throttle;
			bool rollLeft = Input.GetKey(KeyCode.A);
			bool rollRight = Input.GetKey(KeyCode.D);
			bool pitchUp = Input.GetKey(KeyCode.W);
			bool pitchDown = Input.GetKey(KeyCode.S);
			bool yowLeft = Input.GetKey(KeyCode.LeftArrow);
			bool yowRight = Input.GetKey(KeyCode.RightArrow);
			bool isthrottle = Input.GetKey(KeyCode.UpArrow);
			bool airBrakes = Input.GetKey(KeyCode.DownArrow);


			if (rollLeft)
				roll = -1;
			else if (rollRight)
				roll = 1;
			else
				roll = 0;


			if (pitchUp)
				pitch = 1;
			else if (pitchDown)
				pitch = -1;
			else
				pitch = 0;


			if (yowLeft)
				yow = -1;
			else if (yowRight)
				yow = 1;
			else
				yow = 0;


			if (isthrottle)
				throttle = 1;
			else if (airBrakes)
				throttle = -1;
			else
				throttle = 0;


			// auto throttle up, or down if braking.
			//float throttle = airBrakes ? -1 : 1;
#if MOBILE_INPUT
            AdjustInputForMobileControls(ref roll, ref pitch, ref throttle);
#endif
			// Pass the input to the aeroplane
			m_Aeroplane.Move(roll, pitch, yow, throttle, airBrakes);
		}


		private void AdjustInputForMobileControls(ref float roll, ref float pitch, ref float throttle)
		{
			// because mobile tilt is used for roll and pitch, we help out by
			// assuming that a centered level device means the user
			// wants to fly straight and level!

			// this means on mobile, the input represents the *desired* roll angle of the aeroplane,
			// and the roll input is calculated to achieve that.
			// whereas on non-mobile, the input directly controls the roll of the aeroplane.

			float intendedRollAngle = roll * maxRollAngle * Mathf.Deg2Rad;
			float intendedPitchAngle = pitch * maxPitchAngle * Mathf.Deg2Rad;
			roll = Mathf.Clamp((intendedRollAngle - m_Aeroplane.RollAngle), -1, 1);
			pitch = Mathf.Clamp((intendedPitchAngle - m_Aeroplane.PitchAngle), -1, 1);

			// similarly, the throttle axis input is considered to be the desired absolute value, not a relative change to current throttle.
			float intendedThrottle = throttle * 0.5f + 0.5f;
			throttle = Mathf.Clamp(intendedThrottle - m_Aeroplane.Throttle, -1, 1);
		}
	}
}
