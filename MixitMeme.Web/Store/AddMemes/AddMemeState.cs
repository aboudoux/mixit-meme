using BlazorState;
using MixitMeme.Web.Infrastructure;

namespace MixitMeme.Web.Store.AddMemes;

public class AddMemeState : State<AddMemeState>
{
	public bool DialogVisible { get; set; }

	public bool AuthorInvalid { get; set; }
	public bool UrlInvalid { get; set; }

	public string Url { get; set; }
	public string Author { get; set; }

	public override void Initialize()
	{
		DialogVisible = false;
		AuthorInvalid = false;
		UrlInvalid = false;
		Url = string.Empty;
		Author = string.Empty;
	}

	public record ShowDialog(bool Visible) : IAction;
	public record AddMeme(MemeUrl MemeUrl, MemeAuthor Author) : IAction;
	public record ResetInvalid : IAction;

	
}