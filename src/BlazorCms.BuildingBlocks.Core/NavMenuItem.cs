namespace BlazorCms.BuildingBlocks;

public class NavMenuItem
{
    public string Id { get; set; } = $"nbi_{Guid.NewGuid().GetHashCode():x}";
    public string Url { get; set; } = "#";
    public string? Text { get; set; }
    public string? Title { get; set; }
    public string? Icon { get; set; }
    public string? Token { get; set; }
    public string? ReturnUrl { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsActive { get; set; }
    public bool IsDisabled { get; set; }
    public bool IsSignOut { get; set; }
    public bool IsSeparator { get; set; }
    public bool IsDarkTheme { get; set; }
    public AuthVisibility Visibility { get; set; }
	public ImageProperties? Image { get; set; }

	/// <summary>
	/// Gets or sets the child items.
	/// </summary>
	public IReadOnlyCollection<NavMenuItem>? DropdownItems { get; set; }
    public bool DropdownMenuEnd { get; set; }
    public bool HasDropdownItems => DropdownItems?.Count > 0;
    public bool CenterText { get; set; }
}

public enum AuthVisibility
{
    Any,
    Authorized,
    NotAuthorized,
}

/// <summary>
/// Encapsulates data related to an image.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ImageProperties"/> class.
/// </remarks>
/// <param name="src">The source attribute value for image tag.</param>
/// <param name="width">The width attribute value for image tag.</param>
/// <param name="height">The height attribute value for image tag.</param>
public class ImageProperties(string src, int? width = null, int? height = null)
{
	/// <summary>
	/// Gets or sets the source attribute of the image.
	/// </summary>
	public string Src { get; set; } = src;

	/// <summary>
	/// Gets or sets the width attribute of the image.
	/// </summary>
	public int? Width { get; set; } = width;

	/// <summary>
	/// Gets or sets the height attribute of the image.
	/// </summary>
	public int? Height { get; set; } = height;

	/// <summary>
	/// Gets or sets the alt attribute of the image.
	/// </summary>
	public string? Alt { get; set; }

	/// <summary>
	/// Gets or sets the class attribute of the image.
	/// </summary>
	public string? CssClass { get; set; }

	/// <summary>
	/// Gets or sets the style attribute of the image.
	/// </summary>
	public string? Style { get; set; }
}

public class NavbarOptions
{
	public NavbarBrandModel? Brand { get; set; }
	public string AccountUrl { get; set; } = "authentication";
	public string? ProfileAction { get; set; } = "profile";
	public string? SignOutAction { get; set; } = "logout";
	public string? AntiforgeryToken { get; set; }
	public string? UserName { get; set; }
	public string? ProfilePicture { get; set; }
	public IReadOnlyCollection<NavMenuItem>? NavbarItems { get; set; }
	public string MenuId { get; set; } = $"mainNav_{Guid.NewGuid().GetHashCode():x}";
	public string CollapseId { get; set; } = $"mainNavCollapse_{Guid.NewGuid().GetHashCode():x}";
	public string TogglerLabel { get; set; } = "Toggle navigation";
	public bool IsDarkTheme { get; set; }
}

public class NavbarBrandModel
{
	public string? Name { get; set; }
	public string Url { get; set; } = "/";
	public ImageProperties? Image { get; set; }
}

public class LightboxOptions
{
    public string? Src { get; set; }
    public string? Alt { get; set; }
    public string? GalleryName { get; set; }
    public string? Title { get; set; }
}