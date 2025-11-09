using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand]
    private async Task LoadTexture()
    {
        await _messageBoxShower.ShowMessageBoxAsync("Message", "Some LoadTexture placeholder");
    }
}