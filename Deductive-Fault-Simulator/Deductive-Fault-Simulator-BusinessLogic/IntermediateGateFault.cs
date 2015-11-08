using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deductive_Fault_Simulator_BusinessLogic
{
    public class IntermediateGateFault
    {
        #region Variables

        public int net_no;
        public int stuck_at_fault;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Initializing Intermediate Gate Fault
        /// </summary>
        public IntermediateGateFault()
        {
            net_no = -1;
            stuck_at_fault = -1;
        }

        #endregion

    }
}
