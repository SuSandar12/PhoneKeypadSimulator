using System.Collections.Generic;

namespace PhoneKeypadSimulator.Models
{
    public class KeyMappings
    {
        public static Dictionary<int, string> KeypadMapping { get; } = new Dictionary<int, string>
        {
            { 0, " " },
            { 1, "" },
            { 2, "ABC" },
            { 3, "DEF" },
            { 4, "GHI" },
            { 5, "JKL" },
            { 6, "MNO" },
            { 7, "PQRS" },
            { 8, "TUV" },
            { 9, "WXYZ" }
        };

        public static char? GetCharacter(int buttonNumber, int pressCount)
        {
            if (!KeypadMapping.ContainsKey(buttonNumber))
                return null;

            string characters = KeypadMapping[buttonNumber];
            
            if (string.IsNullOrEmpty(characters))
                return null;

            int index = (pressCount - 1) % characters.Length;
            return characters[index];
        }

        public static string? GetCharacters(int buttonNumber)
        {
            return KeypadMapping.ContainsKey(buttonNumber) 
                ? KeypadMapping[buttonNumber] 
                : null;
        }
    }
}
