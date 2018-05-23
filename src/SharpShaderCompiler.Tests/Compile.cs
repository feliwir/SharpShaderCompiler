using System;
using Xunit;

namespace SharpShaderCompiler.Tests
{
    public class Compile
    {
        /// <summary>
        /// Tests if the compiler can compile a simple GLSL shader in a Vulkan environment
        /// </summary>
        [Fact]
        public void CompileGLSL_Vulkan()
        {
            var c = new ShaderCompiler();
            Assert.True(c.NativeHandle != null, "[VULKAN] GLSL->SPIRV: NativeHandle for ShaderCompiler is null!");

            var o = new CompileOptions();
            Assert.True(o.NativeHandle != null, "[VULKAN] GLSL->SPIRV: NativeHandle for CompileOptions is null!");

            o.Language = CompileOptions.InputLanguage.GLSL;

            string testShader =  @"#version 450
                                   void main()
                                   {}";

            var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o,"testShader");

            Assert.True(r.NativeHandle != null, "[VULKAN] GLSL->SPIRV: NativeHandle for CompileResult is null!");

            Assert.True(r.CompileStatus == CompileResult.Status.Success,"[VULKAN] GLSL-> SPIRV: Compilation failed!");

            var bc = r.GetBytes();

            Assert.True(bc.Length > 0, "[VULKAN] GLSL-> SPIRV: No bytecode produced!");
        }

        /// <summary>
        /// Tests if the compiler can compile a simple GLSL shader in a Vulkan environment
        /// </summary>
        [Fact]
        public void CompileGLSL_OpenGL()
        {
            var c = new ShaderCompiler();
            Assert.True(c.NativeHandle != null, "[OpenGL] GLSL->SPIRV: NativeHandle for ShaderCompiler is null!");

            var o = new CompileOptions();
            Assert.True(o.NativeHandle != null, "[OpenGL] GLSL->SPIRV: NativeHandle for CompileOptions is null!");

            o.Language = CompileOptions.InputLanguage.GLSL;
            o.Target = CompileOptions.Environment.OpenGL;

            string testShader = @"#version 450
                                   void main()
                                   {}";

            var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.NativeHandle != null, "[OpenGL] GLSL->SPIRV: NativeHandle for CompileResult is null!");

            Assert.True(r.CompileStatus == CompileResult.Status.Success, "[VULKAN] GLSL-> SPIRV: Compilation failed!");

            var bc = r.GetBytes();

            Assert.True(bc.Length > 0, "[OpenGL] GLSL-> SPIRV: No bytecode produced!");
        }
    }
}
