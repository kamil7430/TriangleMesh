using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TriangleMesh.Models;
using TriangleMesh.Services;

namespace TriangleMesh.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IMessageBoxShower _messageBoxShower;
    private readonly IStorageProvider _storageProvider;

    public MainWindowViewModel(IMessageBoxShower messageBoxShower, IStorageProvider storageProvider)
    {
        _messageBoxShower = messageBoxShower;
        _storageProvider = storageProvider;
    }

    public async void OnMainWindowLoaded()
        => await LoadBezierPolygon("BezierPoints.txt");

    public async Task LoadBezierPolygon(string path)
    {
        try
        {
            using var stream = new StreamReader(path);
            var fileContent = await stream.ReadToEndAsync(); 
            BezierPolygon = new BezierPolygon(fileContent);
            Mesh = new Mesh(TriangulationPrecision, BezierPolygon);
        }
        catch (Exception e)
        {
            await _messageBoxShower.ShowMessageBoxAsync("Błąd",
                $"Podczas ładowania punktów w wielokącie Beziera wystąpił błąd:" +
                $"\n{e.Message}\n" +
                $"Sprawdź poprawność pliku i uruchom aplikację ponownie.");
            Environment.Exit(1);
        }
    }
}