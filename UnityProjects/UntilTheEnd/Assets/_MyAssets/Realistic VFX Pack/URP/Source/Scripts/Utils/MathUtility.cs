using System;
using UnityEngine;


namespace RFX.Utility
{

	public static class MathUtility
	{
		public static float Remap(
			this float value,
			float fromMin, float fromMax,
			float toMin, float toMax,
			bool cap = true)
		{
			// clamp the value between the input range
			value = cap ? Mathf.Clamp(value, fromMin, fromMax) : value;
			// map the value from the input range to the output range
			float mappedValue = toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);

			return mappedValue;
		}
	}

}