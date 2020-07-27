using System.Collections.Generic;

namespace ConsoleEShop.Consoles
{
    public class TestConsoleWrapper : IConsole
    {
        public List<string> LinesToRead = new List<string>();

        public TestConsoleWrapper(List<string> lines) // might need to change to params or smth
        {
            LinesToRead = lines;
        }

        public TestConsoleWrapper()
        {

        }

        public void Write(string message)
        {
        }

        public void WriteLine(string message)
        {
        }

        public string ReadLine()
        {
            string result = LinesToRead[0];
            LinesToRead.RemoveAt(0);
            return result;
        }
    }
}
