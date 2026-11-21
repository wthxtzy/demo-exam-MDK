using System.Drawing;

namespace ChudoObuv.WinForms;

internal static class ImageResources
{
    private static Image? _logo;
    private static Image? _placeholder;

    public static Image? Logo => _logo ??= Load("logo.png");
    public static Image? Placeholder => _placeholder ??= Load("placeholder.png");

    public static Image? LoadProductImage(string imageName)
    {
        if (!string.IsNullOrWhiteSpace(imageName))
        {
            var productImage = Load(imageName);
            if (productImage is not null)
            {
                return productImage;
            }
        }

        return Placeholder;
    }

    private static Image? Load(string fileName)
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Images", fileName);
            return File.Exists(path) ? Image.FromFile(path) : null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
