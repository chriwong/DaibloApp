using System;

namespace DiabloApp.Models
{
    public class FlyoutMenuItem
    {
        public FlyoutMenuItem()
        {
            this.TargetType = typeof(FlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
