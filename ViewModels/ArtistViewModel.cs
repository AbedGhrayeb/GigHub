using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class ArtistViewModel
    {
        public IEnumerable<string> FollowingArtists { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
    }
}