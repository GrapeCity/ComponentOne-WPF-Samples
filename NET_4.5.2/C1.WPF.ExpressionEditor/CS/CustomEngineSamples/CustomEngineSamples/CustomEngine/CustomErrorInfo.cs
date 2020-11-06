using C1.ExpressionEditor.Engine;
using Microsoft.CodeAnalysis;

namespace CustomEngineSamples
{
    public class CustomErrorInfo : ErrorInfo
    {
        private Diagnostic _diagnostic;

        public CustomErrorInfo(Diagnostic diagnostic)
        {
            _diagnostic = diagnostic;
            Position = _diagnostic.Location.GetLineSpan().StartLinePosition.Character - 8;
            Length = _diagnostic.Location.GetLineSpan().EndLinePosition.Character - Position - 8;
        }

        public override string FullMessage
        {
            get
            {
                return string.Format("({0}): {1}", Position, Message);
            }
        }

        public override string Message
        {
            get
            {
                return string.Format("error {0}: {1}", _diagnostic.Id, _diagnostic.GetMessage());
            }
        }
    }
}
