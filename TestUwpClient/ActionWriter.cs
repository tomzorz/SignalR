using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUwpClient
{
    public class ActionWriter : TextWriter
    {
        private readonly Action<string> _write;
        private readonly StringBuilder _sb;

        public ActionWriter(Action<string> write)
        {
            _write = write;
            _sb = new StringBuilder();
        }

        public override void Write(char value)
        {
            if (value == '\n')
            {
                _write(_sb.ToString());
                _sb.Clear();
            }
            else
            {
                _sb.Append(value);
            }
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
