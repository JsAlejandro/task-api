using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using taskmanager_api.Models;
namespace taskmanager_api.ModelsResponse {
    public partial class CommentsResponse {
        public int Page  {get; set;}
        public int Pages {get; set;}
        public int Count {get; set;}
        public string  Next {get; set;}
        public string  Previous {get; set;}
        public  IEnumerable<CommentsShowDTO> Docs { get; set; }
    }
}