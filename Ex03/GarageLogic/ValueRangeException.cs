using System;

namespace Ex03.GarageLogic
{
    public class ValueRangeException : Exception
    {
        public float MaxValue { get; }
        public float MinValue { get; }

        public ValueRangeException(float i_MinValue, float i_MaxValue)
            : base($"Value out of range. Allowed range: {i_MinValue} - {i_MaxValue}.")
        {
            MinValue = i_MinValue;
            MaxValue = i_MaxValue;
        }
    }
}
