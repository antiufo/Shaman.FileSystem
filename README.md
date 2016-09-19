# Shaman.FileSystem

## FileSystem
```csharp
using Shaman.Runtime;

// Tries to delete a file. If it fails (file in use), the file is instead renamed.
FileSystem.DeleteOrMoveFile("C:\\Path\\File.dat");

// Returns Example.txt, Example (2).txt and so on
FileSystem.GetUniqueFileName("C:\\Path", "Example.txt");

// Returns "C:\\Path\\Potentially very long name or with foridden-- chars.jpg" (truncated if above Win32 limits)
FileSystem.SanitizeFileName("C:\\Path", "Potentially very long name or with foridden*/ chars.jpg");
```
## MaskedFile
Provides transaction-like features, so that a file never contains incomplete data if the process terminates abruptly.
```csharp

string destination = "C:\\Path\\File.jpg";

string m = MaskedFile.GetMaskedPathFromFile(destination);
// m == "C:\\Path\\$9bf2aa8f-4e94-4e58-b72f-e1f9a9bdb79e.tmp"

using (var stream = File.OpenWrite(m))
{
    m.Write(/*…*/);
}

MaskedFile.PublishMaskedFile(m, destination); // Commits the file

