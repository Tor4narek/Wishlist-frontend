using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using WishlistApp.ViewModels;

namespace WishlistApp;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            var view = Activator.CreateInstance(type);

            // Проверяем, чтобы представление не было окном
            if (view is Window)
                throw new InvalidOperationException($"View {name} is a Window, expected a UserControl.");

            return (Control)view!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}