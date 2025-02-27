using Job.Config;
using Job.Services;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Job.Controller;

public class ResumeSaveJob
{
    public async static Task<(int, string)> Execute(int id)
    {
        
        SaveJobRepo.ResumeSaveJob(id);
        return (0, "");
    }
}