using System.Diagnostics;

using Config;
using Config.i18n;
using Job.Config;
using Job.Services;

namespace Job.Controller;

public class AddSaveJob
{
    public static (int, string) Execute( string name, string srcPath, string destPath, string type)
    {
        return SaveJobRepo.AddSaveJob(name, srcPath, destPath, type);
    }
}