using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{

    [SerializeField] InGameLoop _inGameLoop;

    [SerializeField] Card _card_TwoTimes;
    [SerializeField] Card _card_ThreeTimes;
    [SerializeField] Card _card_FiveTimes;
    [SerializeField] Card _card_TenTimes;
    [SerializeField] Card _card_OneHalf;
    [SerializeField] Card _card_OneThird;
    [SerializeField] Card _card_GameOver;




    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_inGameLoop, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_TwoTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_ThreeTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_FiveTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_TenTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_OneHalf, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_OneThird, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_GameOver, Lifetime.Scoped);

        builder.Register<ScorePresenter>(Lifetime.Singleton).AsSelf();
        builder.Register<ScoreModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<ScoreView>().AsSelf();


        /* builder.RegisterEntryPoint<HighScorePresenter>();
         builder.Register<HighScoreModel>(Lifetime.Singleton);
         builder.RegisterComponentInHierarchy<HighScoreView>().AsSelf();*/
    }
}
