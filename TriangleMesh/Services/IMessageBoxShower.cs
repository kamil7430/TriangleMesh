using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;

namespace TriangleMesh.Services;

public interface IMessageBoxShower
{
    async Task ShowMessageBoxAsync(string title, string content)
    {
        var messageBox = MessageBoxManager.GetMessageBoxStandard(title, content);
        await messageBox.ShowWindowDialogAsync((Window)this);
    }
}
