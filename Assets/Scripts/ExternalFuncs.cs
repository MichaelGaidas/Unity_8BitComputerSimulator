using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExternalFuncs : MonoBehaviour {


    public int convertIntArrayToInt(int[] reg_bits)
    {
        int bits = 0;
        int power_index = 7;
        for (int i = 0; i < reg_bits.Length; i++)
        {
            bits += reg_bits[i] * ((int)Math.Pow(2, power_index--));
        }

        return bits;
    }

    public int[] convertIntToIntArray(int bits, int size)
    {
        int[] binaryNum = new int[size];
        int i = 0;
        while (bits > 0)
        {
            binaryNum[i] = bits % 2;
            bits /= 2;
            i++;

        }
        return binaryNum;
    }

    public int[] concatArr(int[] opcode, int[] memloc)
    {
        Array.Reverse(memloc);
        int[] concatenatedArray = new int[8];
        for (int i = 0; i < concatenatedArray.Length; i++)
        {
            if (i < concatenatedArray.Length / 2)
            {
                concatenatedArray[i] = opcode[i];
            }
            else
            {
                concatenatedArray[i] = memloc[i];
            }
        }

        return concatenatedArray;
    }
}
