using System;
using System.IO;
using System.Threading.Tasks;
using TriangleMesh.Models;
using TriangleMesh.Services;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMessageBoxShower _messageBoxShower;

    public MainWindowViewModel(IMessageBoxShower messageBoxShower)
    {
        _messageBoxShower = messageBoxShower;
    }

    public async void OnMainWindowLoaded()
        => BezierPolygon = await LoadBezierPolygon("BezierPoints.txt");

    public async Task<BezierPolygon> LoadBezierPolygon(string path)
    {
        try
        {
            using var stream = new StreamReader(path);
            var fileContent = await stream.ReadToEndAsync();
            return new BezierPolygon(fileContent);
        }
        catch (Exception e)
        {
            await _messageBoxShower.ShowMessageBoxAsync("Błąd",
                $"Podczas ładowania punktów w wielokącie Beziera wystąpił błąd:" +
                $"\n{e.Message}\n" +
                $"Sprawdź poprawność pliku i uruchom aplikację ponownie.");
            Environment.Exit(1);
        }
        return null!;
    }
}