using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bit [7, 6, 5, 4] IR decoder
// Bit [3, 2, 1, 0] Code for bus
public class InstructionRegisterModule : MonoBehaviour
{
    private GameObject[] gameObjBits = new GameObject[8];
    private BusModule busModule;
    private int[] busBits;

    public int[] bits = new int[8];

    private const int END = 4;
    private const int OFFSET = 0;

    public void InstructionRegisterReset()
    {
        for (int i = 0; i < gameObjBits.Length; i++)
        {
            gameObjBits[i] = gameObject.transform.GetChild(i).gameObject;
            gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        busModule = GameObject.Find("Bus").GetComponent<BusModule>();
        busBits = busModule.busBits;
    }

    void Start()
    {
        InstructionRegisterReset();
    }

    public void IR_II()
    {
        for (int i = bits.Length - 1; i >= 0; i--)
        {
            bits[i] = busBits[i];
            if (bits[i] == 1)
            {
                if (i >= 4)
                {
                    gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); // LSB
                }
                else
                {
                    gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.blue); // MSB
                }
            }
            else
            {
                gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }

        }
    }

    public void IR_IO()
    {
        busModule.writeToBus(bits, END, OFFSET);
    }
}
