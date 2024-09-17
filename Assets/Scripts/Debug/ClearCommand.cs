using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDebug
{
    [CreateAssetMenu(fileName = "ClearCommand", menuName = "Developer/Commands/ClearCommand")]
    public class ClearCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            DevConsoleBehaviour.Instance.ClearLog();
            return true;
        }
    }
}
