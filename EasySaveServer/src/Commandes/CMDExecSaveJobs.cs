using Job.Config;
using Job.Controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMDExecSaveJobs : CMD
{
    private const string STOP = "STOP";
    private const string WARNING = "WARNING";
    private const string ERROR = "ERROR";
    private const string LOCK = "LOCK";
    private const string PAUSE = "PAUSE";
    private const string RUNNING = "RUNNING";
    private readonly Configuration config = ConfigSingleton.Instance();
    private List<int> _Ids;

    private DateTime _lastNotificationDate;

    public CMDExecSaveJobs(List<int> ids) : base("ExecSaveJobs")
    {
        _Ids = ids;
    }

    public List<int> Ids
    {
        get => _Ids;
        set => _Ids = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string toString()
    {
        var json = new JObject();
        json.Add("commande", commande);
        json["ids"] = JToken.FromObject(_Ids);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public override async Task run(MessageList messageList)
    {
        try
        {
            Console.WriteLine("CMDExecSaveJobs.run");
            //ExecSaveJob
            //UpdateStatus(ids,RUNNING);
            Console.WriteLine(Ids + RUNNING);
            var executionTracker = new ExecutionTracker();
            executionTracker.OnTrackerChanged += UpdateLastExecDate;
            var lockTracker = new LockTracker();
            lockTracker.OnTrackerChanged += UpdateStatusOnLock;
            var separator = Ids.Count >= 1 ? ";" : "";
            // var (returnCode, message) = await ExecuteSaveJob.Execute(Ids, separator, executionTracker, lockTracker);
            await ExecuteSaveJob.Execute(Ids, separator, executionTracker, lockTracker);

            // Send data
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    private void UpdateLastExecDate(object sender, TrackerChangedEventArgs eventArgs)
    {
        try
        {
            List<SaveJob> saveJobs = config.GetSaveJobs().ToList();

            var sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
            if (sj != null)
            {
                saveJobs.Remove(sj);
                sj.LastSave = eventArgs.Timestamp;
                switch (eventArgs.ReturnCode)
                {
                    case 1:
                    {
                        sj.Status = STOP;
                        break;
                    }
                    case 2:
                    {
                        sj.Status = WARNING;
                        break;
                    }
                    case 3:
                    {
                        sj.Status = ERROR;
                        break;
                    }
                    case 4:
                    {
                        sj.Status = LOCK;
                        break;
                    }
                }

                saveJobs.Add(sj);
            }

            config.SetSaveJobs(saveJobs.ToArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void UpdateStatusOnLock(object sender, TrackerLockEventArgs eventArgs)
    {
        try
        {
            List<SaveJob> saveJobs = config.GetSaveJobs().ToList();

            var sj = saveJobs.FirstOrDefault(i => i.Id == eventArgs.Id);
            if (sj != null)
            {
                saveJobs.Remove(sj);
                switch (eventArgs.Status)
                {
                    case 0:
                    {
                        sj.Status = RUNNING;
                        break;
                    }
                    case 4:
                    {
                        sj.Status = LOCK;
                        if ((DateTime.Now - _lastNotificationDate).TotalSeconds >= 7)
                            // Notify save is locked by a software
                            //NotificationMessageManagerSingleton.GenerateNotification(this.Manager, 4, $"{Translation.Translator.GetString("SaveJobLockBy")} {eventArgs.BusinessAppName}");
                            _lastNotificationDate = DateTime.Now;
                        break;
                    }
                }

                saveJobs.Add(sj);
            }

            config.SetSaveJobs(saveJobs.ToArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}