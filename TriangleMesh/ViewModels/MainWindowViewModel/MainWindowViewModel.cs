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
        _bezierPolygon = LoadBezierPolygon("BezierPoints.txt");
    }

    private BezierPolygon? LoadBezierPolygon(string path)
    {
        try
        {
            using var stream = new StreamReader(path);
            var fileContent = stream.ReadToEnd();
            return new BezierPolygon(fileContent);
        }
        catch (Exception e)
        {
            _messageBoxShower.ShowMessageBox("Error", e.Message);
        }
        return null;
    }
}