using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

// 16 * 8 RAM module = 128 Bits
public class RAMModule : MonoBehaviour
{

    public int[][] reg = new int[16][]; // 16 registers, 8-bit (1 byte) memory
    public int memoryAddress; // 0x00 - 0xFF
    public int[] bits = new int[8];

    private GameObject[] fourDipBits = new GameObject[4];
    private GameObject[] ramBits = new GameObject[8];
    private BusModule busModule;
    private ExternalFuncs externalFunc;

    private const int MEM_SIZE = 8;
    private const int END = 0;
    private const int OFFSET = 0;

    public void RAMReset()
    {
        for (int i = 0; i < 16; i++)
        {
            reg[i] = new int[8];
        }

        for (int i = 0; i < fourDipBits.Length; i++)
        {
            fourDipBits[i] = gameObject.transform.GetChild(i).gameObject;
            fourDipBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        for (int i = 0; i < ramBits.Length; i++)
        {
            ramBits[i] = gameObject.transform.GetChild(i + 4).gameObject;
            ramBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }


        busModule = GameObject.Find("Bus").GetComponent<BusModule>();
        externalFunc = GameObject.Find("ExternalFuncs").GetComponent<ExternalFuncs>();
    }

    void Start()
    {
        RAMReset();
    }

    // shows contents of RAM constantly -> use in editor if needed to swap between memory locations
    public void Update()
    {
        updateDIPBits();
        updateRAMBits();
    }

    public void RAM_MI()
    {
        int bus_bits = externalFunc.convertIntArrayToInt(busModule.busBits);
        memoryAddress = bus_bits;
    }

    public void RAM_RI()
    {
        int[] bus_bits_arr = busModule.busBits;
        for (int i = bus_bits_arr.Length - 1; i >= 0; i--)
        {
            reg[memoryAddress][i] = bus_bits_arr[i];
        }
    }

    public void RAM_RO()
    {
        busModule.writeToBus(reg[memoryAddress], END, OFFSET);
    }

    public void updateDIPBits()
    {
        int[] addr_to_int = externalFunc.convertIntToIntArray(memoryAddress, MEM_SIZE);
        Array.Reverse(addr_to_int); // MSB first

        for (int i = addr_to_int.Length - 1; i >= 4; i--)
        {
            bits[i] = addr_to_int[i];
            if (addr_to_int[i] == 1)
            {
                fourDipBits[i - 4].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
            }
            else
            {
                fourDipBits[i - 4].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
        
    }

    public void updateRAMBits()
    {
        for (int i = 0; i < bits.Length; i++)
        {
            bits[i] = reg[memoryAddress][i];
            if (reg[memoryAddress][i] == 1)
            {
                ramBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            else
            {
                ramBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }
    }

    public void initRam(int loc, int[] contents)
    {  
        reg[loc] = contents;
    }




}
