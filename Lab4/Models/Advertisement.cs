using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Advertisement
    {
        public int Id
        {
            set;
            get;
        }

        public string CommunityID
        {
            set;
            get;
        }

        [Required]
        [DisplayName("File Name")]
        public string FileName
        {
            set;
            get;
        }

        [Required]
        [Url]
        public string Url
        {
            set;
            get;
        }


    }
}
