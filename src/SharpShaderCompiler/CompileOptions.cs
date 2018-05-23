using System;
using System.Collections.Generic;
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

        private IntPtr _handle;
        private InputLanguage _lang = InputLanguage.GLSL;
        private OptimizationLevel _level = OptimizationLevel.None;
        private Environment _env = Environment.Vulkan;
        private bool _debug = false;

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

        public CompileOptions()
        {
            _handle = ShadercNative.shaderc_compile_options_initialize();
        }

        public CompileOptions(IntPtr handle)
        {
            _handle = handle;
        }

        ~CompileOptions()
        {
            ShadercNative.shaderc_compile_options_release(_handle);
        }

        CompileOptions Clone()
        {
            return new CompileOptions(ShadercNative.shaderc_compile_options_clone(_handle));
        }
    }
}
