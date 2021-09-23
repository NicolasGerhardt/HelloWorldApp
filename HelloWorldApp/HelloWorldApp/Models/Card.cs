using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldApp.Models
{
    class Card
    {
        public string image { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
        public string code { get; set; }
    }
}


//        {
//            "image": "https://deckofcardsapi.com/static/img/KH.png",
//            "value": "KING",
//            "suit": "HEARTS",
//            "code": "KH"
//        }