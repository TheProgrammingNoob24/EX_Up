using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{

    [SerializeField] UIAnimation _uiAnimation;
    [SerializeField] CardBehaviourSummary _cardBehaviourSummary;
    [SerializeField] Card _card_TwoTimes;
    [SerializeField] Card _card_ThreeTimes;
    [SerializeField] Card _card_FiveTimes;
    [SerializeField] Card _card_TenTimes;
    [SerializeField] Card _card_OneHalf;
    [SerializeField] Card _card_OneThird;
    [SerializeField] Card _card_GameOver;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(_uiAnimation, Lifetime.Singleton);
        builder.RegisterComponentInNewPrefab(_card_TwoTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_ThreeTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_FiveTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_TenTimes, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_OneHalf, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_OneThird, Lifetime.Scoped);
        builder.RegisterComponentInNewPrefab(_card_GameOver, Lifetime.Scoped);

        builder.RegisterEntryPoint<InGameLoop>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<CardBehaviourSummary>().AsSelf();


        builder.Register<ScorePresenter>(Lifetime.Singleton).AsSelf();
        builder.Register<ScoreModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<ScoreView>().AsSelf();

        builder.Register<VerticalCutInPresenter>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<VerticalCutInView>().AsSelf();

    }
}
