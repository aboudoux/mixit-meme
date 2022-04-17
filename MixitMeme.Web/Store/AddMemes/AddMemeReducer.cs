using BlazorState;
using MediatR;
using MixitMeme.Web.Infrastructure;
using MixitMeme.Web.Store.MemeViewers;

namespace MixitMeme.Web.Store.AddMemes;

public class AddMemeReducer : 
	ActionHandler<AddMemeState.ShowDialog>,
	IRequestHandler<AddMemeState.AddMeme>,
	IRequestHandler<AddMemeState.ResetInvalid>
{
	private readonly IMemeRepository _memeRepository;
	private readonly IMediator _uiBus;
	private readonly IMemeChecker _memeChecker;
	private AddMemeState State => Store.GetState<AddMemeState>();
	public AddMemeReducer(IStore store, IMemeRepository memeRepository, IMediator uiBus, IMemeChecker memeChecker) : base(store)
	{
		_memeRepository = memeRepository ?? throw new ArgumentNullException(nameof(memeRepository));
		_uiBus = uiBus ?? throw new ArgumentNullException(nameof(uiBus));
		_memeChecker = memeChecker ?? throw new ArgumentNullException(nameof(memeChecker));
	}

	public override Task<Unit> Handle(AddMemeState.ShowDialog action, CancellationToken aCancellationToken)
	{
		State.Initialize();
		State.DialogVisible = action.Visible;
		return Unit.Task;
	}

	public async Task<Unit> Handle(AddMemeState.AddMeme action, CancellationToken cancellationToken)
	{
		var urlExists = await _memeChecker.Exists(action.MemeUrl);
		if (action.MemeUrl.Value.IsEmpty() || !urlExists) {
			State.UrlInvalid = true;
			return Unit.Value;
		}

		if (action.Author.Value.IsEmpty())
		{
			State.AuthorInvalid = true;
			return Unit.Value;
		}

		_memeRepository.Add(action.MemeUrl, action.Author);
		await _uiBus.Send(new AddMemeState.ShowDialog(false));
		await _uiBus.Send(new MemeViewerState.LoadLastMeme());
		return Unit.Value;
	}

	public Task<Unit> Handle(AddMemeState.ResetInvalid request, CancellationToken cancellationToken)
	{
		State.AuthorInvalid = false;
		State.UrlInvalid = false;
		return Unit.Task;
	}
}