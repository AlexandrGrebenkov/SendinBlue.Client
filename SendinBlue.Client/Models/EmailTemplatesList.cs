using System;
using System.Collections.Generic;
using System.Text;

namespace SendinBlue.Client.Models
{
    public class EmailTemplatesList
    {
        public int Count { get; set; }

        public IEnumerable<EmailTemplateSummary> Templates { get; set; }
    }
}
