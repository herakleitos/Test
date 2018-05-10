using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Chaint.Common.Devices.PLC
{
    public class PLCErrMessage
    {
        private Hashtable htPlcErrMsg5D6 = new Hashtable();
        private Hashtable htPlcErrMsg6D0 = new Hashtable();
        private PLCType m_enmType = PLCType.PLC_5D6;

        public PLCErrMessage(PLCType plcType)
        {
            m_enmType = plcType;
            if (m_enmType == PLCType.PLC_5D6)
            {
                InitialErrMsgDB_5D6();
            }
            else
            {
                IntialErrMsgDB_6D0();
            }
        }

        /// <summary>
        /// Prodave5.6 错误代码
        /// </summary>
        private void InitialErrMsgDB_5D6()
        {
            htPlcErrMsg5D6.Add(0X0000, "no Error");
            htPlcErrMsg5D6.Add(0X00CA, "no resources available");
            htPlcErrMsg5D6.Add(0X00CB, "configuration error");
            htPlcErrMsg5D6.Add(0X00CD, "illegal call");
            htPlcErrMsg5D6.Add(0X00CE, "module not found");
            htPlcErrMsg5D6.Add(0X00CF, "driver not loaded");
            htPlcErrMsg5D6.Add(0X00D0, "hardware fault");
            htPlcErrMsg5D6.Add(0X00D1, "software fault");
            htPlcErrMsg5D6.Add(0X00D2, "memory fault");
            htPlcErrMsg5D6.Add(0X00D7, "no message");
            htPlcErrMsg5D6.Add(0X00D8, "storage fault");
            htPlcErrMsg5D6.Add(0X00DB, "internal timeout");
            htPlcErrMsg5D6.Add(0X00E1, "too manay channels open");
            htPlcErrMsg5D6.Add(0X00E2, "internal fault");
            htPlcErrMsg5D6.Add(0X00E7, "hardware fault");
            htPlcErrMsg5D6.Add(0X00E9, "sin_serv.exe not started");
            htPlcErrMsg5D6.Add(0X00EA, "protected");
            htPlcErrMsg5D6.Add(0X00F0, "scp db file does not exist");
            htPlcErrMsg5D6.Add(0X00F1, "no global does storage available");
            htPlcErrMsg5D6.Add(0X00F2, "error during transmission");
            htPlcErrMsg5D6.Add(0X00F3, "error during reception");
            htPlcErrMsg5D6.Add(0X00F4, "device does not exist");
            htPlcErrMsg5D6.Add(0X00F5, "incorrect sub system");
            htPlcErrMsg5D6.Add(0X00F6, "unknown code");
            htPlcErrMsg5D6.Add(0X00F7, "buffer too small");
            htPlcErrMsg5D6.Add(0X00F8, "buffer too small");
            htPlcErrMsg5D6.Add(0X00F9, "incorrect protocol");
            htPlcErrMsg5D6.Add(0X00FB, "reception error");
            htPlcErrMsg5D6.Add(0X00FC, "licence error");
            htPlcErrMsg5D6.Add(0X0101, "connection not established / parameterised");
            htPlcErrMsg5D6.Add(0X010A, "negative acknowledgement received / timeout error");
            htPlcErrMsg5D6.Add(0X010C, "data does not exist or disabled");
            htPlcErrMsg5D6.Add(0X012A, "system storage no longer available");
            htPlcErrMsg5D6.Add(0X012E, "incorrect parameter");
            htPlcErrMsg5D6.Add(0X0132, "no memory in DPRAM");
            htPlcErrMsg5D6.Add(0X0201, "incorrect interface specified");
            htPlcErrMsg5D6.Add(0X0202, "maximum amount of interfaces exceeded");
            htPlcErrMsg5D6.Add(0X0203, "PRODAVE already initialised");
            htPlcErrMsg5D6.Add(0X0204, "wrong parameter list");
            htPlcErrMsg5D6.Add(0X0205, "PRODAVE not initialised");
            htPlcErrMsg5D6.Add(0X0206, "handle cannot be set");
            htPlcErrMsg5D6.Add(0X0207, "data segment cannot be disabled");
            htPlcErrMsg5D6.Add(0X0300, "initialisiation error");
            htPlcErrMsg5D6.Add(0X0301, "initialisiation error");
            htPlcErrMsg5D6.Add(0X0302, "block too small,DW does not exist");
            htPlcErrMsg5D6.Add(0X0303, "block limit exceeded, correct amount");
            htPlcErrMsg5D6.Add(0X0310, "no HW found");
            htPlcErrMsg5D6.Add(0X0311, "HW defective");
            htPlcErrMsg5D6.Add(0X0312, "incorrect config param");
            htPlcErrMsg5D6.Add(0X0313, "incorrect baud rate/ interrupt vector");
            htPlcErrMsg5D6.Add(0X0314, "HSA parameterised incorrectly");
            htPlcErrMsg5D6.Add(0X0315, "MPI address error");
            htPlcErrMsg5D6.Add(0X0316, "HW device already allocated");
            htPlcErrMsg5D6.Add(0X0317, "interrupt not available");
            htPlcErrMsg5D6.Add(0X0318, "interrupt occupied");
            htPlcErrMsg5D6.Add(0X0319, "sap not occupied");
            htPlcErrMsg5D6.Add(0X031A, "no remote station found");
            htPlcErrMsg5D6.Add(0X031B, "internal error");
            htPlcErrMsg5D6.Add(0X031C, "system error");
            htPlcErrMsg5D6.Add(0X031D, "error buffer size");
            htPlcErrMsg5D6.Add(0X0320, "hardware fault");
            htPlcErrMsg5D6.Add(0X0321, "DLL function error");
            htPlcErrMsg5D6.Add(0X0330, "version conflict");
            htPlcErrMsg5D6.Add(0X0331, "error com config");
            htPlcErrMsg5D6.Add(0X0332, "hardware fault");
            htPlcErrMsg5D6.Add(0X0333, "com not configured");
            htPlcErrMsg5D6.Add(0X0334, "com not available");
            htPlcErrMsg5D6.Add(0X0335, "serial drv in use");
            htPlcErrMsg5D6.Add(0X0336, "no connection");
            htPlcErrMsg5D6.Add(0X0337, "job rejected");
            htPlcErrMsg5D6.Add(0X0380, "interal error");
            htPlcErrMsg5D6.Add(0X0381, "hardware fault");
            htPlcErrMsg5D6.Add(0X0382, "no driver or device found");
            htPlcErrMsg5D6.Add(0X0384, "no driver or device found");
            htPlcErrMsg5D6.Add(0X03FF, "system fault");
            htPlcErrMsg5D6.Add(0X0800, "toolbox occupied");

            htPlcErrMsg5D6.Add(0X4001, "connection not known");
            htPlcErrMsg5D6.Add(0X4002, "connection not established");
            htPlcErrMsg5D6.Add(0X4003, "connection is being established");
            htPlcErrMsg5D6.Add(0X4004, "connection broken down");

            htPlcErrMsg5D6.Add(0X8000, "function already actively occupied");
            htPlcErrMsg5D6.Add(0X8001, "not allowed in this operating status");
            htPlcErrMsg5D6.Add(0X8101, "hardware fault");
            htPlcErrMsg5D6.Add(0X8103, "object access not allowed");
            htPlcErrMsg5D6.Add(0X8104, "context is not supported");
            htPlcErrMsg5D6.Add(0X8105, "invalid address");
            htPlcErrMsg5D6.Add(0X8106, "type(data type) not supported");
            htPlcErrMsg5D6.Add(0X8107, "type(data type) not consistent");
            htPlcErrMsg5D6.Add(0X810A, "object does not exist");
            htPlcErrMsg5D6.Add(0X8301, "memory slot on CPU not sufficient");
            htPlcErrMsg5D6.Add(0X8404, "grave error");
            htPlcErrMsg5D6.Add(0X8500, "incorrect PDU size");
            htPlcErrMsg5D6.Add(0X8702, "address invalid");


            htPlcErrMsg5D6.Add(0XD201, "syntax error block name");
            htPlcErrMsg5D6.Add(0XD202, "syntax error function parameter");
            htPlcErrMsg5D6.Add(0XD203, "syntax error block type");
            htPlcErrMsg5D6.Add(0XD204, "no linked block in storage medium");
            htPlcErrMsg5D6.Add(0XD205, "object already exists");
            htPlcErrMsg5D6.Add(0XD206, "object already exists");
            htPlcErrMsg5D6.Add(0XD207, "block exists in EPROM");
            htPlcErrMsg5D6.Add(0XD209, "block does not exist");
            htPlcErrMsg5D6.Add(0XD20E, "no block available");
            htPlcErrMsg5D6.Add(0XD210, "block number too big");
            htPlcErrMsg5D6.Add(0XD241, "protection level of function not sufficient");
            htPlcErrMsg5D6.Add(0XD406, "information not available");

            htPlcErrMsg5D6.Add(0XEF01, "incorrect ID2");
            htPlcErrMsg5D6.Add(0XFFFB, "TeleService Library not found");
            htPlcErrMsg5D6.Add(0XFFFE, "unknown error FFFE hex");
            htPlcErrMsg5D6.Add(0XFFFF, "timeout error. Check interface");
        }

        /// <summary>
        /// PLC Prodave6.0 错误代码
        /// </summary>
        private void IntialErrMsgDB_6D0()
        {
            htPlcErrMsg6D0.Add(0x0000, "Success");
            htPlcErrMsg6D0.Add(0x0101, "Connection not established / no parameters assigned");
            htPlcErrMsg6D0.Add(0x0112, "Incorrect parameter");
            htPlcErrMsg6D0.Add(0x0113, "Invalid block type");
            htPlcErrMsg6D0.Add(0x0114, "Block not found");
            htPlcErrMsg6D0.Add(0x0115, "Block already exists");
            htPlcErrMsg6D0.Add(0x0116, "Block is write-protected");
            htPlcErrMsg6D0.Add(0x0117, "The block is too large");
            htPlcErrMsg6D0.Add(0x0118, "Invalid block number");
            htPlcErrMsg6D0.Add(0x0119, "Incorrect password entered");
            htPlcErrMsg6D0.Add(0x011A, "PG resource error");
            htPlcErrMsg6D0.Add(0x011B, "PLC resource error");
            htPlcErrMsg6D0.Add(0x011C, "Internal error: Protocol error");
            htPlcErrMsg6D0.Add(0x011D, "Too many blocks (module-related restriction)");
            htPlcErrMsg6D0.Add(0x011E, "No connection to database or S7DOS handle invalid");
            htPlcErrMsg6D0.Add(0x011F, "Result buffer too small");
            htPlcErrMsg6D0.Add(0x0120, "End of block list");
            htPlcErrMsg6D0.Add(0x0140, "Insufficient memory available");
            htPlcErrMsg6D0.Add(0x0141, "Job cannot be processed because resources are missing");
            htPlcErrMsg6D0.Add(0x0170, "The simulator could not be found");
            htPlcErrMsg6D0.Add(0x0180, "The activated online function cannot be executed");
            htPlcErrMsg6D0.Add(0x0181, "Driver already open or too many open channels");
            htPlcErrMsg6D0.Add(0x01C1, "The field structure of the current database does not match the expected format");
            htPlcErrMsg6D0.Add(0x01C3, "The length specification in the block header does not match the actual length of a section in the database");
            htPlcErrMsg6D0.Add(0x01C4,"A problem occurred processing the last ID file.");
            htPlcErrMsg6D0.Add(0x01C5, "Incorrect block format, the block does not have a valid PG format. ");
            htPlcErrMsg6D0.Add(0x01C6, "File not found. ");
            htPlcErrMsg6D0.Add(0x01C7, "Invalid operating system update components. ");
            htPlcErrMsg6D0.Add(0x01C8, "The database given as the destination already exists. ");
            htPlcErrMsg6D0.Add(0x01C9, "The database is already blocked by another application. ");
            htPlcErrMsg6D0.Add(0x01CC, "A DLL could not be loaded. ");
            htPlcErrMsg6D0.Add(0x01CD, "A function could not be found in the dynamically reloaded DLL. ");
            htPlcErrMsg6D0.Add(0x01CF, "File not found or access protection violation ");
            htPlcErrMsg6D0.Add(0x01D0, "Password not found. ");
            htPlcErrMsg6D0.Add(0x01E0, "Diagnostics not activated. ");
            htPlcErrMsg6D0.Add(0x01E1, "No diagnostic data are available. ");
            htPlcErrMsg6D0.Add(0x01E2, "The diagnostic data are inconsistent.");
            htPlcErrMsg6D0.Add(0x01F0, "Function not implemented. ");
            htPlcErrMsg6D0.Add(0x01FF, "System error.");
            
            htPlcErrMsg6D0.Add(0x0312, "Wrong configuration parameters");
            htPlcErrMsg6D0.Add(0x0315, "MPI address error");
            htPlcErrMsg6D0.Add(0x0805, "S7 protocol: Invalid flags. ");
            htPlcErrMsg6D0.Add(0x0810, "S7 protocol: The data could not be sent correctly. ");
            htPlcErrMsg6D0.Add(0x0811, "S7 protocol: No job found for the received data. ");
            htPlcErrMsg6D0.Add(0x0D80, "A diagnostic error occurred. ");
            
            htPlcErrMsg6D0.Add(0x400A, "The communications server could not be started. ");
            htPlcErrMsg6D0.Add(0x400F, "The connection to the communications server has been aborted. ");
            htPlcErrMsg6D0.Add(0x401F, "No such request in asynchronous list. ");
            htPlcErrMsg6D0.Add(0x4020, "No local memory available. ");
            htPlcErrMsg6D0.Add(0x4021, "The local memory cannot be locked. ");
            htPlcErrMsg6D0.Add(0x4022, "No reply to STEP 7 message frame. ");
            htPlcErrMsg6D0.Add(0x4023, "Messages are no longer executed by the CPU. The application does not react. ");
            htPlcErrMsg6D0.Add(0x4060, "Wrong Windows mode. ");
            htPlcErrMsg6D0.Add(0x4061, "No global memory. ");
            htPlcErrMsg6D0.Add(0x4104, "Online: No resources available in the driver. ");
            htPlcErrMsg6D0.Add(0x4107, "Online: Connection is closed. ");
            htPlcErrMsg6D0.Add(0x4109, "Online: No acknowledgment of the sending and receiving of data. ");
            htPlcErrMsg6D0.Add(0x410E, "Online: Connection aborted. ");
            htPlcErrMsg6D0.Add(0x4110, "Online: No connection established. No response from remote partner. ");
            htPlcErrMsg6D0.Add(0x4114, "Online: Connection already exists. ");
            htPlcErrMsg6D0.Add(0x4116, "Online: The connection to the target module cannot be established. ");
            htPlcErrMsg6D0.Add(0x411A, "Online: Illegal address. ");
            htPlcErrMsg6D0.Add(0x411C, "Online: Network error. ");
            htPlcErrMsg6D0.Add(0x4201, "Online: No resources available in the driver. ");
            htPlcErrMsg6D0.Add(0x4211, "Online: No other station with master capabilities could be found in the subnet. ");
            htPlcErrMsg6D0.Add(0x4212, "Online: Station not online. ");
            htPlcErrMsg6D0.Add(0x4215, "Online: Function not implemented or not permitted in current context. ");
            htPlcErrMsg6D0.Add(0x4216, "Online: Invalid DP slave station address or error message from DP slave. ");
            htPlcErrMsg6D0.Add(0x4228, "The bus parameters could not be automatically determined (online). There are no stations on the bus sending bus parameter message frames. Set the MPI/PROFIBUS interface manually. ");
            htPlcErrMsg6D0.Add(0x4230, "Online: No other active partner can be found. ");
            htPlcErrMsg6D0.Add(0x4231, "Online: Bus faulty. ");
            htPlcErrMsg6D0.Add(0x4232, "Online: Incorrect highest node address. ");
            htPlcErrMsg6D0.Add(0x4233, "Online: System error. ");
            htPlcErrMsg6D0.Add(0x42A1, "Online: Cannot initialize or open COM interface. ");
            htPlcErrMsg6D0.Add(0x42B0, "Online: Hardware not found. ");
            htPlcErrMsg6D0.Add(0x42B1, "Online: The local programming device interface is defective. ");
            htPlcErrMsg6D0.Add(0x42B2, "Online: Driver configuration error or invalid registry parameter. ");
            htPlcErrMsg6D0.Add(0x42B3, "Online: The local MPI address for the programming device/PC is higher than the maximum node address or the wrong transmission rate or wrong interrupt vector is set. ");
            htPlcErrMsg6D0.Add(0x42B5, "Online: The set local node address is already in use. ");
            htPlcErrMsg6D0.Add(0x42B6, "Online: The configured hardware interface is already being used by another programming package. Close all S7 applications and restart your programming package. ");
            htPlcErrMsg6D0.Add(0x42B7, "Online: The set interrupt vector (IRQ) is not available for this module. ");
            htPlcErrMsg6D0.Add(0x42B8, "Online: The set interrupt vector (IRQ) is already in use. ");
            htPlcErrMsg6D0.Add(0x42C0, "Online: Cannot load the selected communication driver; File not found. ");
            htPlcErrMsg6D0.Add(0x42C1, "Online: Function not implemented in loaded communication driver. ");
            htPlcErrMsg6D0.Add(0x42C2, "A connection between your PC/programming device and the PLC cannot be established. ");
            htPlcErrMsg6D0.Add(0x42D0, "Online: Incompatible adapter version or wrong adapter type connected. ");
            htPlcErrMsg6D0.Add(0x42D1, "Online: No interrupt received from PC/MPI cable. ");
            htPlcErrMsg6D0.Add(0x42D2, "Online: Communication link to the adapter damaged. ");
            htPlcErrMsg6D0.Add(0x42D3, "Online: COM port not configured under Windows. ");
            htPlcErrMsg6D0.Add(0x42D4, "Online: COM port currently not accessible. ");
            htPlcErrMsg6D0.Add(0x42D5, "Online: The serial driver is currently being used by an application with another configuration. ");
            htPlcErrMsg6D0.Add(0x42D6, "Online: The TS adapter interface is set up for a modem connection and there is no remote connection to a TS adapter. ");
            htPlcErrMsg6D0.Add(0x42D7, "Online: The TS adapter refused the job as the necessary legitimization was missing. ");
            htPlcErrMsg6D0.Add(0x42D9, "The connection was not established because another interface is already active on the TS adapter.");
            htPlcErrMsg6D0.Add(0x42E0, "Online: Windows system error in communication driver. ");
            htPlcErrMsg6D0.Add(0x42EE, "Online: No global memory available. ");
            htPlcErrMsg6D0.Add(0x42EF, "Online: SIN_SERV not started. ");
            htPlcErrMsg6D0.Add(0x42FA, "Online: Station is not online. ");
            htPlcErrMsg6D0.Add(0x4305, "Invalid S7 transport address buffer. ");
            htPlcErrMsg6D0.Add(0x4306, "Job cannot be found. Wrong wUserID or wrong Windows handle. ");
            htPlcErrMsg6D0.Add(0x4430, "Cannot read/write data record. ");
            htPlcErrMsg6D0.Add(0x4501, "Incorrect transfer parameter(s) (for example, incorrect structure Version, Pointer ZERO, etc.).");
            htPlcErrMsg6D0.Add(0x4510, "Direct connection with TS Adapter in modem mode.");
            
            htPlcErrMsg6D0.Add(0x7000, "General address error");
            htPlcErrMsg6D0.Add(0x7010, "ConTableLen smaller or 0");
            htPlcErrMsg6D0.Add(0x7011, "Connection number already assigned parameters");
            htPlcErrMsg6D0.Add(0x7012, "Range violation of connection number");
            htPlcErrMsg6D0.Add(0x7015, "Connection not loaded");
            htPlcErrMsg6D0.Add(0x7016, "Illegal MPI/Profibus/PPI address");
            htPlcErrMsg6D0.Add(0x7017, "Illegal MPI address");
            htPlcErrMsg6D0.Add(0x7018, "Illegal MPI address");
            htPlcErrMsg6D0.Add(0x7020, "No memory available");
            htPlcErrMsg6D0.Add(0x7025, "An attribute has the wrong/no value");
            htPlcErrMsg6D0.Add(0x7030, "Warning: Active connection number unloaded!!!");
            htPlcErrMsg6D0.Add(0x7040, "Buffer too small");
            htPlcErrMsg6D0.Add(0x7041, "PDU smaller than data buffer, choose smaller buffer");
            htPlcErrMsg6D0.Add(0x7042, "PDU smaller than amount, choose smaller amount");
            htPlcErrMsg6D0.Add(0x7043, "Wrong bit number (too small/large)");
            htPlcErrMsg6D0.Add(0x7045, "Error when reading data");
            htPlcErrMsg6D0.Add(0x7046, "Error when writing data");
            htPlcErrMsg6D0.Add(0x7050, "Error when reading DB");
            htPlcErrMsg6D0.Add(0x7051, "Error when writing DB");
            htPlcErrMsg6D0.Add(0x7060, "Error when reading a block");
            htPlcErrMsg6D0.Add(0x7061, "Error when writing a block");
            htPlcErrMsg6D0.Add(0x7062, "Too many data are to be read");
            htPlcErrMsg6D0.Add(0x7064, "Too many data are to be written");
            htPlcErrMsg6D0.Add(0x7065, "No block found");
            htPlcErrMsg6D0.Add(0x7066, "Block limit exceeded");
            htPlcErrMsg6D0.Add(0x7070, "No filestream, file not found");
            htPlcErrMsg6D0.Add(0x7071, "Permitted amount of error messages exceeded");
            htPlcErrMsg6D0.Add(0x7072, "Wrong entry in Error.dat");
            htPlcErrMsg6D0.Add(0x7080, "Info cannot be read");
            htPlcErrMsg6D0.Add(0x7081, "Hardware version and Firmware version cannot be read");
            htPlcErrMsg6D0.Add(0x7082, "MLFB cannot be read");
            htPlcErrMsg6D0.Add(0x7085, "Status cannot be read");
            htPlcErrMsg6D0.Add(0x7090, "A newer Block already exists");
            htPlcErrMsg6D0.Add(0x7100, "Block is password-protected, cannot be read");
            
            htPlcErrMsg6D0.Add(0x8001, "The service requested cannot be performed while the block is in the current state; other block functions are therefore not possible. Repeat the function later. ");
            htPlcErrMsg6D0.Add(0x8003, "S7 protocol error: Error occurred while transferring block. ");
            htPlcErrMsg6D0.Add(0x8100, "Application, general error: Service unknown to remote module. ");
            htPlcErrMsg6D0.Add(0x8104, "This service is not implemented on the module, or a message frame error has been registered. ");
            htPlcErrMsg6D0.Add(0x8204, "The type specification for the object is inconsistent. ");
            htPlcErrMsg6D0.Add(0x8205, "A copied block already exists and is not linked. ");
            htPlcErrMsg6D0.Add(0x8301, "Insufficient memory space or work memory on the module, or specified storage medium not accessible. ");
            htPlcErrMsg6D0.Add(0x8302, "Too few resources available or the processor resources are not available. ");
            htPlcErrMsg6D0.Add(0x8304, "No further parallel upload possible. Too few resources. ");
            htPlcErrMsg6D0.Add(0x8305, "Function not available. ");
            htPlcErrMsg6D0.Add(0x8306, "Insufficient work memory (for copying, linking, loading AWP)..");
            htPlcErrMsg6D0.Add(0x8307, "Available work memory not enough  (for copying, linking, loading AWP).");
            htPlcErrMsg6D0.Add(0x8401, "S7 protocol error: Invalid service sequence (for example, loading or uploading a block). ");
            htPlcErrMsg6D0.Add(0x8402, "Service cannot be performed owing to state of addressed object. ");
            htPlcErrMsg6D0.Add(0x8404, "S7 protocol: The function cannot be performed. ");
            htPlcErrMsg6D0.Add(0x8405, "Remote block is in DISABLE state (CFB). The function cannot be performed. ");
            htPlcErrMsg6D0.Add(0x8500, "S7 protocol error: Wrong message frame. ");
            htPlcErrMsg6D0.Add(0x8503, "Message from the module: Service canceled prematurely. ");
            htPlcErrMsg6D0.Add(0x8701, "Error addressing the object in the communications partner (for example, area length error). ");
            htPlcErrMsg6D0.Add(0x8702, "The requested service is not supported by the module. ");
            htPlcErrMsg6D0.Add(0x8703, "Access to object refused. ");
            htPlcErrMsg6D0.Add(0x8704, "Access error: Object damaged. ");
            
            htPlcErrMsg6D0.Add(0xD001, "Protocol error: Illegal job number. ");
            htPlcErrMsg6D0.Add(0xD002, "Parameter error: Illegal job variant. ");
            htPlcErrMsg6D0.Add(0xD003, "Parameter error: Debugging function not supported by module. ");
            htPlcErrMsg6D0.Add(0xD004, "Parameter error: Illegal job status. ");
            htPlcErrMsg6D0.Add(0xD005, "Parameter error: Illegal job termination. ");
            htPlcErrMsg6D0.Add(0xD006, "Parameter error: Illegal link disconnection ID. ");
            htPlcErrMsg6D0.Add(0xD007, "Parameter error: Illegal number of buffer elements. ");
            htPlcErrMsg6D0.Add(0xD008, "Parameter error: Illegal scan rate. ");
            htPlcErrMsg6D0.Add(0xD009, "Parameter error: Illegal number of executions. ");
            htPlcErrMsg6D0.Add(0xD00A, "Parameter error: Illegal trigger event. Check whether the specified trigger is permitted on this module. ");
            htPlcErrMsg6D0.Add(0xD00B, "Parameter error: Illegal trigger condition. Check whether the specified trigger is permitted on this module. ");
            htPlcErrMsg6D0.Add(0xD011, "Parameter error in path of the call environment: Block does not exist. ");
            htPlcErrMsg6D0.Add(0xD012, "Parameter error: Wrong address in block. ");
            htPlcErrMsg6D0.Add(0xD014, "Parameter error: Block being deleted/overwritten. ");
            htPlcErrMsg6D0.Add(0xD015, "Parameter error: Illegal variable address. ");
            htPlcErrMsg6D0.Add(0xD016, "Parameter error: Test jobs not possible, because of errors in user program. ");
            htPlcErrMsg6D0.Add(0xD017, "Parameter error: Illegal trigger number. ");
            htPlcErrMsg6D0.Add(0xD025, "Parameter error: Incorrect path. ");
            htPlcErrMsg6D0.Add(0xD026, "Parameter error: Incorrect access type. ");
            htPlcErrMsg6D0.Add(0xD027, "Parameter error: Invalid number of data blocks. ");
            htPlcErrMsg6D0.Add(0xD031, "Internal protocol error. ");
            htPlcErrMsg6D0.Add(0xD032, "Parameter error: Wrong result buffer length. ");
            htPlcErrMsg6D0.Add(0xD033, "Protocol error: Wrong job length. ");
            htPlcErrMsg6D0.Add(0xD03F, "Coding error: Error in parameter section (for example, reserve bytes not equal to 0). ");
            htPlcErrMsg6D0.Add(0xD041, "Data error: Illegal status list ID. ");
            htPlcErrMsg6D0.Add(0xD042, "Data error: Illegal variable address. ");
            htPlcErrMsg6D0.Add(0xD043, "Data error: Referenced job not found, check job data. ");
            htPlcErrMsg6D0.Add(0xD044, "Data error: Illegal variable value, check job data. ");
            htPlcErrMsg6D0.Add(0xD045, "Data error: Leaving the BASP control (ODIS) is not allowed in HOLD. ");
            htPlcErrMsg6D0.Add(0xD046, "Data error: Illegal measuring stage during run-time measurement. ");
            htPlcErrMsg6D0.Add(0xD047, "Data error: Illegal hierarchy in 'Read job list'. ");
            htPlcErrMsg6D0.Add(0xD048, "Data error: Illegal deletion code in 'Delete job'. ");
            htPlcErrMsg6D0.Add(0xD049, "Invalid substitute ID in 'Replace job'. ");
            htPlcErrMsg6D0.Add(0xD04A, "Error executing 'program status'. ");
            htPlcErrMsg6D0.Add(0xD05F, "Coding error: Error in data section (for example, reserve bytes not equal to 0, ...). ");
            htPlcErrMsg6D0.Add(0xD061, "Resource error: No memory space for job. ");
            htPlcErrMsg6D0.Add(0xD062, "Resource error: Job list full. ");
            htPlcErrMsg6D0.Add(0xD063, "Resource error: Trigger event occupied. ");
            htPlcErrMsg6D0.Add(0xD064, "Resource error: Memory space too small for one result buffer element. ");
            htPlcErrMsg6D0.Add(0xD065, "Resource error: Memory space too small for a number of result buffer elements. ");
            htPlcErrMsg6D0.Add(0xD066, "Resource error: The timer available for run-time measurement is occupied by another job. ");
            htPlcErrMsg6D0.Add(0xD067, "Resource error: Too many 'modify variable' jobs active (including multi-processor operation). ");
            htPlcErrMsg6D0.Add(0xD081, "Function not permitted in current mode. ");
            htPlcErrMsg6D0.Add(0xD082, "Mode error: Cannot exit HOLD mode. ");
            htPlcErrMsg6D0.Add(0xD0A1, "Function not permitted in current protection level. ");
            htPlcErrMsg6D0.Add(0xD0A2, "Function not possible at present, because a memory-modifying function is running. ");
            htPlcErrMsg6D0.Add(0xD0A3, "Too many 'modify variable' jobs active on the I/O (including multi-processor operation). ");
            htPlcErrMsg6D0.Add(0xD0A4, "'Forcing' has already been established. ");
            htPlcErrMsg6D0.Add(0xD0A5, "Referenced job not found. ");
            htPlcErrMsg6D0.Add(0xD0A6, "Job cannot be disabled/enabled. ");
            htPlcErrMsg6D0.Add(0xD0A7, "Job cannot be deleted because it is currently being read, for example. Try again. ");
            htPlcErrMsg6D0.Add(0xD0A8, "Job cannot be replaced because it is currently being read or deleted, for example. Try again. ");
            htPlcErrMsg6D0.Add(0xD0A9, "Job cannot be read because it is currently being deleted, for example. Try again. ");
            htPlcErrMsg6D0.Add(0xD0AA, "Time limit exceeded in processing operation. ");
            htPlcErrMsg6D0.Add(0xD0AB, "Invalid job parameters in process operation. ");
            htPlcErrMsg6D0.Add(0xD0AC, "Invalid job data in process operation. ");
            htPlcErrMsg6D0.Add(0xD0AD, "Operating mode already set. ");
            htPlcErrMsg6D0.Add(0xD0AE, "The job was set up over a different connection and can be handled over this connection. ");
            htPlcErrMsg6D0.Add(0xD0C1, "At least one error has been detected while accessing the variable(s). ");
            htPlcErrMsg6D0.Add(0xD0C2, "Mode transition to STOP/HOLD. ");
            htPlcErrMsg6D0.Add(0xD0C3, "At least one error has been detected while accessing the variable(s). Mode transition to STOP/HOLD. ");
            htPlcErrMsg6D0.Add(0xD0C4, "Time-out during run-time measurement. ");
            htPlcErrMsg6D0.Add(0xD0C5, "Display of block stack inconsistent, because blocks were deleted/reloaded. ");
            htPlcErrMsg6D0.Add(0xD0C6, "Job was automatically deleted as the jobs it referenced have been deleted. ");
            htPlcErrMsg6D0.Add(0xD0C7, "Job was automatically deleted because STOP mode was exited. ");
            htPlcErrMsg6D0.Add(0xD0C8, "'Block status' aborted because of inconsistencies between test job and running program. ");
            htPlcErrMsg6D0.Add(0xD0C9, "Exit the status area by resetting OB90. ");
            htPlcErrMsg6D0.Add(0xD0CA, "Exit the status area by resetting OB90 and access error reading variables before exiting. ");
            htPlcErrMsg6D0.Add(0xD0CB, "The output disable for the peripheral outputs has been activated again. ");
            htPlcErrMsg6D0.Add(0xD0CC, "The number of data for the debugging functions is restricted by the time limit. ");
            
            htPlcErrMsg6D0.Add(0xD201, "Syntax error in block name. ");
            htPlcErrMsg6D0.Add(0xD202, "Syntax error in function parameters. ");
            htPlcErrMsg6D0.Add(0xD205, "Linked block already exists in RAM: Conditional copying is not possible. ");
            htPlcErrMsg6D0.Add(0xD206, "Linked block already exists in EPROM: Conditional copying is not possible. ");
            htPlcErrMsg6D0.Add(0xD208, "Maximum number of copied (not linked) blocks on module exceeded. ");
            htPlcErrMsg6D0.Add(0xD209, "(At least) one of the given blocks not found on the module. ");
            htPlcErrMsg6D0.Add(0xD20A, "Maximum number of blocks linkable with a job exceeded. ");
            htPlcErrMsg6D0.Add(0xD20B, "Maximum number of blocks deletable with a job exceeded. ");
            htPlcErrMsg6D0.Add(0xD20C, "OB cannot be copied as the associated priority class does not exist. ");
            htPlcErrMsg6D0.Add(0xD20D, "SDB cannot be interpreted (for example, unknown number). ");
            htPlcErrMsg6D0.Add(0xD20E, "No (further) block available. ");
            htPlcErrMsg6D0.Add(0xD20F, "Module-specific maximum block size exceeded. ");
            htPlcErrMsg6D0.Add(0xD210, "Invalid block number. ");
            htPlcErrMsg6D0.Add(0xD212, "Incorrect header attribute (run-time relevant). ");
            htPlcErrMsg6D0.Add(0xD213, "Too many SDBs. Note the restrictions on the module being used. ");
            htPlcErrMsg6D0.Add(0xD216, "Invalid user program - reset module. ");
            htPlcErrMsg6D0.Add(0xD217, "Protection level specified in module properties not permitted. ");
            htPlcErrMsg6D0.Add(0xD218, "Incorrect attribute (active/passive). ");
            htPlcErrMsg6D0.Add(0xD219, "Incorrect block lengths (for example, incorrect length of first section or of the whole block). ");
            htPlcErrMsg6D0.Add(0xD21A, "Incorrect local data length or write-protection code faulty. ");
            htPlcErrMsg6D0.Add(0xD21B, "Module cannot compress or compression was interrupted early. Note the restrictions on the module being used. ");
            htPlcErrMsg6D0.Add(0xD21D, "The volume of dynamic project data transferred is illegal. It does not match the CPU configuration or the current user program. Check your settings and then save them again. ");
            htPlcErrMsg6D0.Add(0xD21E, "Unable to assign parameters to a module (such as FM, CP). The system data could not be linked.  For more information see the diagnostic buffer. ");
            htPlcErrMsg6D0.Add(0xD220, "Invalid programming language. Note the restrictions on the module being used. ");
            htPlcErrMsg6D0.Add(0xD221, "The system data for connections or routing are not valid. The clock parameters could have incorrect settings. ");
            htPlcErrMsg6D0.Add(0xD222, "The system data of the global data definition contain invalid parameters. Note the restrictions on the module being used and check your configuration. ");
            htPlcErrMsg6D0.Add(0xD223, "Error in instance data block for communication function block or maximum number of instance DBs exceeded. Check the programming and note the restrictions on the module being used. ");
            htPlcErrMsg6D0.Add(0xD224, "The SCAN system data block contains invalid parameters. ");
            htPlcErrMsg6D0.Add(0xD225, "The DP system data block contains invalid parameters. ");
            htPlcErrMsg6D0.Add(0xD226, "A structural error occurred in a block. ");
            htPlcErrMsg6D0.Add(0xD230, "A structural error occurred in a block. ");
            htPlcErrMsg6D0.Add(0xD231, "At least one loaded OB cannot be copied as the associated priority class does not exist. ");
            htPlcErrMsg6D0.Add(0xD232, "At least one block number of a loaded block is illegal. ");
            htPlcErrMsg6D0.Add(0xD234, "Block already exists in the given memory medium or in the job. ");
            htPlcErrMsg6D0.Add(0xD235, "The block contains an incorrect checksum. ");
            htPlcErrMsg6D0.Add(0xD236, "The block does not contain a checksum. ");
            htPlcErrMsg6D0.Add(0xD237, "You are about to load the block twice, i.e. a block with the same time stamp already exists on the CPU.");
            htPlcErrMsg6D0.Add(0xD238, "At least one of the blocks specified is not a DB.");
            htPlcErrMsg6D0.Add(0xD239, "At least one of the DBs specified is not available as a linked variant in the load memory.");
            htPlcErrMsg6D0.Add(0xD23A, "At least one of the DBs specified is considerably different from the copied and linked variant.");
            htPlcErrMsg6D0.Add(0xD240, "Coordination rules violated. ");
            htPlcErrMsg6D0.Add(0xD241, "The function is not permitted in the current protection level. ");
            htPlcErrMsg6D0.Add(0xD242, "Protection violation while processing F blocks. F blocks can be processed only after a password has been entered. F block SDB99 cannot be deleted.");
            htPlcErrMsg6D0.Add(0xD250, "Update and module ID or version do not match. ");
            htPlcErrMsg6D0.Add(0xD251, "Incorrect sequence of operating system components. ");
            htPlcErrMsg6D0.Add(0xD252, "Checksum error. ");
            htPlcErrMsg6D0.Add(0xD253, "No executable loader available; update only possible via memory card. ");
            htPlcErrMsg6D0.Add(0xD254, "Storage error in operating system.");
            htPlcErrMsg6D0.Add(0xD280, "Error compiling block in S7-300 CPU.");
            htPlcErrMsg6D0.Add(0xD2A1, "Another block function or a trigger on a block is active. Finish executing the other online function.");
            htPlcErrMsg6D0.Add(0xD2A2, "A trigger is active on a block. Complete the debugging function first.");
            htPlcErrMsg6D0.Add(0xD2A3, "The block is not activated (linked) or the block is about to be deleted. Repeat the function later.");
            htPlcErrMsg6D0.Add(0xD2A4, "The block is already being processed via another block function. Repeat the function later.");
            htPlcErrMsg6D0.Add(0xD2A6, "It is not possible to save and change the user program simultaneously. Repeat the function later.");
            htPlcErrMsg6D0.Add(0xD2A7, "The block has the attribute 'unlinked' or is not being processed. It is not possible to carry out a debugging function for this block.");
            htPlcErrMsg6D0.Add(0xD2A8, "A running debugging function is preventing parameters from being assigned to the CPU. Complete the debugging function first.");
            htPlcErrMsg6D0.Add(0xD2A9, "New parameters are being assigned to the CPU. It is not possible to load the user program at the same time. Repeat the function later.");
            htPlcErrMsg6D0.Add(0xD2AA, "New parameters are currently being assigned to the modules. Repeat the function later.");
            htPlcErrMsg6D0.Add(0xD2AB, "The volume of dynamic project data is currently being changed. The user program is being re-evaluated. Wait until the system has finished re-evaluating, and then repeat your job. ");
            htPlcErrMsg6D0.Add(0xD2AC, "The requested changes cannot be activated while configuring in RUN (CiR), since running jobs are not terminated yet. Execute the function later.");
            htPlcErrMsg6D0.Add(0xD2B0, "An error occurred while configuring in RUN (CiR). The changes requested are not valid.");
            htPlcErrMsg6D0.Add(0xD2C0, "The number of technological objects has been exceeded. Reduce the number of amount of technological objects (axes, cam,...).");
            htPlcErrMsg6D0.Add(0xD2C1, "The same technology data block already exists on the module");
            htPlcErrMsg6D0.Add(0xD2C2, "Download user program or download hardware configuration is not possible, if  Enable Peripheral Outputs is active. Exit Enable Peripheral Outputs before download.");
            
            htPlcErrMsg6D0.Add(0xD401, "Information function unavailable.");
            htPlcErrMsg6D0.Add(0xD402, "Information function unavailable.");
            htPlcErrMsg6D0.Add(0xD403, "Service has already been logged on/off (Diagnostics/PMC).");
            htPlcErrMsg6D0.Add(0xD404, "Maximum number of nodes reached. No more logons possible for diagnostics/PMC.");
            htPlcErrMsg6D0.Add(0xD405, "Service not supported or syntax error in function parameters.");
            htPlcErrMsg6D0.Add(0xD406, "Required information currently unavailable.");
            htPlcErrMsg6D0.Add(0xD407, "Diagnostics error occurred.");
            htPlcErrMsg6D0.Add(0xD408, "Update aborted.");
            htPlcErrMsg6D0.Add(0xD409, "Error on DP bus.");
            htPlcErrMsg6D0.Add(0xD601, "Syntax error in function parameter.");
            htPlcErrMsg6D0.Add(0xD602, "Incorrect password entered.");
            htPlcErrMsg6D0.Add(0xD603, "Link has already been legitimized.");
            htPlcErrMsg6D0.Add(0xD604, "Link has already been enabled.");
            htPlcErrMsg6D0.Add(0xD605, "Legitimization not possible as password does not exist.");
            htPlcErrMsg6D0.Add(0xD801, "At least one variable address is invalid.");
            htPlcErrMsg6D0.Add(0xD802, "Specified job does not exist.");
            htPlcErrMsg6D0.Add(0xD803, "Illegal job status.");
            htPlcErrMsg6D0.Add(0xD804, "Illegal cycle time (illegal time base or multiple).");
            htPlcErrMsg6D0.Add(0xD805, "Additional cyclic read job cannot be set up.");
            htPlcErrMsg6D0.Add(0xD806, "The referenced job is in a state in which the requested function cannot be performed.");
            htPlcErrMsg6D0.Add(0xD807, "Function aborted due to overload, meaning executing the read cycle takes longer than the set scan cycle time.");
            htPlcErrMsg6D0.Add(0xDC01, "Error in date and/or time specification.");
            
            htPlcErrMsg6D0.Add(0xEF01, "S7 protocol error: Error at ID2. Only 00H permitted in job.");
            htPlcErrMsg6D0.Add(0xEF02, "S7 protocol error: Error at ID2. Set of resources does not exist.");
            htPlcErrMsg6D0.Add(0xFFFE, "Error FFFE hex"); 
        }

        /// <summary>
        /// 输入错误代码的十进制数值
        /// </summary>
        /// <param name="ErrCode"></param>
        /// <returns></returns>
        public string GetErrorMsgByCode(int ErrCode)
        {
            string strErrMsg = "";
            try
            {
                if (m_enmType == PLCType.PLC_5D6)
                {
                    if (htPlcErrMsg5D6.Contains(ErrCode))
                        strErrMsg = htPlcErrMsg5D6[ErrCode].ToString();
                    else
                        strErrMsg = "-1";
                }
                else
                {
                    if (htPlcErrMsg6D0.Contains(ErrCode))
                        strErrMsg = htPlcErrMsg6D0[ErrCode].ToString();
                    else
                        strErrMsg = "-1";
                }
            }
            catch (System.Exception ex)
            {
                Console.Write(ex.Message);
                strErrMsg = string.Format("ErrCode:{0}", ErrCode);
            }
            return strErrMsg;
        }


    }
}
