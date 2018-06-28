using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCrossApp.Core.ViewModels;

namespace MvvmCrossApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();
        }
    }
}