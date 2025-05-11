namespace ServiceLayer.BookServices.Models;

public record DropdownTuple(string Value, string Text)
{
    public override string ToString() =>
        $"{nameof(Value)}: {Value}, {nameof(Text)}: {Text}";
}
