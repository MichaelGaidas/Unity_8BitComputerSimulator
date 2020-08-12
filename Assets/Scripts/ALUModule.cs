using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ALUModule : MonoBehaviour
{
    public GameObject[] gameObjRegs = new GameObject[2]; // Reg A / Reg B
    public int[] bits = new int[8]; 

    private GameObject[] gameObjBits = new GameObject[8]; 
    private BusModule busModule;
    private ExternalFuncs externalFunc;

    private const int END = 0;
    private const int OFFSET = 0;

    public void ALUReset()
    {
        // Get all child objects (bits) for our ALU
        for (int i = 0; i < gameObjBits.Length; i++)
        {
            gameObjBits[i] = gameObject.transform.GetChild(i).gameObject; // Bit7 = [0] ... Bit0 = [7]
            gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        busModule = GameObject.Find("Bus").GetComponent<BusModule>();
        externalFunc = GameObject.Find("ExternalFuncs").GetComponent<ExternalFuncs>();
    }

    void Start()
    {
        ALUReset();
    }

    // EO = sum for ALU of Reg A and Reg B
    public void ALU_EO(bool add, bool sub)
    {
        int t = arithmeticRegisterContents(0, 1, add, sub); // add true, sub false
        int[] res = externalFunc.convertIntToIntArray(t, 8);
        Array.Reverse(res); // reverse to MSB first

        for (int i = 0; i < 8; i++)
        {
            bits[i] = res[i];
            if (res[i] == 1)
            {
                gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            else
            {
                gameObjBits[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            }
        }

        busModule.writeToBus(bits, END, OFFSET);
    }


    // 4-Bit Adder Logic
    int arithmeticRegisterContents(int index_reg_A, int index_reg_B, bool add, bool sub)
    {
        int reg_A_bits_to_int = externalFunc.convertIntArrayToInt(gameObjRegs[index_reg_A].GetComponent<RegisterModule>().bits);
        int reg_B_bits_to_int = externalFunc.convertIntArrayToInt(gameObjRegs[index_reg_B].GetComponent<RegisterModule>().bits);

        int carry = 0;
        int sum = 0;

        if (add)
        {
            for (int i = 0; i < 8; i++)
            {
                bool bit_regA_i = (reg_A_bits_to_int & (1 << i)) != 0;
                bool bit_regB_i = (reg_B_bits_to_int & (1 << i)) != 0;

                int bit_A = bit_regA_i == true ? 1 : 0;
                int bit_B = bit_regB_i == true ? 1 : 0;

                int res_xor_AB = bit_A ^ bit_B;
                int res_and_AB = bit_A & bit_B;

                // Sum
                if ((res_xor_AB ^ carry) == 1)
                {
                    sum += 1 * (int)Math.Pow(2, i); // sum += 1 * 2^i
                    carry = 0;
                }
                if (((carry & res_xor_AB) ^ res_and_AB) == 1)
                {
                    carry = 1;
                }
            }
            if (carry == 1)
            {
                Debug.Log("Overflow Detected");
            }

        }
        else if (sub)
        {
            carry = 1; // need to carry in one bit for two's compliment
            for (int i = 0; i < 8; i++)
            {
                bool bit_regA_i = (reg_A_bits_to_int & (1 << i)) != 0;
                bool bit_regB_i = (reg_B_bits_to_int & (1 << i)) != 0;

                int bit_A = bit_regA_i == true ? 1 : 0;
                int bit_B = bit_regB_i == true ? 0 : 1; // invert bits for 2's compliment

                int res_xor_AB = bit_A ^ bit_B;
                int res_and_AB = bit_A & bit_B;

                // Sum
                if ((res_xor_AB ^ carry) == 1)
                {
                    sum += 1 * (int)Math.Pow(2, i); // sum += 1 * 2^i
                    carry = 0;
                }
                if (((carry & res_xor_AB) ^ res_and_AB) == 1)
                {
                    carry = 1;
                }
            }

            Debug.Log(sum);
            if (carry == 1) // result of subtraction is positive or zero
            {
                Debug.Log("Result of Sub. is positive or zero");
            }
            else
            {
                Debug.Log("Value is represented in unsigned interpretation");
                Debug.Log("Signed Rep:");
                Debug.Log(-256 + sum);
            }
        }

        return sum;
    }



}
