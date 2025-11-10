using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel
{
    [RelayCommand]
    private async Task LoadTexture()
    {
        var filePickerOpenOptions = new FilePickerOpenOptions
        {
            Title = "Wybierz plik obrazu",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        };
        
        var files = await _storageProvider.OpenFilePickerAsync(filePickerOpenOptions);

        if (files.Any())
        {
            var file = files[0];
            try
            {
                await using var stream = await file.OpenReadAsync();
                ObjectTexture = new Bitmap(stream);
            }
            catch (Exception e)
            {
                await _messageBoxShower.ShowMessageBoxAsync("Błąd otwierania pliku",
                    $"Podczas otwierania pliku wystąpił błąd:\n{e.Message}");
            }
        }
    }

    [RelayCommand]
    private void RestoreDefaultTexture()
    {
        var uri = new Uri("avares://TriangleMesh/Assets/golden-retriever.jpg");
        using var stream = AssetLoader.Open(uri);
        ObjectTexture = new Bitmap(stream);
    }
}