using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// based on jk-flip synch binary counter (4-bit)
public class ProgramCounterModule : MonoBehaviour
{
    public int[] pcBits = new int[4];

    public bool countEnable; // increment PC
    public bool programCounterOut; // contents of counter to bus
    public bool programCounterIn; // contents of bus to counter (jump instr)

    private GameObject[] pcBitGameObj = new GameObject[4];
    private BusModule busModule;
    private ExternalFuncs externalFunc;

    private int bits;

    private const int END = 0;
    private const int OFFSET = 4;

    public void ProgramCounterReset()
    {
        busModule = GameObject.Find("Bus").GetComponent<BusModule>();
        externalFunc = GameObject.Find("ExternalFuncs").GetComponent<ExternalFuncs>();

        for (int i = 0; i < pcBits.Length; i++)
        {
            pcBits[i] = 0;
        }

        for (int i = 0; i < pcBitGameObj.Length; i++)
        {
            pcBitGameObj[i] = gameObject.transform.GetChild(i).gameObject;
            pcBitGameObj[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        bits = 0;
    }

    void Start()
    {
        ProgramCounterReset();
    }


    public void PC_CO()
    {
        busModule.writeToBus(pcBits, END, OFFSET);
    }

    public void PC_CE()
    {
        if (bits < 15)
        {
            bits++;
        }
        else
        {
            bits = 0;
        }

        int[] bits_to_intArray = externalFunc.convertIntToIntArray(bits, pcBits.Length);
        Array.Reverse(bits_to_intArray);
        for (int i = 0; i < bits_to_intArray.Length; i++)
        {
            pcBits[i] = bits_to_intArray[i];
            if (bits_to_intArray[i] == 1)
            {
                pcBitGameObj[i].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                pcBitGameObj[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
    }

    public int PC_J()
    {
        int[] bus_bits = busModule.busBits;
        for (int i = bus_bits.Length - 5; i >= 0; i--)
        {
            pcBits[i] = bus_bits[i + 4];
            if (pcBits[i] == 1)
            {
                pcBitGameObj[i].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                pcBitGameObj[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }

        bits = externalFunc.convertIntArrayToInt(bus_bits);

        return bits;
    }
}
