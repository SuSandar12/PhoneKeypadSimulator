using System;
using System.Collections.Generic;
using PhoneKeypadSimulator.Models;

namespace PhoneKeypadSimulator.Services
{
    public class KeypadServices
    {
        private const char BACKSPACE_CHAR = '*';
        private const char SEND_CHAR = '#';

        public Dictionary<int, string> KeypadMapping { get; }

        public KeypadServices()
        {
            KeypadMapping = new Dictionary<int, string>(KeyMappings.KeypadMapping);
        }

        public string ConvertText(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            List<char> result = new List<char>();
            int i = 0;

            while (i < input.Length)
            {
                char currentChar = input[i];

                if (currentChar == SEND_CHAR)
                {
                    break;
                }

                if (currentChar == BACKSPACE_CHAR)
                {
                    if (result.Count > 0)
                    {
                        result.RemoveAt(result.Count - 1);
                    }
                    i++;
                    continue;
                }

                if (currentChar == '0')
                {
                    int j = i + 1;
                    while (j < input.Length && (input[j] == '0' || input[j] == ' '))
                    {
                        j++;
                    }

                    bool isSeparator = result.Count > 0 && j < input.Length && 
                                     char.IsDigit(input[j]) && 
                                     input[j] != '0' && 
                                     input[j] != SEND_CHAR && 
                                     input[j] != BACKSPACE_CHAR;

                    if (!isSeparator)
                    {
                        result.Add(' ');
                    }

                    i = j;
                    continue;
                }

                if (currentChar == ' ')
                {
                    int j = i + 1;
                    while (j < input.Length && (input[j] == ' ' || input[j] == '0'))
                    {
                        j++;
                    }

                    bool isSeparator = result.Count > 0 && j < input.Length && 
                                     char.IsDigit(input[j]) && 
                                     input[j] != '0' && 
                                     input[j] != SEND_CHAR && 
                                     input[j] != BACKSPACE_CHAR;

                    i = j;
                    continue;
                }

                if (!int.TryParse(currentChar.ToString(), out int buttonNumber))
                {
                    i++;
                    continue;
                }

                if (buttonNumber >= 1 && buttonNumber <= 9)
                {
                    if (!KeypadMapping.ContainsKey(buttonNumber) || string.IsNullOrEmpty(KeypadMapping[buttonNumber]))
                    {
                        i++;
                        continue;
                    }

                    string characters = KeypadMapping[buttonNumber];
                    int pressCount = 0;
                    int j = i;

                    while (j < input.Length && input[j] == currentChar)
                    {
                        pressCount++;
                        j++;
                    }

                    int charIndex = (pressCount - 1) % characters.Length;
                    char character = char.ToLower(characters[charIndex]);
                    result.Add(character);

                    i = j;
                }
                else
                {
                    i++;
                }
            }

            return new string(result.ToArray());
        }
    }
}
