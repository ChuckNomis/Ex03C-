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

        public class InvalidLicensePlateException : Exception
        {
            public InvalidLicensePlateException(string i_Input)
                : base($"Invalid license plate format: '{i_Input}'. Expected format is xx-xxx-xx (digits only).")
            {
            }
        }
        public class InvalidPhoneNumberException : Exception
        {
            public InvalidPhoneNumberException(string i_Input)
                : base($"Invalid phone number format: '{i_Input}'. Expected format is xxx-xxxxxxx (digits only).")
            {
            }
        }
    }
}
