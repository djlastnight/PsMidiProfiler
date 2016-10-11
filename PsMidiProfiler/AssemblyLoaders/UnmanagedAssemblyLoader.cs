namespace PsMidiProfiler.AssemblyLoaders
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public static class UnmanagedAssemblyLoader
    {
        public static string Load(Assembly assembly, string appName, string libraryResourceName, string libraryName)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), appName);
            string tempDllPath = Path.Combine(tempDir, libraryName);

            using (Stream s = assembly.GetManifestResourceStream(libraryResourceName))
            {
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                if (!File.Exists(tempDllPath))
                {
                    byte[] data = new BinaryReader(s).ReadBytes((int)s.Length);
                    File.WriteAllBytes(tempDllPath, data);
                }
            }

            Environment.SetEnvironmentVariable("PATH", tempDir + ";" + tempDllPath);
            UnmanagedAssemblyLoader.LoadLibrary(libraryName);
            return tempDllPath;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
    }
}