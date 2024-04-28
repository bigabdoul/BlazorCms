using BlazorCms.BuildingBlocks;
using Microsoft.AspNetCore.Components;

namespace BlazorCms.Components.Pages;

public class BlogPostBase : BlockComponentBase
{
    [Parameter] public string? Category { get; set; }
    [Parameter] public string? Slug { get; set; }
}
