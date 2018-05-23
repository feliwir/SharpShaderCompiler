using System;
using Xunit;

namespace SharpShaderCompiler.Tests
{
    public class Compile
    {
        [Fact]
        public void CompileGLSL()
        {
            var c = new ShaderCompiler();
            Assert.True(c.NativeHandle != null);

            var o = new CompileOptions();
            Assert.True(o.NativeHandle != null);

            o.Language = CompileOptions.InputLanguage.GLSL;
            //o.Target = CompileOptions.Environment.OpenGL;

            string testShader =  @"#version 450
                                   void main()
                                   {}";

            var result = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o,"testShader");

            Assert.True(result.CompileStatus == CompileResult.Status.Success);
        }
    }
}
