using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShaderCompiler
{
    public class CompileOptions
    {
        public enum InputLanguage
        {
            GLSL = 0,
            HLSL = 1,
        };

        public enum OptimizationLevel
        {
            None,
            Size,  // optimize towards reducing code size
            Performance,  // optimize towards performance
        };

        public enum Environment
        {
            Vulkan,
            OpenGL,
            OpenGL_Compat,
        };

        public delegate IncludeResult IncludeHandler(string requestedSource, string requestingSource);

        private IntPtr _handle;
        private InputLanguage _lang = InputLanguage.GLSL;
        private OptimizationLevel _level = OptimizationLevel.None;
        private Environment _env = Environment.Vulkan;
        private bool _debug = false;
        private ShadercNative.ReleaseInclude _includeReleaser;
        private Dictionary<int, IncludeResult> _includeMap;
        private int _nextInclude = 0;

        public IntPtr NativeHandle => _handle;
        public InputLanguage Language
        {
            get { return _lang; }
            set
            {
                _lang = value;
                ShadercNative.shaderc_compile_options_set_source_language(_handle, (int)_lang);
            }
        }
        public bool GenerateDebug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                ShadercNative.shaderc_compile_options_set_generate_debug_info(_handle);
            }
        }
        public OptimizationLevel Optimization
        {
            get { return _level; }
            set
            {
                _level = value;
                ShadercNative.shaderc_compile_options_set_optimization_level(_handle,(int)_level);
            }
        }
        public Environment Target
        {
            get { return _env; }
            set
            {
                _env = value;
                //TODO add proper version support
                ShadercNative.shaderc_compile_options_set_target_env(_handle, (int)_env, 0);
            }
        }
        public IncludeHandler IncludeCallback;

        public CompileOptions()
        {
            _handle = ShadercNative.shaderc_compile_options_initialize();
            ShadercNative.shaderc_compile_options_set_include_callbacks(_handle, DelegateWrapper, ReleaseInclude, IntPtr.Zero);
        }

        public CompileOptions(IntPtr handle)
        {
            _handle = handle;
        }

        ~CompileOptions()
        {
            ShadercNative.shaderc_compile_options_release(_handle);
        }

        public CompileOptions Clone()
        {
            return new CompileOptions(ShadercNative.shaderc_compile_options_clone(_handle));
        }

        private ShadercNative.IncludeResult DelegateWrapper(IntPtr userData, [MarshalAs(UnmanagedType.LPStr)] string requestedSource, int type,
                                         [MarshalAs(UnmanagedType.LPStr)] string requestingSource, UIntPtr includeDepth)
        {
            var result = IncludeCallback(requestedSource, requestingSource);

            return  result.NativeStruct;
        }

        private void ReleaseInclude(IntPtr userData, ShadercNative.IncludeResult result)
        {
            
        }
    }
}
