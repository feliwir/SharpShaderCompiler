# SharpShaderCompiler

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/feliwir/SharpShaderCompiler/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/k3cl4ce8sgry8wy3?svg=true)](https://ci.appveyor.com/project/feliwir/sharpshadercompiler)

**SharpShaderCompiler** is a .NET shader compiler for compiling GLSL and HLSL to SPIRV bytecode.

## Example

```csharp
void Compile()
{
    //Create a new compiler and new options
    var c = new ShaderCompiler();
    var o = new CompileOptions();

    //Set our compile options
    o.Language = CompileOptions.InputLanguage.GLSL;
    o.Optimization = CompileOptions.OptimizationLevel.Performance;

    //Specify a minimal vertex shader
    string testShader =  @"#version 450
                            void main()
                            {}";

    //Compile the specified vertex shader and give it a name
    var r = c.Compile(testShader, ShaderCompiler.Stage.Vertex, o,"testShader");

    //Check if we had any compilation errors
    if(r.CompileStatus != CompileResult.Status.Success)
    {
        //Write the error out
        System.Console.WriteLine(r.ErrorMessage);
        return;
    }

    //Get the produced SPV bytecode
    var bc = r.GetBytes();
}
```