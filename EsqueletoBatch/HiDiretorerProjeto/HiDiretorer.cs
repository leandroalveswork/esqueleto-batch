namespace EsqueletoBatch.HiDiretorerProjeto;
public static class HiDiretorer
{
    public static string GetCurrentDirectorySemBinDebug()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var indexOfpastaBinDebug = currentDirectory.IndexOf("\\bin\\Debug");
        var currentDirectorySemBinDebug = indexOfpastaBinDebug != -1 ? currentDirectory.Substring(0, indexOfpastaBinDebug) : currentDirectory;
        return currentDirectorySemBinDebug;
    }
}