using KronoMata.Model;

namespace KronoMata.Web.Models
{
    public class ScheduledJobSaveViewModel : BaseViewModel
    {
        public ScheduledJobSaveViewModel() 
        {
            ScheduledJob = new ScheduledJob()
            {
                StartTime = DateTime.Now
            };
        }

        public string ActionUrl { get; set; } = String.Empty;
        public List<PluginMetaData> Plugins { get; set; } = new List<PluginMetaData>();
        public List<Model.Host> Hosts { get; set; } = new List<Model.Host>();
        public ScheduledJob ScheduledJob { get; set; }

        public class EnumValues
        {
            public EnumValues(string name, int value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; set; }
            public int Value { get; set; }
        }

        public List<EnumValues> Frequencies
        {
            get
            {
                var list = new List<EnumValues>();

                foreach (ScheduleFrequency enumFrequency in Enum.GetValues(typeof(ScheduleFrequency)))
                {
                    list.Add(new EnumValues(enumFrequency.ToString(), (int)enumFrequency));
                }

                return list;
            }
        }
    }
}
