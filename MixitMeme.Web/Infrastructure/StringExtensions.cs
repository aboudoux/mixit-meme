namespace MixitMeme.Web.Infrastructure;

public static class StringExtensions
{
	public static bool IsEmpty(this string source) => string.IsNullOrWhiteSpace(source);
}