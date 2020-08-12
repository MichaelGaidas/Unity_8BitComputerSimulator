using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RegisterModule : MonoBehaviour
{
    public int[] bits = new int[8];

    private GameObject[] gameObjBits = new GameObject[8];
    private GameObject busModule;
    private int[] busBits;

    private const int END = 0;
    private const int OFFSET = 0;

    public void RegisterReset()
    {
        // Get all child objects for our register
        for (int i = 0; i < gameObjBits.Length; i++)
        {
            gameObjBits[i] = gameObject.transform.GetChild(i).gameObject; // Bit7 = [0] ... Bit0 = [7]
            gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }


        busModule = GameObject.Find("Bus");
        busBits = busModule.GetComponent<BusModule>().busBits; // get bits of bus
    }

    void Start()
    {
        RegisterReset();
    }

    // read contents of bus to A register
    public void REGI()
    {
        for (int i = bits.Length - 1; i >= 0; i--)
        {
            bits[i] = busBits[i];
            if (bits[i] == 1)
            {
                gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            else
            {
                gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
    }

    public void REGO()
    {
        busModule.GetComponent<BusModule>().writeToBus(bits, END, OFFSET);
    }
}
