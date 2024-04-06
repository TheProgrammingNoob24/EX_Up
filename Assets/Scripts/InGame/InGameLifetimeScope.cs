using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{

    [SerializeField] Card _card_TwoTimes;
    /*[SerializeField] Card _card_ThreeTimes;
    [SerializeField] Card _card_FiveTimes;
    [SerializeField] Card _card_TenTimes;
    [SerializeField] Card _card_OneHalf;
    [SerializeField] Card _card_OneThird;
    [SerializeField] Card _card_GameOver;*/




    protected override void Configure(IContainerBuilder builder)
    {
        
        builder.RegisterComponent(_card_TwoTimes).AsSelf();
       /* builder.RegisterComponent(_card_ThreeTimes).AsSelf();
        builder.RegisterComponent(_card_FiveTimes).AsSelf();
        builder.RegisterComponent(_card_TenTimes).AsSelf();
        builder.RegisterComponent(_card_OneHalf).AsSelf();
        builder.RegisterComponent(_card_OneThird).AsSelf();
        builder.RegisterComponent(_card_GameOver).AsSelf();*/
        

        builder.Register<ScorePresenter>(Lifetime.Singleton).AsSelf();
        builder.Register<ScoreModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<ScoreView>().AsSelf();


        /* builder.RegisterEntryPoint<HighScorePresenter>();
         builder.Register<HighScoreModel>(Lifetime.Singleton);
         builder.RegisterComponentInHierarchy<HighScoreView>().AsSelf();*/
    }
}
