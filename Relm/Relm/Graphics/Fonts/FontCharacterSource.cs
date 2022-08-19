using System.Text;

namespace Relm.Graphics.Fonts
{
    public struct FontCharacterSource
    {
        readonly string _string;
        readonly StringBuilder _builder;
        public readonly int Length;


        public FontCharacterSource(string s)
        {
            _string = s;
            _builder = null;
            Length = s.Length;
        }


        public FontCharacterSource(StringBuilder builder)
        {
            _builder = builder;
            _string = null;
            Length = _builder.Length;
        }


        public char this[int index]
        {
            get
            {
                if (_string != null)
                    return _string[index];

                return _builder[index];
            }
        }
    }
}