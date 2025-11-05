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
            _messageBoxShower.ShowMessageBox("B³¹d",
                $"Podczas ³adowania punktów w wielok¹cie Beziera wyst¹pi³ b³¹d:\n{e.Message}");
        }
        return null;
    }
}