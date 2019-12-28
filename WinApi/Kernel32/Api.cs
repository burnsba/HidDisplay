using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApi.Kernel32
{
    /// <summary>
    /// Function definitions for kernel32.dll.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file,
        /// file stream, directory, physical disk, volume, console buffer, tape drive, communications
        /// resource, mailslot, and pipe. The function returns a handle that can be used to access the
        /// file or device for various types of I/O depending on the file or device and the flags and
        /// attributes specified.
        /// To perform this operation as a transacted operation, which results in a handle that can
        /// be used for transacted I/O, use the CreateFileTransacted function.
        /// </summary>
        /// <param name="lpFileName">
        /// The name of the file or device to be created or opened. You may use either forward
        /// slashes (/) or backslashes () in this name.
        /// In the ANSI version of this function, the name is limited to MAX_PATH characters.
        /// To extend this limit to 32,767 wide characters, call the Unicode version of the
        /// function and prepend "\?" to the path.For more information, see Naming Files,
        /// Paths, and Namespaces.
        /// For information on special device names, see Defining an MS-DOS Device Name.
        /// To create a file stream, specify the name of the file, a colon, and then the
        /// name of the stream.For more information, see File Streams.
        /// </param>
        /// <param name="dwDesiredAccess">
        /// The requested access to the file or device, which can be summarized as read, write, 
        /// both or neither zero).
        /// The most commonly used values are GENERIC_READ, GENERIC_WRITE, or both
        /// (GENERIC_READ | GENERIC_WRITE). For more information, see Generic Access Rights,
        /// File Security and Access Rights, File Access Rights Constants, and ACCESS_MASK.
        /// If this parameter is zero, the application can query certain metadata such as
        /// file, directory, or device attributes without accessing that file or device,
        /// even if GENERIC_READ access would have been denied.
        /// You cannot request an access mode that conflicts with the sharing mode that
        /// is specified by the dwShareMode parameter in an open request that already
        /// has an open handle.
        /// </param>
        /// <param name="dwShareMode">
        /// The requested sharing mode of the file or device, which can be read, write,
        /// both, delete, all of these, or none (refer to the following table). Access
        /// requests to attributes or extended attributes are not affected by this flag.
        /// If this parameter is zero and CreateFile succeeds, the file or device cannot
        /// be shared and cannot be opened again until the handle to the file or device
        /// is closed.For more information, see the Remarks section.
        /// You cannot request a sharing mode that conflicts with the access mode that
        /// is specified in an existing request that has an open handle. CreateFile
        /// would fail and the GetLastError function would return ERROR_SHARING_VIOLATION.
        /// </param>
        /// <param name="lpSecurityAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but
        /// related data members: an optional security descriptor, and a Boolean value
        /// that determines whether the returned handle can be inherited by child processes.
        /// This parameter can be NULL.
        /// If this parameter is NULL, the handle returned by CreateFile cannot be inherited
        /// by any child processes the application may create and the file or device
        /// associated with the returned handle gets a default security descriptor.
        /// The lpSecurityDescriptor member of the structure specifies a SECURITY_DESCRIPTOR
        /// for a file or device. If this member is NULL, the file or device associated with
        /// the returned handle is assigned a default security descriptor.
        /// CreateFile ignores the lpSecurityDescriptor member when opening an existing file
        /// or device, but continues to use the bInheritHandle member.
        /// The bInheritHandlemember of the structure specifies whether the returned handle
        /// can be inherited.</param>
        /// <param name="dwCreationDisposition">
        /// An action to take on a file or device that exists or does not exist.
        /// For devices other than files, this parameter is usually set to OPEN_EXISTING.</param>
        /// <param name="dwFlagsAndAttributes">
        /// The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the
        /// most common default value for files.
        /// This parameter can include any combination of the available file attributes
        /// (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL.
        /// This parameter can also contain combinations of flags(FILE_FLAG_) for control
        /// of file or device caching behavior, access modes, and other special-purpose flags.
        /// These combine with any FILE_ATTRIBUTE_ values.
        /// This parameter can also contain Security Quality of Service (SQOS) information by
        /// specifying the SECURITY_SQOS_PRESENT flag.Additional SQOS-related flags information
        /// is presented in the table following the attributes and flags tables.</param>
        /// <param name="hTemplateFile">
        /// A valid handle to a template file with the GENERIC_READ access right. The
        /// template file supplies file attributes and extended attributes for the file that is
        /// being created.
        /// This parameter can be NULL.
        /// When opening an existing file, CreateFile ignores this parameter.
        /// When opening a new encrypted file, the file inherits the discretionary access control
        /// list from its parent directory.For additional information, see File Encryption.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified
        /// file, device, named pipe, or mail slot.
        /// If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended
        /// error information, call GetLastError.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        /// <summary>
        /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file,
        /// file stream, directory, physical disk, volume, console buffer, tape drive, communications
        /// resource, mailslot, and pipe. The function returns a handle that can be used to access the
        /// file or device for various types of I/O depending on the file or device and the flags and
        /// attributes specified.
        /// To perform this operation as a transacted operation, which results in a handle that can
        /// be used for transacted I/O, use the CreateFileTransacted function.
        /// </summary>
        /// <param name="lpFileName">
        /// The name of the file or device to be created or opened. You may use either forward
        /// slashes (/) or backslashes () in this name.
        /// In the ANSI version of this function, the name is limited to MAX_PATH characters.
        /// To extend this limit to 32,767 wide characters, call the Unicode version of the
        /// function and prepend "\?" to the path.For more information, see Naming Files,
        /// Paths, and Namespaces.
        /// For information on special device names, see Defining an MS-DOS Device Name.
        /// To create a file stream, specify the name of the file, a colon, and then the
        /// name of the stream.For more information, see File Streams.
        /// </param>
        /// <param name="dwDesiredAccess">
        /// The requested access to the file or device, which can be summarized as read, write, 
        /// both or neither zero).
        /// The most commonly used values are GENERIC_READ, GENERIC_WRITE, or both
        /// (GENERIC_READ | GENERIC_WRITE). For more information, see Generic Access Rights,
        /// File Security and Access Rights, File Access Rights Constants, and ACCESS_MASK.
        /// If this parameter is zero, the application can query certain metadata such as
        /// file, directory, or device attributes without accessing that file or device,
        /// even if GENERIC_READ access would have been denied.
        /// You cannot request an access mode that conflicts with the sharing mode that
        /// is specified by the dwShareMode parameter in an open request that already
        /// has an open handle.
        /// </param>
        /// <param name="dwShareMode">
        /// The requested sharing mode of the file or device, which can be read, write,
        /// both, delete, all of these, or none (refer to the following table). Access
        /// requests to attributes or extended attributes are not affected by this flag.
        /// If this parameter is zero and CreateFile succeeds, the file or device cannot
        /// be shared and cannot be opened again until the handle to the file or device
        /// is closed.For more information, see the Remarks section.
        /// You cannot request a sharing mode that conflicts with the access mode that
        /// is specified in an existing request that has an open handle. CreateFile
        /// would fail and the GetLastError function would return ERROR_SHARING_VIOLATION.
        /// </param>
        /// <param name="lpSecurityAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but
        /// related data members: an optional security descriptor, and a Boolean value
        /// that determines whether the returned handle can be inherited by child processes.
        /// This parameter can be NULL.
        /// If this parameter is NULL, the handle returned by CreateFile cannot be inherited
        /// by any child processes the application may create and the file or device
        /// associated with the returned handle gets a default security descriptor.
        /// The lpSecurityDescriptor member of the structure specifies a SECURITY_DESCRIPTOR
        /// for a file or device. If this member is NULL, the file or device associated with
        /// the returned handle is assigned a default security descriptor.
        /// CreateFile ignores the lpSecurityDescriptor member when opening an existing file
        /// or device, but continues to use the bInheritHandle member.
        /// The bInheritHandlemember of the structure specifies whether the returned handle
        /// can be inherited.</param>
        /// <param name="dwCreationDisposition">
        /// An action to take on a file or device that exists or does not exist.
        /// For devices other than files, this parameter is usually set to OPEN_EXISTING.</param>
        /// <param name="dwFlagsAndAttributes">
        /// The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the
        /// most common default value for files.
        /// This parameter can include any combination of the available file attributes
        /// (FILE_ATTRIBUTE_*). All other file attributes override FILE_ATTRIBUTE_NORMAL.
        /// This parameter can also contain combinations of flags(FILE_FLAG_) for control
        /// of file or device caching behavior, access modes, and other special-purpose flags.
        /// These combine with any FILE_ATTRIBUTE_ values.
        /// This parameter can also contain Security Quality of Service (SQOS) information by
        /// specifying the SECURITY_SQOS_PRESENT flag.Additional SQOS-related flags information
        /// is presented in the table following the attributes and flags tables.</param>
        /// <param name="hTemplateFile">
        /// A valid handle to a template file with the GENERIC_READ access right. The
        /// template file supplies file attributes and extended attributes for the file that is
        /// being created.
        /// This parameter can be NULL.
        /// When opening an existing file, CreateFile ignores this parameter.
        /// When opening a new encrypted file, the file inherits the discretionary access control
        /// list from its parent directory.For additional information, see File Encryption.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified
        /// file, device, named pipe, or mail slot.
        /// If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended
        /// error information, call GetLastError.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilea
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Microsoft.Win32.SafeHandles.SafeFileHandle CreateFile(
            string lpFileName,
            [MarshalAs(UnmanagedType.U4)] uint dwDesiredAccess,
            [MarshalAs(UnmanagedType.U4)] uint dwShareMode,
            IntPtr lpSecurityAttributes,
            [MarshalAs(UnmanagedType.U4)] uint dwCreationDisposition,
            [MarshalAs(UnmanagedType.U4)] uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        /// <summary>
        /// Retrieves a module handle for the specified module. The
        /// module must have been loaded by the calling process.
        /// To avoid the race conditions described in the Remarks section,
        /// use the GetModuleHandleEx function.
        /// </summary>
        /// <param name="lpModuleName">
        /// <para>The name of the loaded module (either a .dll or .exe file). If the file
        /// name extension is omitted, the default library extension .dll is appended. The
        /// file name string can include a trailing point character (.) to indicate that the
        /// module name has no extension. The string does not have to specify a path. When
        /// specifying a path, be sure to use backslashes (), not forward slashes (/). The name
        /// is compared (case independently) to the names of modules currently mapped into the
        /// address space of the calling process.
        /// </para>
        /// <para>If this parameter is NULL, GetModuleHandle returns a handle to the file used
        /// to create the calling process (.exe file).
        /// </para>
        /// <para>The GetModuleHandle function does not retrieve handles for modules that were
        /// loaded using the LOAD_LIBRARY_AS_DATAFILE flag. For more information, see LoadLibraryEx.
        /// </para>
        /// </param>
        /// <returns><para>If the function succeeds, the return value is a handle to the
        /// specified module.
        /// </para>
        /// <para>If the function fails, the return value is NULL. To
        /// get extended error information, call GetLastError.
        /// </para></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
