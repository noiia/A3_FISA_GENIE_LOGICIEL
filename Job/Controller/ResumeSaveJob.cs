using Job.Config;
using Job.Services;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Job.Controller;

public class ResumeSaveJob
{
    public static (int, string) Execute(int id)
    {
        return SaveJobRepo.ResumeSaveJob(id);
        // return (0, "");
    }
}