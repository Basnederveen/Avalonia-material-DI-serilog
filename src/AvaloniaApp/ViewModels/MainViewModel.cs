using System.Linq;
using System.Threading.Tasks;
using AvaloniaApp.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace AvaloniaApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private string greeting;

    public MainViewModel(ILogger<MainViewModel> logger)
    {
        logger.LogInformation("MainViewModel created");
    }

    [RelayCommand]
    private async Task OpenFile()
    {
        var files = await this.OpenFileDialogAsync();
        if (files != null)
        {
            this.Greeting = $"You selected {files.Count()} files. First file: {files.First()}";
        }
    }
}
