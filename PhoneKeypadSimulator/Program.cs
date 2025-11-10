using PhoneKeypadSimulator.Services;

namespace PhoneKeypadSimulator
{
    /// <summary>
    /// Main program for Phone Keypad Simulator.
    /// Simulates an old mobile phone keypad input system with batch text conversion.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DisplayWelcomeMessage();

            var keypadService = new KeypadServices();

            bool continueRunning = true;
            while (continueRunning)
            {
                string input = GetUserInput();
                
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                // Process the full input string
                string result = keypadService.ConvertText(input);
                
                // Display the result
                Console.WriteLine($"\n>>> Message: \"{result}\" <<<\n");

                continueRunning = AskToContinue();

                if (continueRunning)
                {
                    Console.WriteLine("\n--- New Message ---\n");
                }
            }

            Console.WriteLine("\nHappy Coding! Hope to see you again!");
        }

        /// <summary>
        /// Displays welcome message and usage instructions.
        /// </summary>
        static void DisplayWelcomeMessage()
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
        }

        /// <summary>
        /// Gets user input until '#' or Enter is pressed.
        /// </summary>
        /// <returns>The input string entered by the user</returns>
        static string GetUserInput()
        {
            Console.WriteLine("Type your message (press # or Enter to send):\n");
            Console.Write("Input: ");

            System.Text.StringBuilder input = new System.Text.StringBuilder();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                char keyPress = keyInfo.KeyChar;

                // Handle Escape key to cancel
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\n\nMessage cancelled.");
                    return string.Empty;
                }

                // Handle Enter key - treat as send (#)
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("#");
                    input.Append('#');
                    break;
                }

                // Handle printable characters
                if (char.IsDigit(keyPress) || keyPress == '*' || keyPress == '#')
                {
                    Console.Write(keyPress);
                    input.Append(keyPress);

                    // If '#' was pressed, stop reading
                    if (keyPress == '#')
                    {
                        break;
                    }
                }
                // Handle backspace key
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                        Console.Write("\b \b"); // Erase the character from console
                    }
                }
            }

            return input.ToString();
        }

        /// <summary>
        /// Asks the user if they want to send another message or exit.
        /// </summary>
        /// <returns>True if user wants to continue, False otherwise</returns>
        static bool AskToContinue()
        {
            Console.Write("Do you want to send another message? (Y/N): ");
            ConsoleKeyInfo response = Console.ReadKey();
            Console.WriteLine();

            return char.ToLower(response.KeyChar) == 'y';
        }
    }
}
