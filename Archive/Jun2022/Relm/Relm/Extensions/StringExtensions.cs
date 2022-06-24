namespace Relm.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string input)
        {
            return int.TryParse(input, out _);
        }
    }
}
