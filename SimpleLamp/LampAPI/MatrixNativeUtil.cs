using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleLamp.LampAPI
{
    internal class MatrixNativeUtil
    {
        public static byte[] CreateBuffer(int bufferSize)
        {
            Queue<byte> outputValue = new Queue<byte>();

            for (int index = 0; index < bufferSize; index++)
                outputValue.Enqueue((byte)new Random().Next(0, 255));

            return outputValue.ToArray();
        }
    }
}
