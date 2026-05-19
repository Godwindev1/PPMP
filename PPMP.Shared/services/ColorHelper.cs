using System.Drawing;

public static class ColorHelper
{
    public static string LightenColor(string hex, double percent)
    {
        var color = ColorTranslator.FromHtml(hex);
        int r = (int)(color.R + (255 - color.R) * percent);
        int g = (int)(color.G + (255 - color.G) * percent);
        int b = (int)(color.B + (255 - color.B) * percent);
        return $"#{r:X2}{g:X2}{b:X2}";
    }

    public static string DarkenColor(string hex, double percent)
    {
        var color = ColorTranslator.FromHtml(hex);
        int r = (int)(color.R * (1 - percent));
        int g = (int)(color.G * (1 - percent));
        int b = (int)(color.B * (1 - percent));
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}
