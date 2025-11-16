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
    private bool _wasDebugModeWarningMessageBoxShown = false;
    
    [RelayCommand]
    private async Task ShowDebugModeWarningMessageBox()
    {
        if (_wasDebugModeWarningMessageBoxShown)
            return;

        await _messageBoxShower.ShowMessageBoxAsync("Ostrzeżenie o wydajności", 
            "Uwaga, jeżeli ten program jest uruchomiony w trybie debugowania (za pośrednictem\n" +
            "jakiegoś IDE), możesz doświadczyć niskiej wydajności renderowania wypełnionej siatki trójkątów.\n" +
            "W takiej sytuacji zalecam zbudowanie programu za pomocą komendy \"dotnet run -c Release\"\n" +
            "i uruchomienie wersji release, która powinna działać znacznie wydajniej.");
        _wasDebugModeWarningMessageBoxShown = true;
    }
    
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
                ObjectTexture = WriteableBitmap.Decode(stream);
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
        ObjectTexture = WriteableBitmap.Decode(stream);
    }
}