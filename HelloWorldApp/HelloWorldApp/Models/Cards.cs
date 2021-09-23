using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorldApp.Models
{
    class Cards
    {
        public IList<Card> cards { get; set; }
        public int remaining { get; set; }
    }
}

//{
//    "success": true,
//    "cards": [
//        {
//        "image": "https://deckofcardsapi.com/static/img/KH.png",
//            "value": "KING",
//            "suit": "HEARTS",
//            "code": "KH"
//        },
//        {
//        "image": "https://deckofcardsapi.com/static/img/8C.png",
//            "value": "8",
//            "suit": "CLUBS",
//            "code": "8C"
//        }
//    ],
//    "deck_id":"3p40paa87x90",
//    "remaining": 50
//}