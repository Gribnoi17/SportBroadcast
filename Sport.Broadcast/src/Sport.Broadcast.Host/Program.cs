namespace Sport.Broadcast.Host
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(app => { app.UseStartup<Startup>(); })
                .Build()
                .RunAsync();
        }
    }
}