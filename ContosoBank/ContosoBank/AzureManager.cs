using Microsoft.WindowsAzure.MobileServices;
using ContosoBank.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace ContosoBank
{
    public class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<TimeTable> timelineTable;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://1811mood.azurewebsites.net");
            this.timelineTable = this.client.GetTable<TimeTable>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task AddTimeline(TimeTable timetable)
        {
            await this.timelineTable.InsertAsync(timetable);
        }

        public async Task<List<TimeTable>> GetTimelines()
        {
            return await this.timelineTable.ToListAsync();
        }

    }
}