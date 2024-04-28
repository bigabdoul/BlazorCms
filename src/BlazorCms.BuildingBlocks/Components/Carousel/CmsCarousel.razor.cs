using BlazorCms.BuildingBlocks.Abstractions;
using static BlazorCms.BuildingBlocks.BlockComponentBase;

namespace BlazorCms.BuildingBlocks.Components.Carousel;

public class CarouselChildItem
{
    public CarouselCaption? Caption { get; set; }
    public CarouselImage? Image { get; set; }
    public int Interval { get; set; }
    public bool HasImage => !string.IsNullOrWhiteSpace(Image?.Src);
    public bool HasCaption => !string.IsNullOrWhiteSpace(Caption?.HeaderText) || !string.IsNullOrWhiteSpace(Caption?.BodyText);

    /// <summary>
    /// Generates random image links from the https://unsplash.it website by default.
    /// </summary>
    /// <param name="coverPicture">The first, default image to add.</param>
    /// <param name="minCount">The minimum number of images to generate.</param>
    /// <param name="maxCount">The maximum number of images to generate.</param>
    /// <param name="maxImageIndex">The highest index of the images to generate.</param>
    /// <param name="smallWidth">The width of the small image source.</param>
    /// <param name="smallHeight">The height of the small image source.</param>
    /// <param name="fullWidth">The width of the full image source.</param>
    /// <param name="fullHeight">The height of the full image source.</param>
    /// <param name="altPrefix">The prefix for the alternative image text.</param>
    /// <param name="smallSourceGetter">The small image source getter function.</param>
    /// <param name="fullSourceGetter">The full image source getter function.</param>
    /// <returns></returns>
    public static CarouselChildItem[] GenerateRandomImages(string? coverPicture = null,
        ushort minCount = 1,
        ushort maxCount = 3,
        ushort maxImageIndex = 1000,
        ushort smallWidth = 1349,
        ushort smallHeight = 450,
        ushort fullWidth = 1400,
        ushort fullHeight = 768,
        string? altPrefix = "Slide",
        Func<int, string?>? smallSourceGetter = null,
        Func<int, string?>? fullSourceGetter = null)
    {
        List<CarouselChildItem> images = [];

        // make sure the numbers are what they should be
        // relative to each other (i.e., minCount <= maxCount)
        (minCount, maxCount) = minCount > maxCount ? (maxCount, minCount) : (minCount, maxCount);
        var count = Random.Shared.Next(minCount, maxCount + 1);

        if (IsNotBlank(coverPicture))
        {
            images.Add(new() { Image = new() { Src = coverPicture, Alt = "Cover Picture" } });
        }

        for (var i = 0; maxImageIndex > 0 && i < count; i++)
        {
            var index = Random.Shared.Next(1, maxImageIndex);
            var src = smallSourceGetter?.Invoke(index) ?? $"https://unsplash.it/{smallWidth}/{smallHeight}.jpg?image={index}";
            var srcFull = fullSourceGetter?.Invoke(index) ?? $"https://unsplash.it/{fullWidth}/{fullHeight}.jpg?image={index}";

            // make sure at least one source is set
            if (Coalesce(IsNotBlank, src, srcFull))
            {
                images.Add(new()
                {
                    Image = new()
                    {
                        Src = src,
                        SrcFull = srcFull,
                        Alt = $"{altPrefix} {i + 1}"
                    }
                });
            }
        }

        return [.. images];
    }
}

public class CarouselImage
{
    public string? Src { get; set; }
    public string? SrcFull { get; set; }
    public string? Alt { get; set; }
    public string? Style { get; set; }
    public string? Link { get; set; }
    public string LinkTarget { get; set; } = "_blank";
}

public class CarouselCaption
{
    public string? HeaderText { get; set; }
    public string? BodyText { get; set; }
}

public enum CarouselPause
{
    None,
    MouseEnter,
    MouseLeave,
}

public class CarouselConfiguration : ComponentConfiguration<CmsCarousel>
{
}
