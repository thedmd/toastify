using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Aleab.Common.Extensions
{
    public static class StackTraceExtensions
    {
        #region Static Members

        public static string GetFrames(this StackTrace stackTrace, int startingFrame, int frameCount, int indentDepth = 1)
        {
            if (stackTrace == null)
                throw new ArgumentNullException(nameof(stackTrace));

            if (frameCount <= 0)
                return string.Empty;

            if (startingFrame <= 0)
                startingFrame = 0;
            if (startingFrame > stackTrace.FrameCount - 1)
                return string.Empty;

            int fc = stackTrace.FrameCount - startingFrame;
            if (frameCount > fc)
                frameCount = fc;

            if (indentDepth < 0)
                indentDepth = 0;

            var frames = from f in stackTrace.GetFrames()?.Skip(startingFrame).Take(frameCount)
                         let method = f.GetMethod()
                         let @class = method.ReflectedType
                         let @params = method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}")
                         let lineNumber = f.GetFileLineNumber()
                         where method != null && @class != null
                         let indent = new StringBuilder().Append(' ', 3 * indentDepth).ToString()
                         let sLineNumber = lineNumber > 0 ? $", {lineNumber}" : string.Empty
                         select $"{indent}at {@class.FullName}.{method.Name}({string.Join(", ", @params)}){sLineNumber}";
            return string.Join(Environment.NewLine, frames);
        }

        #endregion
    }
}