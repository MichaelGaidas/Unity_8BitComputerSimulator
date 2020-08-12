using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Assembler : MonoBehaviour {

    public List<string> readAssemblyFromFile(string path)
    {
        List<string> commands = new List<string>();
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                commands.Add(line);
            }
        }
        return commands;
    }

    public List<string> readAssemblyFromGUI(string codeString)
    {
        List<string> commands = new List<string>();
        using (StringReader reader = new StringReader(codeString))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                commands.Add(line);
            }
        }
        return commands;
    }


}
