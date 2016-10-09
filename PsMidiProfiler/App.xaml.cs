using PsMidiProfiler.AssemblyLoaders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PsMidiProfiler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += this.OnAssemblyResolve;
            this.LoadAssemblies();
        }


        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return ManagedAssemblyLoader.Get(args.Name);
        }

        private void LoadAssemblies()
        {
            UnmanagedAssemblyLoader.Load(
                Assembly.GetExecutingAssembly(),
                "PsMidiProfiler",
                "PsMidiProfiler.Lib.bass.dll",
                "bass.dll");

            ManagedAssemblyLoader.Load("PsMidiProfiler.Lib.Bass.Net.dll", "Bass.Net.dll");
        }
    }
}
