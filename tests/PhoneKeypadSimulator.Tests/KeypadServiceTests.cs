using Xunit;
using PhoneKeypadSimulator.Services;
namespace PhoneKeypadSimulator.Tests;

public class KeypadServiceTests
{
    private readonly KeypadServices _keypadService;

    public KeypadServiceTests()
    {
        _keypadService = new KeypadServices();
    }

    [Fact]
    public void ConvertText_ReturnExpectedLetter_ValidKey()
    {
        var result = _keypadService.ConvertText("2#").ToUpper();
        Assert.Equal("A", result);
    }

    [Fact]
    public void ConvertText_ReturnExpectedLetter_MultipleValidKeys()
    {
        var result = _keypadService.ConvertText("4433555 555666#").ToUpper();
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void ConvertText_ReturnEmptyString_NoValidKeys()
    {
        var result = _keypadService.ConvertText("1111#");
        Assert.Equal(string.Empty, result);
    }   

    [Fact]
    public void ConvertText_ReturnExpectedLetter_WithBackspace()
    {
        var result = _keypadService.ConvertText("4433*555 555666#").ToUpper();
        Assert.Equal("HLLO", result);
    }

    [Fact]
    public void ConvertText_ReturnExpectedLetter_ComplexInput()
    {
        var result = _keypadService.ConvertText("4427 7999 2226663444664#").ToUpper();
        Assert.Equal("HAPPY CODING", result);
    }

    [Fact]
    public void ConvertText_ReturnExpectedLetter_ComplexInput2()
    {
        var result = _keypadService.ConvertText("8 88777444666*664#").ToUpper();
        Assert.Equal("TURING", result);
    }

}
