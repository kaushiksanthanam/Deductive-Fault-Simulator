using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deductive_Fault_Simulator_BusinessLogic
{
    /// <summary>
    /// Logic Record represents the inputs,
    /// outputs and operator of a logic gate.
    /// </summary>
    public class LogicRecord
    {
        #region Variables

        public Operators _operator;
        public List<int> _inputs;
        public int _output;

        #endregion

        #region Constructor

        public LogicRecord()
        {
            _operator = Operators.NONE;
            _inputs = new List<int>();
            _output = 0;
        }

        #endregion
    }
}
