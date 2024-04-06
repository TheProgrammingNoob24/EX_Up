using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private HighScoreView _highScoreView;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ScorePresenter>();
        builder.Register<ScoreModel>(Lifetime.Singleton);
        builder.RegisterComponent(_scoreView);

        builder.RegisterEntryPoint<HighScorePresenter>();
        builder.Register<HighScoreModel>(Lifetime.Singleton);
        builder.RegisterComponent(_highScoreView);
    }
}
