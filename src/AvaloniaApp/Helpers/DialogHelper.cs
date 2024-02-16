using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using AvaloniaApp.Services;

namespace AvaloniaApp.Helpers;

public static class DialogHelper
{
    /// <summary>
    /// Shows an open file dialog for a registered context, most likely a ViewModel
    /// </summary>
    /// <param name="context">The context</param>
    /// <param name="title">The dialog title or a default is null</param>
    /// <param name="selectMany">Is selecting many files allowed?</param>
    /// <param name="filters">A list of filters, default is all files (FilePickerFileTypes.All)</param>
    /// <param name="suggestedStartLocation">The SuggestedStartLocation, default is WellKnownFolder.Documents </param>
    /// <returns>An array of file names</returns>
    /// <exception cref="ArgumentNullException">if context was null</exception>
    public static async Task<IEnumerable<string>?> OpenFileDialogAsync(this object? context, string? title = null, bool selectMany = true,
        List<FilePickerFileType>? filters = null, string? suggestedStartLocation = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // lookup the TopLevel for the context
        var topLevel = DialogService.GetTopLevelForContext(context);

        if (topLevel != null)
        {
            // Open the file dialog
            var startLocation = await GetStartLocation(suggestedStartLocation, topLevel);
            var storageFiles = await topLevel.StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions()
                {
                    AllowMultiple = selectMany,
                    Title = title ?? "Select any file(s)",
                    FileTypeFilter = filters ?? new List<FilePickerFileType> { FilePickerFileTypes.All },
                    SuggestedStartLocation = startLocation
                });

            // return the result
            return storageFiles.Select(s => s.TryGetLocalPath()!);
        }

        // TODO: should not return null but empty array
        return null;
    }

    public static async Task<IEnumerable<string>?> OpenFolderDialogAsync(this object? context, string? title = null,
        bool selectMany = true, string? initialDirectory = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // lookup the TopLevel for the context
        var topLevel = DialogService.GetTopLevelForContext(context);

        if (topLevel != null)
        {
            // Open the file dialog
            var startLocation = await GetStartLocation(initialDirectory, topLevel);
            var storageFolders = await topLevel.StorageProvider.OpenFolderPickerAsync(
                new FolderPickerOpenOptions()
                {
                    AllowMultiple = selectMany,
                    Title = title ?? "Select any folder(s)",
                    SuggestedStartLocation = startLocation
                });

            // return the result
            return storageFolders.Select(s => s.TryGetLocalPath()!);
        }

        // TODO: should not return null but empty array
        return null;
    }

    private static async Task<IStorageFolder?> GetStartLocation(string? initialDirectory, TopLevel topLevel)
    {
        if (initialDirectory == null) return null;
        if (!Path.Exists(initialDirectory)) return null;

        var uri = new Uri(initialDirectory).AbsoluteUri;
        var folder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(uri);

        return folder;
    }
}

