namespace BlazorCms.BuildingBlocks.Content.Images;

public class CmsPictureSource
{
    //<source media="(min-width: 465px)" srcset="file-small.jpg" />
    public string? Media { get; set; }
    public string? Srcset { get; set; }
}
