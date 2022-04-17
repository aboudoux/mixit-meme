namespace MixitMeme.Web.Infrastructure;

public record MemeData(int Id, string Url, string Author)
{
	public static MemeData Empty() => new MemeData(0, string.Empty, string.Empty);
}