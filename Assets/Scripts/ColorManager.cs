using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    private Renderer RAM;
    private Renderer IR;
    private Renderer ALU;
    private Renderer PC;
    private Renderer A;
    private Renderer B;
    private Renderer O;

    private Renderer[] components;

    private readonly Dictionary<string, int> comparisonDict = new Dictionary<string, int>()
    {
        { "RAM",    0},
        { "IR",     1},
        { "ALU",    2},
        { "PC",     3},
        { "A_reg",  4},
        { "B_reg",  5},
        { "O_reg",  6}
    };

   
    void Start()
    {
        RAM = GameObject.Find("RAM").GetComponent<Renderer>();
        IR = GameObject.Find("Instruction Register").GetComponent<Renderer>();
        ALU = GameObject.Find("ALU").GetComponent<Renderer>();
        PC = GameObject.Find("Program Counter").GetComponent<Renderer>();

        A = GameObject.Find("Register_A").GetComponent<Renderer>();
        B = GameObject.Find("Register_B").GetComponent<Renderer>();
        O = GameObject.Find("Output Register").GetComponent<Renderer>();

        components = new Renderer[]{ RAM, IR, ALU, PC, A, B, O};
    }

    public void ResetColors()
    {
        for (int i = 0; i < components.Length; i++)
        {
            components[i].material.SetColor("_Color", Color.gray);
        }
    }

    public void ParseCommand(string command)
    {
        ResetColors();

        components[comparisonDict[command.Split(' ')[0]]].material.SetColor("_Color", Color.cyan);
        
    }
}
