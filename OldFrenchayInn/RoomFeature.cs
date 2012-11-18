using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldFrenchayInn {
    /// <summary>
    /// Represents a distinct feature of a room.
    /// </summary>
    public class RoomFeature {
        public static RoomFeature Get(RoomFeatures feature){
            switch (feature){
                case RoomFeatures.Internet:
                    return new RoomFeature("Internet Connection",
                                           "This snazzy internet connection will load your google in record times!");
                case RoomFeatures.TV:
                    return new RoomFeature("TV",
                                           "Never miss an episode of corrie again with this moving picture box (Also available in 4k HD)");
                case RoomFeatures.Minibar:
                    return new RoomFeature("Mini-bar", "Yes, we said mini. Enjoy your thimble.");
                default:
                    throw new IndexOutOfRangeException("Not a real feature");
            }
        }

        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public RoomFeature(string name, string description){
            Name = name;
            Description = description;
        }

        public override string ToString(){
            return Name;
        }
    }
}
