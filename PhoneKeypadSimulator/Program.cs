using PhoneKeypadSimulator.Services;

namespace PhoneKeypadSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("   Phone Keypad Simulator");
            Console.WriteLine("========================================\n");
            Console.WriteLine("Instructions:");
            Console.WriteLine("  - Type number keys (0-9) to input text");
            Console.WriteLine("  - Press '0' for space (acts as separator between sequences)");
            Console.WriteLine("  - Press '*' for backspace (delete last character)");
            Console.WriteLine("  - Press '#' or Enter to send message");
            Console.WriteLine("  - Press Escape to cancel\n");
            Console.WriteLine("========================================\n");


            var keypadService = new KeypadServices();

            while (true)
            {
                Console.Write("Input: ");
                string input = GetUserInput();
                
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                string result = keypadService.ConvertText(input);
                Console.WriteLine($"Output: \"{result}\"\n");

                Console.Write("Do you want to continue? (Y/N): ");
                if (char.ToLower(Console.ReadKey().KeyChar) != 'y')
                {
                    break;
                }
                Console.WriteLine("\n");
            }
        }

        static string GetUserInput()
        {
            System.Text.StringBuilder input = new System.Text.StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return string.Empty;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.Write('#');
                    input.Append('#');
                    break;
                }

                if (char.IsDigit(keyInfo.KeyChar) || keyInfo.KeyChar == '*' || keyInfo.KeyChar == '#')
                {
                    if (keyInfo.KeyChar == '0')
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write(keyInfo.KeyChar);
                    }
                    input.Append(keyInfo.KeyChar);

                    if (keyInfo.KeyChar == '#')
                    {
                        break;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
            }

            Console.WriteLine();
            return input.ToString();
        }
    }
}
