using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.SceneManagement;

public class CPUModule : MonoBehaviour
{
    public string path = @"";
    public GUIManager guiManager;
    public ColorManager colorManager;

    private BusModule BUS;
    private RAMModule RAM;
    private InstructionRegisterModule IR;
    private ALUModule ALU;
    private RegisterModule A;
    private RegisterModule B;
    private RegisterModule O;
    private ProgramCounterModule PC;

    private ExternalFuncs externalFunc;
    private Assembler assembler;

    private const int MEM_SIZE = 8;
    private List<string> codes;

    private int currentExecutionCount = 0; // corresponds to the length (# of commands) for each operation

    private readonly Dictionary<string, int[]> microcodeDict = new Dictionary<string, int[]>() {
        {"LDA", new int[] { 0, 0, 0, 1 }  },
        {"ADD", new int[] { 0, 0, 1, 0 }  },
        {"SUB", new int[] { 0, 0, 1, 1 }  },
        {"STA", new int[] { 0, 1, 0, 0 }  },
        {"LDI", new int[] { 0, 1, 0, 1 }  },
        {"JMP", new int[] { 0, 1, 1, 0 }  }

    };

    private readonly string[] strFetch = new string[5] { "PC CO", "RAM MI", "RAM OUT", "IR IN", "PC CE (inc)" };
    private readonly string[] strLDA = new string[4] { "IR OUT", "RAM MI", "RAM OUT", "A_reg IN" };
    private readonly string[] strADD = new string[8] { "IR OUT", "RAM MI", "RAM OUT", "B_reg IN", "ALU ADD (EO)", "A_reg IN", "A_reg OUT", "O_reg IN" };
    private readonly string[] strSUB = new string[8] { "IR OUT", "RAM MI", "RAM OUT", "B_reg IN", "ALU SUB (EO)", "A_reg IN", "A_reg OUT", "O_reg IN" };
    private readonly string[] strSTA = new string[4] { "IR OUT", "RAM MI", "A_reg OUT", "RAM IN" };
    private readonly string[] strLDI = new string[2] { "IR OUT", "A_reg IN" };
    private readonly string[] strJMP = new string[2] { "IR OUT", "PC J" };

   

    void Start()
    {
        RAM = GameObject.Find("RAM").GetComponent<RAMModule>();
        IR = GameObject.Find("Instruction Register").GetComponent<InstructionRegisterModule>();
        ALU = GameObject.Find("ALU").GetComponent<ALUModule>();
        PC = GameObject.Find("Program Counter").GetComponent<ProgramCounterModule>();
        BUS = GameObject.Find("Bus").GetComponent<BusModule>();

        A = GameObject.Find("Register_A").GetComponent<RegisterModule>();
        B = GameObject.Find("Register_B").GetComponent<RegisterModule>();
        O = GameObject.Find("Output Register").GetComponent<RegisterModule>();

        externalFunc = GameObject.Find("ExternalFuncs").GetComponent<ExternalFuncs>();
        assembler = GameObject.Find("Assembler").GetComponent<Assembler>();
 
    }

    private void ResetComponents()
    {    
        StopAllCoroutines();
        currentExecutionCount = 0;
        guiManager.ClearLogInputField();
        colorManager.ResetColors();
   
        BUS.BusReset();
        RAM.RAMReset();
        IR.InstructionRegisterReset();
        ALU.ALUReset();
        PC.ProgramCounterReset();
        A.RegisterReset();
        B.RegisterReset();
        O.RegisterReset();      
    }
    

    void Update()
    {
        if (guiManager.start)
        {
            Debug.Log("Restarting / starting");
            ResetComponents();

            codes = assembler.readAssemblyFromGUI(guiManager.codeStr);
            InitProgram();
            ExecuteProgram();

            guiManager.start = false;
        }
    }
    


    IEnumerator ExecuteAfterDuration(float time, Action func, string[] operationNames, int index)
    { 
        yield return new WaitForSeconds(time);

        func.Invoke();

        colorManager.ParseCommand(operationNames[index]);
        guiManager.UpdateLogInputField(operationNames[index]);
    }

    private void InvokeFunctions(List<Action> functions, int offset, string[] operationNames)
    {
        for (int i = 0; i < functions.Count; i++)
        {
            StartCoroutine(ExecuteAfterDuration(guiManager.delaySpeed * (i + 1 + offset), functions[i], operationNames, i));
        }
    }

    private void InitProgram()
    {
        for (int i = 0; i < codes.Count; i++)
        {
            string opcode = codes[i].Split(' ')[0];
            int[] memloc = externalFunc.convertIntToIntArray((Convert.ToInt32(codes[i].Split(' ')[1])), MEM_SIZE);

            int[] opcode_memaddr = externalFunc.concatArr(microcodeDict[opcode], memloc);
            RAM.initRam(i, opcode_memaddr);
        }
    }


    // fetch - 5
    private int Fetch(int offset)
    {
        List<Action> fetchActions = new List<Action>
        {
            () => PC.PC_CO(),
            () => RAM.RAM_MI(),
            () => RAM.RAM_RO(),
            () => IR.IR_II(),
            () => PC.PC_CE()
        };

        InvokeFunctions(fetchActions, offset, strFetch);

        return 5;
    }


    // LDA - 4
    private int LDA(int offset)
    {
        List<Action> LDA = new List<Action>
        {
            () => IR.IR_IO(),                      // IO
            () => RAM.RAM_MI(),                    // MI
            () => RAM.RAM_RO(),                    // RO
            () => A.REGI()                         // AI
        };

        InvokeFunctions(LDA, offset, strLDA);

        return 4;
    }


    // ADD - 8
    private int ADD(int offset)
    {
        List<Action> ADD = new List<Action>
        {
            () => IR.IR_IO(),                      // IO
            () => RAM.RAM_MI(),                    // MI
            () => RAM.RAM_RO(),                    // RO
            () => B.REGI(),                        // BI
            () => ALU.ALU_EO(true, false),         // EO
            () => A.REGI(),                        // AI
            () => A.REGO(),                        // AO
            () => O.REGI()                         // OI
        };

        InvokeFunctions(ADD, offset, strADD);

        return 8;
    }

    // SUB - 8
    private int SUB(int offset)
    {
        List<Action> SUB = new List<Action>
        {
            () => IR.IR_IO(),                      // IO
            () => RAM.RAM_MI(),                    // MI
            () => RAM.RAM_RO(),                    // RO
            () => B.REGI(),                        // BI
            () => ALU.ALU_EO(false, true),         // EO
            () => A.REGI(),                        // AI
            () => A.REGO(),                        // AO
            () => O.REGI()                         // OI 
        }; 

        InvokeFunctions(SUB, offset, strSUB);

        return 8;
    }

    // STA - 4
    private int STA(int offset)
    {
        List<Action> STA = new List<Action>
        {
            () => IR.IR_IO(),                      // IO
            () => RAM.RAM_MI(),                    // MI
            () => A.REGO(),                        // AO
            () => RAM.RAM_RI()                     // RI
        };

        InvokeFunctions(STA, offset, strSTA);

        return 4;
    }

    // LDI - 2
    private int LDI(int offset)
    {
        List<Action> LDI = new List<Action>
        {
            () => IR.IR_IO(),                      // IO
            () => A.REGI()                         // AI
        };

        InvokeFunctions(LDI, offset, strLDI);

        return 2;
    }

    // JMP - 2
    private int JMP(int offset)
    {
        List<Action> JMP = new List<Action>
        {
            () => IR.IR_IO(),
            () => PC.PC_J()
        };

        InvokeFunctions(JMP, offset, strJMP);

        return 2;
    }


   // private int jumps = 0;
    public void ExecuteProgram()
    {
        int i = 0;
        for (; i < codes.Count; i++)
        {
            currentExecutionCount += Fetch(currentExecutionCount); // always need to fetch before next operation

            string opcode = codes[i].Split(' ')[0];

            if (opcode == "LDA")
            {
                currentExecutionCount += LDA(currentExecutionCount);
            }
            else if (opcode == "ADD")
            {
                currentExecutionCount += ADD(currentExecutionCount);
            }
            else if (opcode == "SUB")
            {
                currentExecutionCount += SUB(currentExecutionCount);
            }
            else if (opcode == "STA")
            {
                currentExecutionCount += STA(currentExecutionCount);
            }
            else if (opcode == "LDI")
            {
              //  Debug.Log("Starting LDI and delay = " + currentExecutionCount);
                currentExecutionCount += LDI(currentExecutionCount);
            }
            else if (opcode == "JMP")
            {
                currentExecutionCount += JMP(currentExecutionCount);
              //  i = PC.PC_J() - 1;        // JMP
            }
            

        }
        
    }

   



}
