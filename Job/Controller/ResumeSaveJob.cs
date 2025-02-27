using Job.Services;

namespace Job.Controller;

public class ResumeSaveJob
{
    public static (int, string) Execute(int id)
    {
        return SaveJobRepo.ResumeSaveJob(id);
        // return (0, "");
    }
}