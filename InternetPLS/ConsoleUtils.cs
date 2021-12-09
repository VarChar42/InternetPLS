using System;
using System.Text;

namespace InternetPLS
{
    public class ConsoleUtils
    {
        public static string Prompt(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        public static string SecretPrompt(string msg)
        {
            Console.Write(msg);
            var done = false;
            var buffer = new StringBuilder();
            
            while (!done)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        done = true;
                        Console.Write('\n');
                        break;
                    case ConsoleKey.Backspace:
                        if (buffer.Length == 0) break;
                        buffer.Length -= 1;
                        (int Left, int Top) pos = Console.GetCursorPosition();
                        pos.Left -= 1;
                        Console.SetCursorPosition(pos.Left, pos.Top);
                        Console.Write(' ');
                        Console.SetCursorPosition(pos.Left, pos.Top);
                        break;
                    default:
                        if (key.KeyChar == 0) break;
                        buffer.Append(key.KeyChar);
                        Console.Write('*');
                        break;
                }
            }

            return buffer.ToString();
        }
    }
}
