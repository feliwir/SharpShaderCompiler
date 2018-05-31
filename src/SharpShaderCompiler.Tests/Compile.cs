using System;
using System.Linq;
using System.Text;
using Xunit;

namespace SharpShaderCompiler.Tests
{
    public class Compile
    {
        /// <summary>
        /// Tests if the compiler can compile a simple GLSL shader in a Vulkan environment
        /// </summary>
        [Fact]
        public void GlslVulkan()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;

            string testShader = @"#version 450
                                   void main()
                                   {}";

            var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.NumberOfErrors == 0, "[Vulkan] GLSL->SPIRV: Has error messages");

            Assert.True(r.CompileStatus == CompileResult.Status.Success, "[Vulkan] GLSL-> SPIRV: Compilation failed:" + r.ErrorMessage);

            var bc = r.GetBytes();

            Assert.True(bc.Length > 0, "[Vulkan] GLSL-> SPIRV: No bytecode produced!");
        }

        /// <summary>
        /// Tests if the compiler can compile a simple GLSL shader in a Vulkan environment
        /// </summary>
        [Fact]
        public void GlslOpenGL()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;
            o.Target = CompileOptions.Environment.OpenGL;

            string testShader = @"#version 450
                                   void main()
                                   {}";

            var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.NumberOfErrors == 0, "[OpenGL] GLSL->SPIRV: Has error messages");

            Assert.True(r.CompileStatus == CompileResult.Status.Success, "[OpenGL] GLSL-> SPIRV: Compilation failed:" + r.ErrorMessage);

            var bc = r.GetBytes();

            Assert.True(bc.Length > 0, "[OpenGL] GLSL-> SPIRV: No bytecode produced!");
        }

        /// <summary>
        /// Tests if the compiler detects a broken shader
        /// </summary>
        [Fact]
        public void BrokenShader()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;

            string testShader = @"#version 450
                                   void main()
                                   {";

            var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.CompileStatus == CompileResult.Status.CompilationError, "[Vulkan] GLSL-> SPIRV: Broken shader result wrong!");

            string msg = r.ErrorMessage;

            Assert.True(msg.Length > 0, "[Vulkan] GLSL->SPIRV: No error message for broken shader produced!");
        }

        /// <summary>
        /// A callback for an include event
        /// </summary>
        /// <param name="requestedSource"></param>
        /// The source that is requested
        /// <param name="requestingSource"></param>
        /// The source that is requesting this header
        /// <returns></returns>
        IncludeResult IncludeHandler(string requestedSource, string requestingSource, CompileOptions.IncludeType type)
        {
            return new IncludeResult(requestedSource, "");
        }

        /// <summary>
        /// Tests if the compiler can resolve an include directive
        /// </summary>
        [Fact]
        public void IncludeTest()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;
            o.IncludeCallback = IncludeHandler;

            string testShader = @"#version 450
                                #include ""common.glsl""
                                void main()
                                {}";

            var r = c.Preprocess(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.NumberOfErrors == 0, "[Vulkan] GLSL->PREPROCESS: Has error messages");

            Assert.True(r.CompileStatus == CompileResult.Status.Success, "[Vulkan] GLSL->PREPROCESS: Compilation failed:" + r.ErrorMessage);

            var bc = r.GetBytes();

            var preprocessed = Encoding.ASCII.GetString(bc);

            Assert.False(preprocessed.Contains("#include"), "[Vulkan] GLSL->PREPROCESS: Preprocessed shader still " +
                                                            "contains include directive:" + preprocessed);
        }

        /// <summary>
        /// Tests if the compiler gives the correct error when no include is specified
        /// </summary>
        [Fact]
        public void IncludeTestFail()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;

            string testShader = @"#version 450
                                #include ""common.glsl""
                                void main()
                                {}";

            var r = c.Preprocess(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.CompileStatus != CompileResult.Status.Success, "[Vulkan] GLSL->PREPROCESS: Compilation succeeded unexpected:" + r.ErrorMessage);
        }

        /// <summary>
        /// Tests if the compiler gives the correct error when no include is specified
        /// </summary>
        [Fact]
        public void Assemble()
        {
            var c = new ShaderCompiler();
            var o = new CompileOptions();

            o.Language = CompileOptions.InputLanguage.GLSL;

            string testShader = @"#version 450
                                void main()
                                {}";

            var r = c.Assemble(testShader, ShaderCompiler.Stage.Vertex, o, "testShader");

            Assert.True(r.CompileStatus == CompileResult.Status.Success, "[Vulkan] GLSL->ASSEMBLE: Compilation failed unexpected:" + r.ErrorMessage);

            var assembly = r.GetString();

            Assert.True(assembly.Length > 0, "[Vulkan] GLSL->ASSEMBLE: Assembly is empty!");
        }
    }
}
