##  Links
- More about scripts **[READMI.md](Scripts/README.md)**.
- More about back-end configuration **[READMI.md](Back-end/README.md)**.
- Useful information about hosting: https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-5.0&tabs=visual-studio
- IIS web-site configuration: 
  - https://medium.com/@rakesh.akalwadi/hosting-net-application-in-azure-vm-using-iis-b266bdd78bc1
  - https://dev.to/dotvvm/deploying-asp-net-core-and-dotvvm-web-applications-to-a-virtual-machine-in-azure-230f
- Library to work with schedule: https://github.com/dahall/taskscheduler

##  Technology Stack
**Back-end**: ASP.NET Core 3.1 (C#), Selenium Web Driver (Python)

**Database**: MS SQL, **ORM**: Entity Framework.

**UI**: Angular 10, Node JS 12.11.1

**Hosting Platform**: Windows Server 2016+, IIS

**Script Schedule System**: built-in Windows service, Task Scheduler

## General Flow

### Authorization & Session
On each request from Server to Web Driver, service check the auth info.
IF the system has authrozied session - use it, if no - run the script of authorizing in Instagram, saving that session and continue the logic using this session.

### Scenarios
There are  2 types of script:
- PostChecker
- AccountsMonitoring

Let`s review both of them, but first note about preconditions.

Preconditions: 
- accounts has already added to db;
- schedule created;

Schedule runs the executalbe file **(WeddingForward.ScheduleTaskExecutor)** which contains the logic about scenario.

Information about scripts which are should be runs reads from the **ScriptsSchedule** table,
**ordered by PlanedStart** date & time, and **filtered by field IsStarted**

Service selects the first record which is suitable for requirements above, on the update it in db as **IsStarted=true**, after successful finish update as **IsFinished=true**
 
####  Post`s info snapshot (PostChecker)
Preconditions:
- initial post info exists in db;

High-level Flow:
- Service retirieview information about post from the Instagram:
  - gets available active user-session or create new one
  - runs the script which retrieving information about specified post:
    - navigates to page with specified post by url;
    - parse json with the data to specified format;
    - returns converted data from the script;
- Service reads data from script;
- Creates post-check-details record in db;

####  Looking for new posts (AccountsMonitoring)

Service select all the existing account in db and do the following steps with each of them:
- retirieview information about account from the Instagram:
  - gets available active user-session or create new one
  - runs the script which retrieving information about specified account:
    - navigates to page with specified account by url;
    - parse json with the data to specified format;
    - returns converted data from the script;
  - reads data from script;
  - if the posts count the same as was on account - exit from script;
  - if the value of posts count is different then:
    - gets first post of account from Instagram (parsing account-info page json):
    - check if this post exists in db;
    - if post already exists - exit;
    - if post is new:
      - retirieview information about post from the Instagram;
      - store post info to db;
      - add to schedule 3-checks for the post
        - now
        - in 1 hour
        - in 1 day
      
### Work with Console
All the next operation you can do from the Visual Studio, project - **WeddingForward.Scheduler.Console**.

#### Add Accounts to db

```C#
...

IAccountsService accountsService = ServiceProvider.GetService<IAccountsService>();
IDataRequestDispatcher dataRequestDispatcher = ServiceProvider.GetService<IDataRequestDispatcher>();

Task<ServiceResult<Account>>[] tasks = new[]
{
    //mariyazakir
    //theweddingbliss
    //justweddingbells
    //lovinghautecouture
    //weddingdressesguide

    accountsService.SearchForAccount("weddingdressesguide"),
    accountsService.SearchForAccount("lovinghautecouture"),
    accountsService.SearchForAccount("justweddingbells"),
    accountsService.SearchForAccount("theweddingbliss"),
    accountsService.SearchForAccount("mariyazakir")
};

ServiceResult<Account>[] serviceResults = await Task.WhenAll(tasks).ConfigureAwait(false);

foreach (ServiceResult<Account> serviceResult in serviceResults)
{
    if (serviceResult.ResultType == ServiceResultType.Error)
    {
        System.Console.WriteLine(JsonConvert.SerializeObject(serviceResult));
        continue;
    }

    Account account = serviceResult.Result;
    if (account == null)
    {
        System.Console.WriteLine("account was not found");
        continue;
    }

    account.PostsCount -= 1;

    var storeAccountInfoCommand = new StoreAccountInfoCommand(account);

    ServiceResult<bool> result = await dataRequestDispatcher
        .ExecuteAsync<StoreAccountInfoCommand, bool>(storeAccountInfoCommand)
        .ConfigureAwait(false);

    if (result.ResultType == ServiceResultType.Error)
    {
        System.Console.WriteLine(JsonConvert.SerializeObject(serviceResult));
    }
}

...
```

#### Add Accounts Monitoring schedule to Windows Task Schedule Service

```C#
...

WeddingForwardContext db = ServiceProvider.GetService<WeddingForwardContext>();

db.ScriptsSchedule.Add(new ScriptsScheduleSet
{
    Id = Guid.NewGuid(),
    IsStarted = false,
    IsFinished = false,
    ScriptType = "AccountsMonitoring",
    Args = null,
    PlanedStart = DateTime.UtcNow
});

await db.SaveChangesAsync().ConfigureAwait(false);

TaskDefinition newTask = TaskService.Instance.NewTask();
newTask.Settings.Enabled = true;
newTask.Settings.RunOnlyIfNetworkAvailable = true;

var timeTrigger = new TimeTrigger
{
    Enabled = true,
    StartBoundary = DateTime.Now.AddMinutes(2)
};

newTask.Triggers.Add(timeTrigger);

newTask.Actions.Add(new ExecAction
{
    Path = @"C:\Test\Test\WeddingForward.ScheduleTaskExecutor.exe", // path to executable file of WeddingForward.ScheduleTaskExecutor library
    WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
});

try
{
    Microsoft.Win32.TaskScheduler.Task registerTaskDefinition =
        TasksFolder.RegisterTaskDefinition(
            $"TriggerExecutor", newTask);
}
catch (Exception e)
{
    System.Console.WriteLine(JsonConvert.SerializeObject(e));
}

...
```

#### Show checked posts results, grouping by account

```C#
...

WeddingForwardContext dbContext = ServiceProvider.GetService<WeddingForwardContext>();

List<PostCheckerHistorySet> postCheckerHistorySets = dbContext.PostCheckerHistory.Include(set => set.PostCheckDetails).ToList();

System.Console.WriteLine();
System.Console.WriteLine();

foreach (IGrouping<string, PostCheckerHistorySet> historySets in postCheckerHistorySets.GroupBy(set => set.AccountId))
{
    var stringBuilder = new StringBuilder();

    stringBuilder.AppendLine($"Account: {historySets.Key}");

    foreach (IGrouping<string, PostCheckerHistorySet> checkerHistorySets in historySets.GroupBy(set => set.PostId))
    {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"Post: https://www.instagram.com/p/{checkerHistorySets.Key}/");

        foreach (PostCheckerHistorySet postCheckerHistorySet in checkerHistorySets)
        {
            stringBuilder.AppendLine(
                $"\t Likes: {postCheckerHistorySet.PostCheckDetails.LikesCount}\t Comments: {postCheckerHistorySet.PostCheckDetails.CommentsCount}\t Checked At: {postCheckerHistorySet.CheckedAt}");
        }

        stringBuilder.AppendLine();
    }

    System.Console.WriteLine(stringBuilder.ToString());
}

System.Console.ReadKey();

...
```
