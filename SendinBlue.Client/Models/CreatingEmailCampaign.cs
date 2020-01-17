using System;
using System.Collections.Generic;
using System.Text;

namespace SendinBlue.Client.Models
{
    public class CreatingEmailCampaign
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public int TemplateId { get; set; }
        public string Subject { get; set; }
        public string ReplyTo { get; set; }

    }
}
