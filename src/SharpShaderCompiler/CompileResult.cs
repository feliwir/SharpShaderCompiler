using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShaderCompiler
{
    public class CompileResult
    {
        public enum Status
        {
            Success = 0,
            InvalidStage,  // error stage deduction
            CompilationError,
            InternalError,  // unexpected failure
            NullResultObject,
            InvalidAssembly,
        };

        IntPtr _handle;

        public IntPtr NativeHandle => _handle;
        public ulong NumberOfWarnings
        {
            get
            {
                return ShadercNative.shaderc_result_get_num_warnings(_handle).ToUInt64();
            }
        }
        public ulong NumberOfErrors
        {
            get
            {
                return ShadercNative.shaderc_result_get_num_errors(_handle).ToUInt64();
            }
        }
        public Status CompileStatus
        {
            get
            {
                return (Status)ShadercNative.shaderc_result_get_compilation_status(_handle);
            }
        }
        public string ErrorMessage
        {
            get
            {
                IntPtr ptr = ShadercNative.shaderc_result_get_error_message(_handle);
                return Marshal.PtrToStringAnsi(ptr);
            }
        }

        public byte[] GetBytes()
        {
            int size = (int)ShadercNative.shaderc_result_get_length(_handle);
            IntPtr nativeBuf = ShadercNative.shaderc_result_get_bytes(_handle);
            byte[] result = new byte[size];
            Marshal.Copy(nativeBuf, result, 0, size);

            return result;
        }

        public CompileResult(IntPtr handle)
        {
            _handle = handle;
        }

    }
}
