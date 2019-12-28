using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Kernel32
{
    /// <summary>
    /// The System.IO flags do not support file access for devices.
    /// </summary>
    /// <remarks>
    /// https://www.pinvoke.net/default.aspx/kernel32.CreateFile
    /// </remarks>
    public enum CreateFileDesiredAccess : uint
    {
        AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
        MaximumAllowed       = 0x2000000,     // MaximumAllowed access type

        Delete      = 0x10000,
        ReadControl = 0x20000,
        WriteDAC    = 0x40000,
        WriteOwner  = 0x80000,
        Synchronize = 0x100000,

        StandardRightsRequired = 0xF0000,
        StandardRightsRead = ReadControl,
        StandardRightsWrite = ReadControl,
        StandardRightsExecute = ReadControl,
        StandardRightsAll = 0x1F0000,
        SpecificRightsAll = 0xFFFF,

        FILE_READ_DATA            = 0x0001, // file & pipe
        FILE_LIST_DIRECTORY       = 0x0001, // directory
        FILE_WRITE_DATA           = 0x0002, // file & pipe
        FILE_ADD_FILE             = 0x0002, // directory
        FILE_APPEND_DATA          = 0x0004, // file
        FILE_ADD_SUBDIRECTORY     = 0x0004, // directory
        FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
        FILE_READ_EA              = 0x0008, // file & directory
        FILE_WRITE_EA             = 0x0010, // file & directory
        FILE_EXECUTE              = 0x0020, // file
        FILE_TRAVERSE             = 0x0020, // directory
        FILE_DELETE_CHILD         = 0x0040, // directory
        FILE_READ_ATTRIBUTES      = 0x0080, // all
        FILE_WRITE_ATTRIBUTES     = 0x0100, // all

        //
        // Generic Section
        //

        GENERIC_READ    = 0x80000000,
        GENERIC_WRITE   = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL     = 0x10000000,
    }
}
