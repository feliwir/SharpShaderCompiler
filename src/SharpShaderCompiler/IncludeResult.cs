using System;
using System.Runtime.InteropServices;

namespace SharpShaderCompiler
{
    public class IncludeResult
    {


        internal ShadercNative.IncludeResult NativeStruct
        {
            get
            {
                return new ShadercNative.IncludeResult()
                {
                    sourceName = _sourceName,
                    sourceNameLength = new UIntPtr((uint) _sourceName.Length),
                    content = _content,
                    contentLength = new UIntPtr((uint) _content.Length),
                    userData = IntPtr.Zero
                };
            }
        }

        string _sourceName;
        string _content;

        public IncludeResult(string sourceName, string content)
        {
            _sourceName = sourceName;
            _content = content;
        }

    }
}
