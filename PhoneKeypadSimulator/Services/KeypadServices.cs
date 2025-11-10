using System;
using System.Collections.Generic;
using System.Linq;
using PhoneKeypadSimulator.Models;

namespace PhoneKeypadSimulator.Services
{
    /// <summary>
    /// Service class for processing keypad input and converting button presses to text.
    /// Processes full input strings in batch mode and returns the converted text.
    /// Handles letter cycling, space characters, backspace, and message sending.
    /// </summary>
    public class KeypadServices
    {
        // Constants for special characters
        private const char BACKSPACE_CHAR = '*';
        private const char SEND_CHAR = '#';
        private const int SPACE_BUTTON = 0;

        /// <summary>
        /// Property to access the keypad mapping dictionary.
        /// Provides read-only access to button number to character sequence mappings.
        /// </summary>
        public Dictionary<int, string> KeypadMapping { get; }


        /// <summary>
        /// Initializes a new instance of the KeypadServices class.
        /// Loads the keypad mapping from KeyMappings class.
        /// </summary>
        public KeypadServices()
        {
            // Initialize keypad mapping from KeyMappings class
            KeypadMapping = new Dictionary<int, string>(KeyMappings.KeypadMapping);
        }

        /// <summary>
        /// Processes a full input string and converts it to text.
        /// 
        /// Behavior:
        /// - Processes the entire input string in sequence
        /// - Consecutive same digits represent button presses (e.g., "222" = button 2 pressed 3 times)
        /// - Button 0: Adds a space character to the output
        /// - Character '*': Deletes the last character (backspace)
        /// - Character '#': Stops processing and returns the result (send message)
        /// - Buttons 1-9: Letters only, no numeric output
        /// - Spaces in input (button 0) between button sequences act as separators but don't appear in output
        /// - Spaces allow typing the same letter twice by breaking button sequences
        /// 
        /// Examples:
        /// - "33#" -> "E" (button 3 pressed twice = 'E', then send)
        /// - "222 2 22" -> "CAB" (button 2: 3 presses='C', space separator, 1 press='A', space separator, 2 presses='B')
        /// - "40#" -> "g " (button 4 once = 'g', button 0 = space, then send)
        /// </summary>
        /// <param name="input">The full input string containing button presses (e.g., "33#", "222 2 22#")</param>
        /// <returns>The converted text message as a string</returns>
        public string ConvertText(string input)
        {
            // Validate input
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            List<char> result = new List<char>();
            int i = 0;
            char? lastFinalizedChar = null;

            // Process each character in the input string
            while (i < input.Length)
            {
                char currentChar = input[i];

                // Handle send character (#) - stop processing and return result
                if (currentChar == SEND_CHAR)
                {
                    break;
                }

                // Handle backspace (*)
                if (currentChar == BACKSPACE_CHAR)
                {
                    if (result.Count > 0)
                    {
                        result.RemoveAt(result.Count - 1);
                        // Update last finalized char
                        if (result.Count > 0)
                        {
                            lastFinalizedChar = result[result.Count - 1];
                        }
                        else
                        {
                            lastFinalizedChar = null;
                        }
                    }
                    i++;
                    continue;
                }

                // Handle space character (' ' or '0') - can be separator or actual space
                if (currentChar == ' ' || currentChar == '0')
                {
                    // Count consecutive spaces (both ' ' and '0')
                    int j = i;
                    while (j < input.Length && (input[j] == ' ' || input[j] == '0'))
                    {
                        j++;
                    }

                    // Check if this is a separator space (between button sequences)
                    // Space is a separator if it appears between button sequences
                    // This allows typing same letter twice by breaking button sequences
                    bool isBetweenSequences = (result.Count > 0) && (j < input.Length) && 
                                             (input[j] != SEND_CHAR) && 
                                             (input[j] != BACKSPACE_CHAR) &&
                                             char.IsDigit(input[j]) &&
                                             input[j] != '0';

                    if (isBetweenSequences)
                    {
                        // Space between sequences - acts as separator (doesn't appear in output)
                        // Don't add to result, just skip it
                    }
                    else
                    {
                        // Space not between sequences - add space character to output
                        // This handles cases like "40#" where '0' adds a space
                        result.Add(' ');
                        lastFinalizedChar = ' ';
                    }

                    // Move past all consecutive spaces
                    i = j;
                    continue;
                }

                // Parse button number
                if (!int.TryParse(currentChar.ToString(), out int buttonNumber))
                {
                    // Invalid character, skip it
                    i++;
                    continue;
                }

                // Handle buttons 1-9 (letters only)
                if (buttonNumber >= 1 && buttonNumber <= 9)
                {
                    if (!KeypadMapping.ContainsKey(buttonNumber) || string.IsNullOrEmpty(KeypadMapping[buttonNumber]))
                    {
                        // Button 1 or invalid button, skip it
                        i++;
                        continue;
                    }

                    string characters = KeypadMapping[buttonNumber];
                    
                    // Count consecutive presses of the same button
                    // Consecutive means same digit, but spaces and special chars break the sequence
                    int pressCount = 0;
                    int j = i;
                    
                    while (j < input.Length)
                    {
                        if (input[j] == currentChar && input[j] != SEND_CHAR && input[j] != BACKSPACE_CHAR)
                        {
                            pressCount++;
                            j++;
                        }
                        else if (input[j] == '0' || input[j] == ' ' || input[j] == SEND_CHAR || input[j] == BACKSPACE_CHAR || !char.IsDigit(input[j]))
                        {
                            // Space, special char, or non-digit breaks the sequence
                            break;
                        }
                        else
                        {
                            // Different digit breaks the sequence
                            break;
                        }
                    }

                    // Calculate which character to display based on press count
                    // Press count cycles: 1 = first char, 2 = second char, etc.
                    // If press count exceeds length, it wraps around: (pressCount - 1) % length
                    int charIndex = (pressCount - 1) % characters.Length;
                    char character = char.ToLower(characters[charIndex]);

                    // Add character to result
                    result.Add(character);
                    lastFinalizedChar = character;

                    // Move to next different character or space
                    i = j;
                }
                else
                {
                    i++;
                }
            }

            // Return the converted text
            return new string(result.ToArray());
        }
    }
}
