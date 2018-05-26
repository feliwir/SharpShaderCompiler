using System;
using System.Runtime.InteropServices;

namespace SharpShaderCompiler
{
    public class ShaderCompiler
    {
        public enum Stage {
            // Forced shader kinds. These shader kinds force the compiler to compile the
            // source code as the specified kind of shader.
            Vertex,
            Fragment,
            Compute,
            Geometry,
            TessControl,
            TessEvaluation,
        }

        private IntPtr _handle;

        public IntPtr NativeHandle => _handle;

        public ShaderCompiler()
        {
            _handle = ShadercNative.shaderc_compiler_initialize();
        }

        ~ShaderCompiler()
        {
            ShadercNative.shaderc_compiler_release(_handle);
        }

        public CompileResult Compile(string source, Stage stage , CompileOptions options, string name,string entryPoint= "main")
        {       
            IntPtr resultPtr = ShadercNative.shaderc_compile_into_spv(_handle, source,new UIntPtr((uint)source.Length), (int)stage, name, entryPoint, options.NativeHandle);
            return new CompileResult(resultPtr);
        }

        public CompileResult Preprocess(string source, Stage stage, CompileOptions options, string name, string entryPoint = "main")
        {
            IntPtr resultPtr = ShadercNative.shaderc_compile_into_preprocessed_text(_handle, source, new UIntPtr((uint) source.Length), (int) stage, name, entryPoint, options.NativeHandle);
            return new CompileResult(resultPtr);
        }
    }
}
