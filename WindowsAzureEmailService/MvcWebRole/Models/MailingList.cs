using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcWebRole.Models
{
    public class MailingList : TableEntity
    {
        public MailingList()
        {
            this.RowKey = "MailingList";
            this.PartitionKey = this.ListName;
        }

        [Required]
        [RegularExpression(@"[\w]+", ErrorMessage = @"Only alphanumeric characters and underscore (_) are allowed.")]
        [Display(Name="Mailing List Name")]
        public string ListName 
        { 
            get
            {
                return this.PartitionKey;
            }
            set 
            {
                this.PartitionKey = value;
            }
        }

        [Display(Name="From Email Address")]
        public string FromEmailAddress { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}