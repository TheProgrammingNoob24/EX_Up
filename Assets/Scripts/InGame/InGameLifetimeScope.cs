using UnityEngine;
using VContainer;
using VContainer.Unity;

public class InGameLifetimeScope : LifetimeScope
{
   
    protected override void Configure(IContainerBuilder builder)
    {
        
        //builder.RegisterComponentInHierarchyGetComponent<Card>();
        //builder.RegisterComponentInHierarchy<Card>().AsSelf();
        

        builder.Register<ScorePresenter>(Lifetime.Singleton).AsSelf();
        builder.Register<ScoreModel>(Lifetime.Singleton).AsSelf();
        builder.RegisterComponentInHierarchy<ScoreView>().AsSelf();


        /* builder.RegisterEntryPoint<HighScorePresenter>();
         builder.Register<HighScoreModel>(Lifetime.Singleton);
         builder.RegisterComponentInHierarchy<HighScoreView>().AsSelf();*/
    }
}
