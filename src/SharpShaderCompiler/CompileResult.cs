using System;
using System.Collections.Generic;
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

        IntPtr NativeHandle => _handle;

        public Status CompileStatus
        {
            get
            {
                return (Status)ShadercNative.shaderc_result_get_compilation_status(_handle);
            }
        }

        public CompileResult(IntPtr handle)
        {
            _handle = handle;
        }

    }
}
