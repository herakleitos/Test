using System;
using System.Collections.Generic;

namespace Chaint.Common.Devices.PLC
{
    /// <summary>
    ///  Use these constants for parameter "area" in daveReadBytes and daveWriteBytes
    /// </summary>
    public enum DaveArea
    {

        /// <summary>
        /// System info of 200 family
        /// </summary>
        daveSysInfo = 0x3,

        /// <summary>
        /// System flags of 200 family
        /// </summary>
        daveSysFlags = 0x5,
        /// <summary>
        /// analog inputs of 200 family
        /// </summary>
        daveAnaIn = 0x6,
        /// <summary>
        /// analog outputs of 200 family
        /// </summary>
        daveAnaOut = 0x7,

        /// <summary>
        /// direct peripheral access   
        /// </summary>
        daveP = 0x80,
        daveInputs = 0x81,
        daveOutputs = 0x82,
        daveFlags = 0x83,

        /// <summary>
        /// data blocks
        /// </summary>
        daveDB = 0x84,

        /// <summary>
        ///  instance data blocks
        /// </summary>
        daveDI = 0x85,

        /// <summary>
        /// not tested
        /// </summary>
        daveLocal = 0x86,

        /// <summary>
        /// don't know what it is
        /// </summary>
        daveV = 0x87,

        /// <summary>
        /// S7 counters
        /// </summary>
        daveCounter = 28,

        /// <summary>
        ///  S7 timers
        /// </summary>
        daveTimer = 29,

        /// <summary>
        /// IEC counters (200 family)
        /// </summary>
        daveCounter200 = 30,

        /// <summary>
        /// IEC timers (200 family)
        /// </summary>
        daveTimer200 = 31
    }
}
