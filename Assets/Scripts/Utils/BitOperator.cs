using System.ComponentModel;

namespace Utils
{
    public static class BitOperator
    {
        public static int TurnIndexToBitIndexValue(int index)
        {
            if (index <= 0)
            {
                return 0;
            }
            
            var newIndex = 1;
            for (int i = 1; i < index; i++)
            {
                newIndex *= 2;
            }
            return newIndex;
        }
        public static int TurnBitToIndexValue(int bitIndex)
        {
            if (bitIndex <= 0)
            {
                return 0;
            }
            
            var newIndex = 0;
            for (int i = 1; i < bitIndex; i*= 2) 
            {
                newIndex++;
            }
            return newIndex;
        }

        public static int TurnCountIntoMaxBitValue(int count)
        {
            var bitIndex = 1;
            for (var i = 0; i < count; i++)
            {
                bitIndex *= 2;
            }
            return bitIndex;
        }
        
        public static bool IsActive(int container, int contained)
        {
            return (container |= contained) != 0;
        }
    }
}