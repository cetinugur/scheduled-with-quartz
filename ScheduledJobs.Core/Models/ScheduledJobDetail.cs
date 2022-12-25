namespace ScheduledJobs.Core.Models
{
    public class ScheduledJobDetail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int JobId { get; set; }
        public bool Active { get; set; }
        /// <summary>
        /// Crontab formatında string olarak tutulur.
        /// Crontab formatı ve örnekleri ile ilgili kaynaklar aşağıdaki gibidir;
        /// http://www.cronmaker.com/?1 (YENİ FORMAT GENERATE ETMEK İÇİN İYİ BİR YARDIMCI)
        /// https://en.wikipedia.org/wiki/Cron#crontab_syntax
        /// https://github.com/atifaziz/NCrontab
        /// http://www.raboof.com/projects/ncrontab/
        /// https://www.gokhanmankara.com/2010/06/crontab-kullanimi-ve-ornek-crontab-uygulamasi/
        /// </summary>
        public string? PeriodAsCron { get; set; }

        public DateTime? LastExecutionTime { get; set; }
    }
}
