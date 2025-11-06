using System;
using System.IO;
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

    public void OnMainWindowLoaded()
        => BezierPolygon = LoadBezierPolygon("BezierPoints.txt");

    public BezierPolygon? LoadBezierPolygon(string path)
    {
        try
        {
            using var stream = new StreamReader(path);
            var fileContent = stream.ReadToEnd();
            return new BezierPolygon(fileContent);
        }
        catch (Exception e)
        {
            _messageBoxShower.ShowMessageBox("Błąd",
                $"Podczas ładowania punktów w wielokącie Beziera wystąpił błąd:" +
                $"\n{e.Message}\n" +
                $"Sprawdź poprawność pliku i uruchom aplikację ponownie.");
        }
        return null;
    }
}