using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusModule : MonoBehaviour
{
    private GameObject[] gameObjBits = new GameObject[8]; // bus bits
    private GameObject[] gameObjRegisters = new GameObject[2]; // registers available, children of the bus
    public int[] busBits = new int[8];

    private void clearBus()
    {
        for (int i = busBits.Length - 1; i >= 0; i--)
        {
            busBits[i] = 0; // start bus low
            gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white); 
        }

    }

    public void BusReset()
    {
        for (int i = busBits.Length - 1; i >= 0; i--)
        {
            busBits[i] = 0; // start bus low
            gameObjBits[i] = gameObject.transform.GetChild(i).gameObject;
            gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        for (int i = 0; i < gameObjRegisters.Length; i++)
        {
            gameObjRegisters[i] = gameObject.transform.GetChild(i + 8).gameObject; // i+8 skip over bits 7 to bit 0 of the bus
        }
    }

    void Start()
    {
        BusReset();
    }

    public void writeToBus(int[] bits, int end, int offset)
    {
        clearBus();
        for (int i = bits.Length - 1; i >= end; i--)
        {
            busBits[i + offset] = bits[i];
            if (busBits[i + offset] == 1)
            {
                gameObjBits[i + offset].GetComponent<Renderer>().material.SetColor("_Color", Color.red); // turn red
            }
            else
            {
                gameObjBits[i + offset].GetComponent<Renderer>().material.SetColor("_Color", Color.white); // turn white
            }
        }
    }
}
