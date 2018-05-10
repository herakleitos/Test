using System;
using System.Collections.Generic;

namespace Chaint.Common.Devices.PLC
{
    /// <summary>
    ///  Protocol types to be used with new daveInterface:
    /// </summary>
    public enum DaveProtocol
    {
        /// <summary>
        /// MPI for S7 300/400
        /// </summary>
        daveProtoMPI = 0,

        /// <summary>
        /// MPI for S7 300/400, "Andrew's version"
        /// </summary>
        daveProtoMPI2 = 1,

        /// <summary>
        /// MPI for S7 300/400, Step 7 Version, experimental 
        /// </summary>
        daveProtoMPI3 = 2,

        /// <summary>
        /// MPI for S7 300/400, "Andrew's version" with STX 
        /// </summary>
        daveProtoMPI4 = 3,

        /// <summary>
        ///  PPI for S7 200 
        /// </summary>
        daveProtoPPI = 10,

        /// <summary>
        /// S5 via programming interface 
        /// </summary>
        daveProtoAS511 = 20,

        /// <summary>
        ///  use s7onlinx.dll for transport
        /// </summary>
        daveProtoS7online = 50,


        /// <summary>
        /// ISO over TCP
        /// </summary>
        daveProtoISOTCP = 122,

        /// <summary>
        /// ISO over TCP with CP243
        /// </summary>
        daveProtoISOTCP243 = 123,

        /// <summary>
        /// MPI with IBH NetLink MPI to ethernet gateway
        /// </summary>
        daveProtoMPI_IBH = 223,


        /// <summary>
        ///  PPI with IBH NetLink PPI to ethernet gateway 
        /// </summary>
        daveProtoPPI_IBH = 224,


        /// <summary>
        ///Libnodave will pass the PDUs of S7 Communication to user defined call back functions.
        ///ProfiBus speed constants. This is the baudrate on MPI network, NOT between adapter and PC:        
        /// </summary>
        daveProtoUserTransport = 255

    }
}
