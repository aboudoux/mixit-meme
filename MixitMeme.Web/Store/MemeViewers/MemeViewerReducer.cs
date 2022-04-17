using BlazorState;
using MediatR;
using MixitMeme.Web.Infrastructure;

namespace MixitMeme.Web.Store.MemeViewers;

public class MemeViewerReducer : 
	ActionHandler<MemeViewerState.LoadLastMeme>,
	IRequestHandler<MemeViewerState.Next>,
	IRequestHandler<MemeViewerState.Previous>
{
	private readonly IMemeRepository _memeRepository;
	private MemeViewerState State => Store.GetState<MemeViewerState>();
	public MemeViewerReducer(IStore store, IMemeRepository memeRepository) : base(store)
	{
		_memeRepository = memeRepository ?? throw new ArgumentNullException(nameof(memeRepository));
	}

	public override Task<Unit> Handle(MemeViewerState.LoadLastMeme action, CancellationToken aCancellationToken)
	{
		if (!_memeRepository.All().Any()) return Unit.Task;
		State.Current = _memeRepository.All().OrderByDescending(a => a.Id).First();
		return Unit.Task;
	}

	public Task<Unit> Handle(MemeViewerState.Next action, CancellationToken cancellationToken)
	{
		if (!_memeRepository.All().Any()) return Unit.Task;
		State.Current = _memeRepository.All().FirstOrDefault(a => a.Id == State.Current.Id + 1) ??
		           _memeRepository.All().First();
		return Unit.Task;
	}

	public Task<Unit> Handle(MemeViewerState.Previous request, CancellationToken cancellationToken)
	{
		if (!_memeRepository.All().Any()) return Unit.Task;
		State.Current = _memeRepository.All().FirstOrDefault(a => a.Id == State.Current.Id - 1) ??
		                _memeRepository.All().Last();
		return Unit.Task;
	}
}

public class MemeViewerState : State<MemeViewerState>
{
	public MemeData Current { get; set; }

	public override void Initialize()
	{
		Current = MemeData.Empty();
	}

	public record LoadLastMeme : IAction;

	public record Next : IAction;
	public record Previous : IAction;
}