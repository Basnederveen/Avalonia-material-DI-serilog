using AvaloniaApp.ViewModels;

namespace AvaloniaApp;

internal class DesignData
{
    public static MainViewModel MainViewModel =>
        new MainViewModel(new Microsoft.Extensions.Logging.Abstractions.NullLogger<MainViewModel>())
        {
            // set any properties here
            Greeting = "Hello Design Time!"
        };
    
}

