﻿using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kill_hunger.Models
{
    public class FileDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public String FileType { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
