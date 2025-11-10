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
            int? lastProcessedButton = null;

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
                        char removed = result[result.Count - 1];
                        result.RemoveAt(result.Count - 1);
                        if (removed == ' ')
                        {
                            lastProcessedButton = 0;
                        }
                        else if (result.Count == 0)
                        {
                            lastProcessedButton = null;
                        }
                    }
                    i++;
                    continue;
                }

                if (currentChar == '0')
                {
                    result.Add(' ');
                    lastProcessedButton = 0;
                    i++;
                    continue;
                }

                if (currentChar == ' ' || currentChar == '0')
                {
                    int spaceStart = i;
                    while (i < input.Length && (input[i] == ' ' || input[i] == '0'))
                    {
                        i++;
                    }

                    if (i < input.Length && char.IsDigit(input[i]) && input[i] != '0')
                    {
                        int nextButton = int.Parse(input[i].ToString());
                        if (lastProcessedButton.HasValue && 
                            lastProcessedButton.Value != 0 && 
                            lastProcessedButton.Value != nextButton)
                        {
                            result.Add(' ');
                        }
                    }
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

                    while (j < input.Length && 
                           input[j] != ' ' && 
                           input[j] != '0' && 
                           input[j] != SEND_CHAR && 
                           input[j] != BACKSPACE_CHAR &&
                           input[j] == currentChar)
                    {
                        pressCount++;
                        j++;
                    }

                    if (pressCount > 0)
                    {
                        int charIndex = (pressCount - 1) % characters.Length;
                        char character = char.ToLower(characters[charIndex]);
                        result.Add(character);
                        lastProcessedButton = buttonNumber;
                    }

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
