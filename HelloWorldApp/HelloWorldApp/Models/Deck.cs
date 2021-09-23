using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldApp.Models
{
    class Deck
    {
        public string deck_id { get; set; }
        public int remaining { get; set; }
        public bool shuffled { get; set; }
    }
}
// {"success": true, "deck_id": "w2xyrn6kd0y3", "remaining": 52, "shuffled": true}