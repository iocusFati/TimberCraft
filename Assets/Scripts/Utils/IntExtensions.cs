namespace Utils
{
    public static class IntExtensions
    {
        public static string TransformThousands(this int number)
        {
            if (number >= 1000)
            {
                double thousands = (double)number / 1000;
                return thousands.ToString("0.0") + "K";
            }
            else
            {
                return number.ToString();
            }
        }
    }
}